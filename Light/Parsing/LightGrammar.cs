using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;
using Light.Ast.Names;
using Light.Ast.References;
using Light.Ast.Statements;
using Light.Framework;
using Light.Internal;

namespace Light.Parsing {
    public class LightGrammar : Grammar {
        // top level:
        public NonTerminal<IAstElement> TopLevelRoot { get; private set; }
        public NonTerminal TopLevelElement { get; private set; }
        public NonTerminal<IEnumerable<IAstElement>> TopLevelElementList { get; private set; }

        public LightGrammar() {
            var lineComment = new CommentTerminal("LineComment", "#", "\n", "\r\n");
            NonGrammarTerminals.Add(lineComment);

            ConstructAll();
            SetAllRules();

            MarkPunctuation("[", "]", "(", ")", "{", "}", ":", "=>", ".", "=", "..");

            RegisterOperators(1, "==");
            RegisterOperators(2, "mod");

            this.Root = TopLevelRoot;
            this.LanguageFlags = LanguageFlags.CreateAst;
        }

        private void ConstructAll() {
            ConstructNameAndTypeRules();
            ConstructExpressions();
            ConstructStatements();
            ConstructDefinitions();

            TopLevelRoot = NonTerminal("TopLevelRoot", node => {
                var list = node.FirstChild.ChildAst(TopLevelElementList);
                if (list != null)
                    return new AstRoot(list);

                return (IAstElement)node.FirstChild.ChildAst(0);
            });
            TopLevelElement = Transient<IAstElement>("TopLevelElement");
            TopLevelElementList = NonTerminal("TopLevelElementList", node => node.ChildAsts<IAstElement>());
        }

        private void SetAllRules() {
            SetNameAndTypeRules();
            SetExpressionRules();
            SetStatementRules();
            SetDefinitionRules();

            TopLevelRoot.Rule = (TopLevelElementList | Expression) + NewLineStar;
            TopLevelElementList.Rule = MakePlusRule(TopLevelElementList, NewLinePlus, TopLevelElement);
            TopLevelElement.Rule = Statement | Definition;
        }

        #region NamesAndTypes

        public IdentifierTerminal Name { get; private set; }
        public NonTerminal<CompositeName> DotSeparatedName { get; private set; }
        public NonTerminal<IAstTypeReference> TypeReference { get; private set; }
        public NonTerminal TypeReferenceListPlus { get; private set; }

        public void ConstructNameAndTypeRules() {
            Name = new IdentifierTerminal("Name");
            DotSeparatedName = NonTerminalThatIsNotAstElement("DotSeparatedName", n => new CompositeName(n.ChildNodes.Select(c => c.Token.Text).ToArray()));

            TypeReference = NonTerminalWithSpecificType("TypeReference", n => (IAstTypeReference)new AstUnknownType(n.Child(Name).Token.Text)); // WIP
            TypeReferenceListPlus = new NonTerminal("TypeReferenceListPlus");
        }

        public void SetNameAndTypeRules() {
            DotSeparatedName.Rule = MakePlusRule(DotSeparatedName, ToTerm("."), Name);

            TypeReference.Rule = Name + ("<" + TypeReferenceListPlus + ">" | Empty);
            TypeReferenceListPlus.Rule = MakePlusRule(TypeReferenceListPlus, ToTerm(","), TypeReference);

        }

        #endregion

        #region Expressions

        public NumberLiteral Number { get; private set; }
        public StringLiteral SingleQuotedString { get; private set; }
        public StringLiteral DoubleQuotedString { get; private set; }
        public NonTerminal<IAstExpression> Boolean { get; private set; }
        public NonTerminal<IAstExpression> Range { get; private set; }

        public NonTerminal<IAstExpression> Expression { get; private set; }
        public NonTerminal<IAstExpression> ExpressionSafeForAnyPosition { get; private set; }
        public NonTerminal<IAstExpression> ExpressionInBrackets { get; private set; }
        public NonTerminal<IAstExpression> Literal { get; private set; }
        public NonTerminal<IAstExpression> ThisExpression { get; private set; }
        public NonTerminal<IAstExpression> BinaryExpression { get; private set; }
        public NonTerminal<IAstMethodReference> BinaryOperator { get; private set; }
        public NonTerminal<IEnumerable<IAstExpression>> CommaSeparatedExpressionListStar { get; private set; }
        public NonTerminal<IAstExpression> ListInitializer { get; private set; }
        public NonTerminal<IAstExpression> ObjectInitializer { get; private set; }
        public NonTerminal ObjectInitializerElementList { get; private set; }
        public NonTerminal<IAstElement> ObjectInitializerElement { get; private set; }

        public NonTerminal<IAstExpression> NewExpression { get; private set; }
        public NonTerminal<IAstExpression> SimpleCallExpression { get; private set; }
        public NonTerminal<IEnumerable<IAstExpression>> ArgumentList { get; private set; }
        public NonTerminal<IAstExpression> SimpleIndexerExpression { get; private set; }
        public NonTerminal<IAstExpression> SimpleIdentifierExpression { get; private set; }
        public NonTerminal<IAstExpression> MemberAccessOrCallExpression { get; private set; }
        public NonTerminal<IAstElement> MemberPathRoot { get; private set; }
        public NonTerminal<IAstElement> MemberPathElement { get; private set; }
        public NonTerminal<IEnumerable<IAstElement>> MemberPathElementListPlus { get; private set; }

        public NonTerminal<IAstExpression> LambdaExpression { get; private set; }

        private void ConstructExpressions() {
            Number = new NumberLiteral("Number", NumberOptions.Default, (c, node) => node.AstNode = new PrimitiveValue(ConvertNumber(node.Token.Value))) {
                DefaultIntTypes = new[] { TypeCode.Int32, TypeCode.Int64, NumberLiteral.TypeCodeBigInt }
            };
            SingleQuotedString = new StringLiteral(
                "SingleQuotedString", "'", StringOptions.NoEscapes,
                ToActualNodeCreator(n => new PrimitiveValue(n.Token.Value))
            );
            DoubleQuotedString = new StringLiteral(
                "DoubleQuotedString", "\"", StringOptions.NoEscapes,
                ToActualNodeCreator(n => new StringWithInterpolation((string)n.Token.Value))
            );

            Boolean = NonTerminal("Boolean", n => new PrimitiveValue(n.FindTokenAndGetText() == "true"));
            Range = NonTerminal("Range", n => new AstRangeExpression((IAstExpression)n.ChildAst(0), (IAstExpression)n.ChildAst(1)));
            
            Literal = Transient<IAstExpression>("Literal");
            Expression = Transient<IAstExpression>("Expression");
            ExpressionSafeForAnyPosition = Transient<IAstExpression>("ExpressionSafeForAnyPosition");
            ExpressionInBrackets = Transient<IAstExpression>("ExpressionInBrackets");
            ThisExpression = NonTerminal("This", n => new AstThisExpression());

            BinaryExpression = NonTerminal("BinaryExpression", node => new BinaryExpression(
                (IAstExpression)node.ChildNodes[0].AstNode,
                (IAstMethodReference)node.ChildNodes[1].AstNode,
                (IAstExpression)node.ChildNodes[2].AstNode
            ));

            BinaryOperator = NonTerminalWithSpecificType<IAstMethodReference>("BinaryOperator", node => new AstUnknownMethod(node.FindTokenAndGetText()));

            CommaSeparatedExpressionListStar = NonTerminal("CommaSeparatedExpressionListStar", node => node.ChildAsts<IAstExpression>());
            ListInitializer = NonTerminal("ListInitializer", node => new AstListInitializer(AstElementsInStarChild(node, 0).Cast<IAstExpression>()));
            ObjectInitializer = NonTerminal("ObjectInitializer", node => new ObjectInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializerElementList = new NonTerminal("ObjectInitializerElementList");
            ObjectInitializerElement = NonTerminal("ObjectInitializerElement", node => new ObjectInitializerEntry(
                node.ChildNodes[0].Token.Text,
                (IAstElement)node.ChildAst(1)
            ));

            MemberAccessOrCallExpression = NonTerminal("MemberAccessOrCall", node => {
                var path = (IEnumerable<IAstElement>)node.ChildAst(1);
                var result = (IAstExpression)node.ChildAst(0);
                foreach (var item in path) {
                    var identifier = item as IdentifierExpression;
                    if (identifier != null) {
                        result = new MemberExpression(result, identifier.Name);
                        continue;
                    }

                    var call = item as CallExpression;
                    if (call != null) {
                        result = new CallExpression(new MemberExpression(result, ((IdentifierExpression)call.Callee).Name), call.Arguments);
                        continue;
                    }

                    var indexer = item as IndexerExpression;
                    if (indexer != null) {
                        result = new IndexerExpression(new MemberExpression(result, ((IdentifierExpression)indexer.Target).Name), indexer.Arguments);
                        continue;
                    }

                    throw new NotImplementedException("LightGrammar: MemberAccessOrCall path element is " + item + ".");
                }

                return result;
            });
            MemberPathRoot = Transient<IAstElement>("MemberPathRoot");
            MemberPathElement = Transient<IAstElement>("MemberPathElement");
            MemberPathElementListPlus = NonTerminal("MemberPathElementListPlus", node => node.ChildAsts<IAstElement>());
            SimpleCallExpression = NonTerminal("Call (Simple)", node => (IAstExpression)new CallExpression(new IdentifierExpression(node.Child(0).Token.Text), AstElementsInStarChild(node, 1).Cast<IAstExpression>()));
            SimpleIdentifierExpression = NonTerminal("Identifier", node => new IdentifierExpression(node.Child(0).Token.Text));
            SimpleIndexerExpression = NonTerminal("Indexer (Simple)", node => new IndexerExpression(node.ChildAst(SimpleIdentifierExpression), AstElementsInStarChild(node, 1)));

            NewExpression = NonTerminal(
                "NewExpression",
                node => new AstNewExpression(
                    node.ChildAst(TypeReference),
                    node.Child(2).ChildAst(CommaSeparatedExpressionListStar) ?? Enumerable.Empty<IAstExpression>(),
                    node.Child(3).ChildAst(ObjectInitializer) 
                )
            );

            ArgumentList = Transient<IEnumerable<IAstExpression>>("ArgumentList");

            LambdaExpression = NonTerminal("Lambda", node => {
                var parametersRaw = node.FirstChild.FirstChild.AstNode;
                var parameters = parametersRaw as IEnumerable<AstParameterDefinition>
                              ?? new[] { (AstParameterDefinition)parametersRaw };

                return new AstLambdaExpression(parameters, (IAstElement)node.ChildAst(1));
            });
        }

        private void SetExpressionRules() {
            Literal.Rule = SingleQuotedString | DoubleQuotedString | Number | Boolean | ListInitializer | ObjectInitializer;
            Boolean.Rule = ToTerm("true") | "false";

            Expression.Rule = ExpressionSafeForAnyPosition | Range | BinaryExpression | MemberAccessOrCallExpression | NewExpression | LambdaExpression;
            ExpressionSafeForAnyPosition.Rule = ExpressionInBrackets | Literal | ThisExpression
                                              | SimpleIdentifierExpression | SimpleIndexerExpression | SimpleCallExpression;
            ExpressionInBrackets.Rule = "(" + Expression + ")";

            Range.Rule = ExpressionSafeForAnyPosition + ".." + ExpressionSafeForAnyPosition;

            ThisExpression.Rule = ToTerm("this");

            BinaryExpression.Rule = Expression + BinaryOperator + NewLineStar + Expression;
            BinaryOperator.Rule = new[] {"+", "-", "*", "/", "mod", "==", "===", "<", ">", "|", "&", "or", "and"}.Select(k => {
                var @operator = (BnfExpression)ToTerm(k);
                @operator.SetFlag(TermFlags.IsOperator);
                return @operator;
            }).Aggregate((a, b) => a | b);

            ListInitializer.Rule = "[" + NewLineStar + CommaSeparatedExpressionListStar + NewLineStar + "]";
            CommaSeparatedExpressionListStar.Rule = MakeStarRule(CommaSeparatedExpressionListStar, ToTerm(",") + NewLineStar, Expression);

            ObjectInitializer.Rule = "{" + NewLineStar + ObjectInitializerElementList + NewLineStar + "}";
            ObjectInitializerElementList.Rule = MakeStarRule(ObjectInitializerElementList, ToTerm(",") + NewLineStar, ObjectInitializerElement);
            ObjectInitializerElement.Rule = Name + ":" + Expression;

            MemberAccessOrCallExpression.Rule = MemberPathRoot + "." + MemberPathElementListPlus;
            MemberPathRoot.Rule = Literal | MemberPathElement | ThisExpression | ExpressionInBrackets;
            MemberPathElement.Rule = SimpleIdentifierExpression | SimpleCallExpression | SimpleIndexerExpression;
            MemberPathElementListPlus.Rule = MakeStarRule(MemberPathElementListPlus, ToTerm("."), MemberPathElement);
            SimpleCallExpression.Rule = Name + ArgumentList;
            SimpleIdentifierExpression.Rule = Name;
            SimpleIndexerExpression.Rule = SimpleIdentifierExpression + "[" + CommaSeparatedExpressionListStar + "]";

            NewExpression.Rule = "new" + TypeReference + (ArgumentList | Empty) + (ObjectInitializer | Empty);
            ArgumentList.Rule = "(" + CommaSeparatedExpressionListStar + ")";

            LambdaExpression.Rule = (UntypedParameter | "(" + ParameterList + ")") + "=>" + Expression;
        }

        private static NonTerminal<IAstExpression> NonTerminal(string name, Func<ParseTreeNode, IAstExpression> nodeCreator) {
            return NonTerminalWithSpecificType(name, nodeCreator);
        }

        private static NonTerminal<IEnumerable<IAstExpression>> NonTerminal(string name, Func<ParseTreeNode, IEnumerable<IAstExpression>> nodeCreator) {
            return NonTerminalThatIsNotAstElement(name, nodeCreator);
        }

        #endregion

        #region Statements

        public NonTerminal<IAstStatement> Statement { get; private set; }
        public NonTerminal<IEnumerable<IAstStatement>> StatementListPlus { get; private set; }
        public NonTerminal<IEnumerable<IAstStatement>> OptionalBodyOfStatements { get; private set; }
        public NonTerminal<IAstStatement> ForStatement { get; private set; }
        public NonTerminal<IAstStatement> ContinueStatement { get; private set; }
        public NonTerminal<IAstStatement> IfOrUnlessStatement { get; private set; }
        public NonTerminal<IAstStatement> VariableDefinition { get; private set; }
        public NonTerminal<IAstExpression> OptionalAssignedValue { get; private set; }
        public NonTerminal<IAstStatement> Assignment { get; private set; }
        public NonTerminal<IAstElement> AssignmentLeftHandSide { get; private set; }
        public NonTerminal<IAstStatement> ReturnStatement { get; private set; }

        private void ConstructStatements() {
            Statement = Transient<IAstStatement>("Statement");
            StatementListPlus = NonTerminal("StatementListPlus", node => node.ChildAsts<IAstStatement>());
            OptionalBodyOfStatements = NonTerminal("OptionalBodyOfStatements", node => (IEnumerable<IAstStatement>)node.ChildAst(0) ?? Enumerable.Empty<IAstStatement>());
            ForStatement = NonTerminal("For", node => new ForStatement(
                // for <1> in <3> do <5> end
                node.Child(1).Token.Text,
                (IAstElement)node.ChildAst(3),
                node.ChildAst(OptionalBodyOfStatements)
            ));
            ContinueStatement = NonTerminal("Continue", _ => new ContinueStatement());
            IfOrUnlessStatement = NonTerminal("If", node => new IfOrUnlessStatement(
                // (if|unless) <1> \r\n <2>
                (IfOrUnlessKind)Enum.Parse(typeof(IfOrUnlessKind), node.Child(0).Child(0).Token.Text, true),
                (IAstElement)node.ChildAst(1),
                new[] { (IAstElement)node.ChildAst(2) }
            ));
            VariableDefinition = NonTerminal("VariableDefinition", node => new AstVariableDefinition(
                node.Child(1).Token.Text,
                AstImplicitType.Instance,
                node.ChildAst(OptionalAssignedValue)
            ));
            OptionalAssignedValue = NonTerminal("OptionalAssignedValue", node => (IAstExpression)node.ChildAst(0));
            Assignment = NonTerminal("Assignment", node => new AssignmentStatement((IAstAssignable)node.ChildAst(0), (IAstExpression)node.ChildAst(1)));
            AssignmentLeftHandSide = Transient<IAstElement>("AssignmentLeftHandSide");
            ReturnStatement = NonTerminal("Return", node => new AstReturnStatement((IAstExpression)node.ChildAst(1)));
        }

        private void SetStatementRules() {
            Statement.Rule = ForStatement | ContinueStatement | IfOrUnlessStatement | VariableDefinition | Assignment | ReturnStatement
                           | SimpleCallExpression | MemberAccessOrCallExpression;
            StatementListPlus.Rule = MakePlusRule(StatementListPlus, NewLinePlus, Statement);
            OptionalBodyOfStatements.Rule = StatementListPlus + NewLinePlus | Empty;
            
            ForStatement.Rule = "for" + Name + "in" + Expression + "do" + NewLinePlus + OptionalBodyOfStatements + "end";
            ContinueStatement.Rule = "continue";
            IfOrUnlessStatement.Rule = (ToTerm("if") | "unless") + Expression + NewLineStar + Statement;
            VariableDefinition.Rule = "let" + Name + OptionalAssignedValue;
            OptionalAssignedValue.Rule = Empty | "=" + Expression;
            Assignment.Rule = AssignmentLeftHandSide + "=" + Expression;
            AssignmentLeftHandSide.Rule = SimpleIdentifierExpression | MemberAccessOrCallExpression;
            ReturnStatement.Rule = "return" + Expression;
        }

        #endregion

        #region Definitions

        public NonTerminal<IAstDefinition> Definition { get; private set; }
        public NonTerminal<IAstElement> Import { get; private set; }
        public NonTerminal<IAstDefinition> TypeDefinition { get; private set; }
        public NonTerminal<IAstDefinition> TypeMember { get; private set; }
        public NonTerminal<IEnumerable<IAstDefinition>> TypeMemberList { get; private set; }
        public NonTerminal<IEnumerable<IAstDefinition>> OptionalTypeContent { get; private set; }
        public NonTerminal OptionalAccessLevel { get; private set; }
        public NonTerminal<IAstDefinition> Property { get; private set; }
        public NonTerminal<IAstDefinition> Constructor { get; private set; }
        public NonTerminal<IAstDefinition> Function { get; private set; }
        public NonTerminal<AstParameterDefinition> Parameter { get; private set; }
        public NonTerminal<AstParameterDefinition> TypedParameter { get; private set; }
        public NonTerminal<AstParameterDefinition> UntypedParameter { get; private set; }
        public NonTerminal<IEnumerable<AstParameterDefinition>> ParameterList { get; private set; }

        private void ConstructDefinitions() {
            Definition = Transient<IAstDefinition>("Definition");
            Import = NonTerminal("Import", node => new ImportDefinition((CompositeName)node.ChildAst(1)));
            TypeDefinition = NonTerminal("TypeDefinition", node => new AstTypeDefinition(
                node.ChildBefore(Name).FindTokenAndGetText(),
                node.Child(Name).Token.Text,
                node.ChildAst(OptionalTypeContent)
            ));
            TypeMember = Transient<IAstDefinition>("TypeMember");
            TypeMemberList = NonTerminal("TypeMemberList", node => node.ChildAsts<IAstDefinition>());
            OptionalTypeContent = NonTerminal("OptionalTypeContent", node => node.ChildAst(TypeMemberList) ?? Enumerable.Empty<IAstDefinition>());
            Property = NonTerminal("Property", node => new AstPropertyDefinition(
                node.Child(2).Token.Text,
                node.ChildAst(TypeReference),
                node.ChildAst(OptionalAssignedValue)
            ));
            Constructor = NonTerminal("Constructor", node => new AstConstructorDefinition(node.ChildAst(ParameterList), node.ChildAst(OptionalBodyOfStatements)));
            Function = NonTerminal(
                "Function",
                node =>  new AstFunctionDefinition(
                    node.Child(Name).Token.Text,
                    node.ChildAst(ParameterList),
                    node.ChildAst(OptionalBodyOfStatements),
                    AstImplicitType.Instance
                )
            );
            OptionalAccessLevel = new NonTerminal("OptionalAccessLevel");
            ParameterList = NonTerminalThatIsNotAstElement("ParameterList", node => node.ChildAsts<AstParameterDefinition>());
            Parameter = Transient<AstParameterDefinition>("Parameter");
            TypedParameter = NonTerminalWithSpecificType("TypedParameter", node => new AstParameterDefinition(node.Child(1).Token.Text, node.ChildAst(TypeReference)));
            UntypedParameter = NonTerminalWithSpecificType("UntypedParameter", node => new AstParameterDefinition(node.FindTokenAndGetText(), AstImplicitType.Instance));
        }

        private void SetDefinitionRules() {
            Definition.Rule = Import | TypeDefinition | Function;
            Import.Rule = "import" + DotSeparatedName;
            TypeDefinition.Rule = OptionalAccessLevel + (ToTerm("interface") | "class") + Name + NewLinePlus + OptionalTypeContent + "end";
            OptionalAccessLevel.Rule = (ToTerm("public") | "private" | Empty);
            TypeMember.Rule = Function | Constructor | Property;
            TypeMemberList.Rule = MakePlusRule(TypeMemberList, NewLinePlus, TypeMember);
            OptionalTypeContent.Rule = (TypeMemberList + NewLinePlus | Empty);
            Property.Rule = OptionalAccessLevel + TypeReference + Name + OptionalAssignedValue;
            Function.Rule = OptionalAccessLevel + "function" + Name + "(" + ParameterList + ")" + NewLinePlus + OptionalBodyOfStatements + "end";
            Constructor.Rule = OptionalAccessLevel + "new" + "(" + ParameterList + ")" + NewLinePlus + OptionalBodyOfStatements + "end";
            ParameterList.Rule = MakeStarRule(ParameterList, ToTerm(","), Parameter);
            Parameter.Rule = TypedParameter | UntypedParameter;
            TypedParameter.Rule = TypeReference + Name;
            UntypedParameter.Rule = Name;
        }

        private static NonTerminal<IAstDefinition> NonTerminal(string name, Func<ParseTreeNode, IAstDefinition> nodeCreator) {
            return NonTerminalWithSpecificType(name, nodeCreator);
        }

        private static NonTerminal<IEnumerable<IAstDefinition>> NonTerminal(string name, Func<ParseTreeNode, IEnumerable<IAstDefinition>> nodeCreator) {
            return NonTerminalThatIsNotAstElement(name, nodeCreator);
        }

        #endregion

        private static NonTerminal<IAstElement> NonTerminal(string name, Func<ParseTreeNode, IAstElement> nodeCreator) {
            return NonTerminalWithSpecificType(name, nodeCreator);
        }

        private static NonTerminal<IEnumerable<IAstElement>> NonTerminal(string name, Func<ParseTreeNode, IEnumerable<IAstElement>> nodeCreator) {
            return NonTerminalThatIsNotAstElement(name, nodeCreator);
        }

        private static NonTerminal<IAstStatement> NonTerminal(string name, Func<ParseTreeNode, IAstStatement> nodeCreator) {
            return NonTerminalWithSpecificType(name, nodeCreator);
        }

        private static NonTerminal<IEnumerable<IAstStatement>> NonTerminal(string name, Func<ParseTreeNode, IEnumerable<IAstStatement>> nodeCreator) {
            return NonTerminalThatIsNotAstElement(name, nodeCreator);
        }

        private static NonTerminal<TAstElement> NonTerminalWithSpecificType<TAstElement>(string name, Func<ParseTreeNode, TAstElement> nodeCreator)
            where TAstElement : class, IAstElement
        {
            return new NonTerminal<TAstElement>(name) {
                AstNodeCreator = ToActualNodeCreator(nodeCreator)
            };
        }

        private static NonTerminal<TAstNode> NonTerminalThatIsNotAstElement<TAstNode>(string name, Func<ParseTreeNode, TAstNode> nodeCreator) {
            return new NonTerminal<TAstNode>(name) {
                AstNodeCreator = (c, n) => n.AstNode = nodeCreator(n)
            };
        }

        private static AstNodeCreator ToActualNodeCreator<TAstElement>(Func<ParseTreeNode, TAstElement> nodeCreator) 
            where TAstElement : class, IAstElement
        {
            return (c, n) => {
                var element = nodeCreator(n);
                if (element == null)
                    return;

                element.SourceSpan = ParsingConverter.FromIrony(n.Span, c.Source.Text);
                n.AstNode = element;
            };
        }

        private NonTerminal<TAstNode> Transient<TAstNode>(string name) {
            var nonTerminal = new NonTerminal<TAstNode>(name);
            MarkTransient(nonTerminal);
            return nonTerminal;
        }

        private IEnumerable<IAstElement> AstElementsInStarChild(ParseTreeNode parent, int index) {
            var child = parent.ChildNodes.ElementAtOrDefault(index);
            if (child == null)
                return No.Elements;

            return child.ChildNodes.Select(n => (IAstElement)n.AstNode);
        }

        private object ConvertNumber(object value) {
            var bigInteger = value as Microsoft.Scripting.Math.BigInteger;
            if (!ReferenceEquals(bigInteger, null))
                return new Integer(bigInteger.ToByteArray());

            if (value is int)
                return new Integer((int)value);

            return value;
        }
    }
}