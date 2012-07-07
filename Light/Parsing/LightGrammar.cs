using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.Literals;
using Light.Ast.Names;
using Light.Ast.Statements;
using Light.Internal;

namespace Light.Parsing {
    public class LightGrammar : Grammar {
        // top level:
        public NonTerminal<IAstElement> TopLevelRoot { get; private set; }
        public NonTerminal TopLevelElement { get; private set; }
        public NonTerminal<IAstElement[]> TopLevelElementList { get; private set; }

        public LightGrammar() {
            var lineComment = new CommentTerminal("LineComment", "#", "\n", "\r\n");
            NonGrammarTerminals.Add(lineComment);

            ConstructAll();
            SetAllRules();

            MarkPunctuation("[", "]", "(", ")", "{", "}", ":", "=>", ".");

            this.Root = TopLevelRoot;
            this.LanguageFlags = LanguageFlags.CreateAst;
        }

        private void ConstructAll() {
            ConstructIdentifiers();
            ConstructExpressions();
            ConstructStatements();
            ConstructDefinitions();

            TopLevelRoot = NonTerminal("TopLevelRoot", node => {
                var list = node.ChildAst(TopLevelElementList);
                if (list != null)
                    return new AstRoot(list);

                return (IAstElement)node.ChildAst(0);
            });
            TopLevelElement = Transient("TopLevelElement");
            TopLevelElementList = NonTerminal("TopLevelElementList", node => node.ChildNodes.Select(n => (IAstElement)n.AstNode).ToArray());
        }

        private void SetAllRules() {
            SetIdentifierRules();
            SetExpressionRules();
            SetStatementRules();
            SetDefinitionRules();

            TopLevelRoot.Rule = TopLevelElementList | Expression;
            TopLevelElementList.Rule = MakePlusRule(TopLevelElementList, NewLinePlus, TopLevelElement);
            TopLevelElement.Rule = Statement | Definition;
        }

        #region Identifiers

        public IdentifierTerminal Name { get; private set; }
        public NonTerminal<CompositeName> DotSeparatedName { get; private set; }

        public void ConstructIdentifiers() {
            Name = new IdentifierTerminal("Name");
            DotSeparatedName = NonTerminalWithNoElement("DotSeparatedName", n => new CompositeName(n.ChildNodes.Select(c => c.Token.Text).ToArray()));
        }

        public void SetIdentifierRules() {
            DotSeparatedName.Rule = MakePlusRule(DotSeparatedName, ToTerm("."), Name);
        }

        #endregion

        #region Expressions

        public NumberLiteral Number { get; private set; }
        public StringLiteral SingleQuotedString { get; private set; }
        public StringLiteral DoubleQuotedString { get; private set; }

        public NonTerminal<IAstElement> Expression { get; private set; }
        public NonTerminal<IAstElement> Literal { get; private set; }
        public NonTerminal<IAstElement> BinaryExpression { get; private set; }
        public NonTerminal<BinaryOperator> BinaryOperator { get; private set; }
        public NonTerminal<IAstElement[]> CommaSeparatedExpressionListStar { get; private set; }
        public NonTerminal<IAstElement> ListInitializer { get; private set; }
        public NonTerminal<IAstElement> ObjectInitializer { get; private set; }
        public NonTerminal ObjectInitializerElementList { get; private set; }
        public NonTerminal<IAstElement> ObjectInitializerElement { get; private set; }

        public NonTerminal<IAstElement> NewExpression { get; private set; }
        public NonTerminal<IAstElement> SimpleCallExpression { get; private set; }
        public NonTerminal<IAstElement> SimpleIndexerExpression { get; private set; }
        public NonTerminal<IAstElement> SimpleIdentifierExpression { get; private set; }
        public NonTerminal<IAstElement> MemberAccessOrCallExpression { get; private set; }
        public NonTerminal<IAstElement> MemberPathRoot { get; private set; }
        public NonTerminal<IAstElement> MemberPathElement { get; private set; }
        public NonTerminal<IAstElement[]> MemberPathElementListPlus { get; private set; }

        public NonTerminal<IAstElement> LambdaExpression { get; private set; }

        private void ConstructExpressions() {
            Number = new NumberLiteral("Number", NumberOptions.Default, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            SingleQuotedString = new StringLiteral("SingleQuotedString", "'", StringOptions.NoEscapes, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            DoubleQuotedString = new StringLiteral("DoubleQuotedString", "\"", StringOptions.NoEscapes, (c, node) => node.AstNode = new StringWithInterpolation((string)node.Token.Value));

            Literal = Transient("Literal");
            Expression = Transient("Expression");
            BinaryExpression = NonTerminal("BinaryExpression", node => new BinaryExpression(
                (IAstElement) node.ChildNodes[0].AstNode,
                (BinaryOperator) node.ChildNodes[1].AstNode,
                (IAstElement) node.ChildNodes[2].AstNode
            ));

            BinaryOperator = NonTerminalWithNoElement("BinaryOperator", node => new BinaryOperator(node.FindTokenAndGetText()));

            CommaSeparatedExpressionListStar = NonTerminal("CommaSeparatedExpressionListStar", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            ListInitializer = NonTerminal("ListInitializer", node => new ListInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializer = NonTerminal("ObjectInitializer", node => new ObjectInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializerElementList = new NonTerminal("ObjectInitializerElementList");
            ObjectInitializerElement = NonTerminal("ObjectInitializerElement", node => new ObjectInitializerEntry(
                node.ChildNodes[0].Token.Text,
                (IAstElement)node.ChildAst(1)
            ));

            MemberAccessOrCallExpression = NonTerminal("MemberAccessOrCall", node => {
                var path = (IAstElement[])node.ChildAst(1);
                var result = (IAstElement)node.ChildAst(0);
                foreach (var item in path) {
                    var call = item as CallExpression;
                    if (call != null) {
                        result = new CallExpression(result, call.MethodName, call.Arguments);
                        continue;
                    }

                    var identifier = item as IdentifierExpression;
                    if (identifier != null) {
                        result = new MemberExpression(result, identifier.Name);
                        continue;
                    }

                    var indexer = item as IndexerExpression;
                    if (indexer != null) {
                        result = new IndexerExpression(new MemberExpression(result, ((IdentifierExpression)indexer.Target).Name), indexer.Arguments.ToArray());
                        continue;
                    }

                    throw new NotImplementedException();
                }

                return result;
            });
            MemberPathRoot = Transient("MemberPathRoot");
            MemberPathElement = Transient("MemberPathElement");
            MemberPathElementListPlus = NonTerminal("MemberPathElementListPlus", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            SimpleCallExpression = NonTerminal("Call (Simple)", node => new CallExpression(null, node.Child(0).Token.Text, AstElementsInStarChild(node, 1)));
            SimpleIdentifierExpression = NonTerminal("Identifier", node => new IdentifierExpression(node.Child(0).Token.Text));
            SimpleIndexerExpression = NonTerminal("Indexer (Simple)", node => new IndexerExpression(node.ChildAst(SimpleIdentifierExpression), AstElementsInStarChild(node, 1)));

            NewExpression = NonTerminal(
                "NewExpression",
                node => new NewExpression(
                    node.ChildNodes[1].Token.Text,
                    node.ChildAst(CommaSeparatedExpressionListStar) ?? new IAstElement[0],
                    node.ChildAst(ObjectInitializer) 
                )
            );

            LambdaExpression = NonTerminal(
                "Lambda", node => (IAstElement)new LambdaExpression(new[] { (IAstElement)node.ChildAst(0) }, (IAstElement)node.ChildAst(1))
            );
        }

        private void SetExpressionRules() {
            Literal.Rule = SingleQuotedString | DoubleQuotedString | Number | ListInitializer | ObjectInitializer;
            Expression.Rule = Literal | BinaryExpression
                            | SimpleCallExpression | SimpleIdentifierExpression | SimpleIndexerExpression | MemberAccessOrCallExpression
                            | NewExpression | LambdaExpression;

            BinaryExpression.Rule = Expression + BinaryOperator + NewLineStar + Expression;
            BinaryOperator.Rule = new[] {"+", "-", "*", "/", "==", "==="}.Select(k => {
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
            MemberPathRoot.Rule = Literal | MemberPathElement;
            MemberPathElement.Rule = SimpleIdentifierExpression | SimpleCallExpression | SimpleIndexerExpression;
            MemberPathElementListPlus.Rule = MakeStarRule(MemberPathElementListPlus, ToTerm("."), MemberPathElement);
            SimpleCallExpression.Rule = Name + "(" + CommaSeparatedExpressionListStar + ")";
            SimpleIdentifierExpression.Rule = Name;
            SimpleIndexerExpression.Rule = SimpleIdentifierExpression + "[" + CommaSeparatedExpressionListStar + "]";
 
            NewExpression.Rule = "new" + Name + (("(" + CommaSeparatedExpressionListStar + ")") | Empty) + (ObjectInitializer | Empty);

            LambdaExpression.Rule = UntypedParameter + "=>" + Expression;
        }

        #endregion

        #region Statements

        public NonTerminal<IAstElement> Statement { get; private set; }
        public NonTerminal<IAstElement[]> StatementListPlus { get; private set; }
        public NonTerminal<IAstElement[]> OptionalBodyOfStatements { get; private set; }
        public NonTerminal<IAstElement> ForStatement { get; private set; }
        public NonTerminal<IAstElement> ContinueStatement { get; private set; }
        public NonTerminal<IAstElement> IfStatement { get; private set; }
        public NonTerminal<IAstElement> VariableDefinition { get; private set; }
        public NonTerminal<IAstElement> Assignment { get; private set; }
        public NonTerminal<IAstElement> AssignmentLeftHandSide { get; private set; }
        public NonTerminal<IAstElement> ReturnStatement { get; private set; }

        private void ConstructStatements() {
            Statement = Transient("Statement");
            StatementListPlus = NonTerminal("StatementListPlus", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            OptionalBodyOfStatements = NonTerminal("OptionalBodyOfStatements", node => (IAstElement[])node.ChildAst(0) ?? new IAstElement[0]);
            ForStatement = NonTerminal("For", node => new ForStatement(
                // for <1> in <3> do <5> end
                node.Child(1).Token.Text,
                (IAstElement)node.ChildAst(3),
                node.ChildAst(OptionalBodyOfStatements)
            ));
            ContinueStatement = NonTerminal("Continue", _ => new ContinueStatement());
            IfStatement = NonTerminal("If", node => new IfStatement(
                // if <1> \r\n <2>
                (IAstElement)node.ChildAst(1),
                (IAstElement)node.ChildAst(2)
            ));
            VariableDefinition = NonTerminal("VariableDefinition", node => new VariableDefinition(node.ChildNodes[1].Token.Text, null));
            Assignment = NonTerminal("Assignment", node => new Assignment((IAstElement)node.ChildAst(0), (IAstElement)node.ChildAst(2)));
            AssignmentLeftHandSide = Transient("AssignmentLeftHandSide");
            ReturnStatement = NonTerminal("Return", node => new ReturnStatement((IAstElement)node.ChildAst(1)));
        }

        private void SetStatementRules() {
            Statement.Rule = ForStatement | ContinueStatement | IfStatement | VariableDefinition | Assignment | ReturnStatement
                           | SimpleCallExpression | MemberAccessOrCallExpression;
            StatementListPlus.Rule = MakePlusRule(StatementListPlus, NewLinePlus, Statement);
            OptionalBodyOfStatements.Rule = StatementListPlus + NewLinePlus | Empty;
            
            ForStatement.Rule = "for" + Name + "in" + Expression + "do" + NewLinePlus + OptionalBodyOfStatements + "end";
            ContinueStatement.Rule = "continue";
            IfStatement.Rule = "if" + Expression + NewLineStar + Statement;
            VariableDefinition.Rule = "let" + Name + (Empty | "=" + Expression);
            Assignment.Rule = AssignmentLeftHandSide + "=" + Expression;
            AssignmentLeftHandSide.Rule = SimpleIdentifierExpression | MemberAccessOrCallExpression;
            ReturnStatement.Rule = "return" + Expression;
        }

        #endregion

        #region Definitions

        public NonTerminal<IAstElement> Definition { get; private set; }
        public NonTerminal<IAstElement> Import { get; private set; }
        public NonTerminal<IAstElement> Function { get; private set; }
        public NonTerminal<IAstElement> Parameter { get; private set; }
        public NonTerminal<IAstElement> TypedParameter { get; private set; }
        public NonTerminal<IAstElement> UntypedParameter { get; private set; }
        public NonTerminal<IAstElement[]> ParameterList { get; private set; }

        private void ConstructDefinitions() {
            Definition = Transient("Definition");
            Import = NonTerminal("Import", node => new ImportDefinition((CompositeName)node.ChildAst(1)));
            Function = NonTerminal(
                "Function",
                node => new FunctionDefinition(
                    node.Child(Name).Token.Text,
                    node.ChildAst(ParameterList),
                    node.ChildAst(OptionalBodyOfStatements)
                )
            );
            ParameterList = NonTerminal("ParameterList", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            Parameter = Transient("Parameter");
            TypedParameter = NonTerminal("TypedParameter", node => new ParameterDefinition(node.Child(1).Token.Text, (CompositeName)node.Child(0).AstNode));
            UntypedParameter = NonTerminal("UntypedParameter", node => new ParameterDefinition(node.FindTokenAndGetText(), null));
        }

        private void SetDefinitionRules() {
            Definition.Rule = Import | Function;
            Import.Rule = "import" + DotSeparatedName;
            Function.Rule = "function" + Name + "(" + ParameterList + ")" + NewLinePlus + OptionalBodyOfStatements + "end";
            ParameterList.Rule = MakeStarRule(ParameterList, ToTerm(","), Parameter);
            Parameter.Rule = TypedParameter | UntypedParameter;
            TypedParameter.Rule = DotSeparatedName + Name;
            UntypedParameter.Rule = Name;
        }

        #endregion

        private static NonTerminal<IAstElement> NonTerminal(string name, Func<ParseTreeNode, IAstElement> nodeCreator) {
            return new NonTerminal<IAstElement>(name, nodeCreator);
        }

        private static NonTerminal<IAstElement[]> NonTerminal(string name, Func<ParseTreeNode, IAstElement[]> nodeCreator) {
            return new NonTerminal<IAstElement[]>(name, nodeCreator);
        }

        private static NonTerminal<TAstNode> NonTerminalWithNoElement<TAstNode>(string name, Func<ParseTreeNode, TAstNode> nodeCreator) {
            return new NonTerminal<TAstNode>(name, nodeCreator);
        }

        private NonTerminal<IAstElement> Transient(string name) {
            var nonTerminal = new NonTerminal<IAstElement>(name);
            MarkTransient(nonTerminal);
            return nonTerminal;
        }

        private IAstElement[] AstElementsInStarChild(ParseTreeNode parent, int index) {
            var child = parent.ChildNodes.ElementAtOrDefault(index);
            if (child == null)
                return new IAstElement[0];

            return child.ChildNodes.Select(n => (IAstElement)n.AstNode).ToArray();
        }
    }
}