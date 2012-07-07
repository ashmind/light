using System;
using System.Collections.Generic;
using System.Linq;

using Irony.Parsing;

using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Literals;
using Light.Ast.Names;
using Light.Ast.Statements;
using Light.Internal;

namespace Light {
    public class LightGrammar : Grammar {
        // top level:
        public NonTerminal TopLevelRoot { get; private set; }
        public NonTerminal TopLevelElement { get; private set; }
        public NonTerminal TopLevelElementList { get; private set; }

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
                var list = (IAstElement[])node.ChildAst(TopLevelElementList);
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
            TopLevelElement.Rule = Statement;
        }

        #region Identifiers

        public IdentifierTerminal Name { get; private set; }
        public NonTerminal DotSeparatedName { get; private set; }

        public void ConstructIdentifiers() {
            Name = new IdentifierTerminal("Name");
            DotSeparatedName = NonTerminal("DotSeparatedName", n => new CompositeName(n.ChildNodes.Select(c => c.Token.Text).ToArray()));
        }

        public void SetIdentifierRules() {
            DotSeparatedName.Rule = MakePlusRule(DotSeparatedName, ToTerm("."), Name);
        }

        #endregion

        #region Expressions

        public NumberLiteral Number { get; private set; }
        public StringLiteral SingleQuotedString { get; private set; }
        public StringLiteral DoubleQuotedString { get; private set; }

        public NonTerminal Expression { get; private set; }
        public NonTerminal Literal { get; private set; }
        public NonTerminal BinaryExpression { get; private set; }
        public NonTerminal BinaryOperator { get; private set; }
        public NonTerminal CommaSeparatedExpressionListStar { get; private set; }
        public NonTerminal ListInitializer { get; private set; }
        public NonTerminal ObjectInitializer { get; private set; }
        public NonTerminal ObjectInitializerElementList { get; private set; }
        public NonTerminal ObjectInitializerElement { get; private set; }

        public NonTerminal NewExpression { get; private set; }
        public NonTerminal SimpleCallExpression { get; private set; }
        public NonTerminal SimpleIdentifierExpression { get; private set; }
        public NonTerminal MemberAccessOrCallExpression { get; private set; }
        public NonTerminal MemberPathRoot { get; private set; }
        public NonTerminal MemberPathElement { get; private set; }
        public NonTerminal MemberPathElementListPlus { get; private set; }

        public NonTerminal LambdaExpression { get; private set; }

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

            BinaryOperator = NonTerminal("BinaryOperator", node => new BinaryOperator(node.FindTokenAndGetText()));

            CommaSeparatedExpressionListStar = NonTerminal("CommaSeparatedExpressionListStar", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            ListInitializer = NonTerminal("ListInitializer", node => new ListInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializer = NonTerminal("ObjectInitializer", node => new ObjectInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializerElementList = new NonTerminal("ObjectInitializerElementList");
            ObjectInitializerElement = NonTerminal("ObjectInitializerElement", node => new ObjectInitializerEntry(
                node.ChildNodes[0].Token.Text,
                (IAstElement)node.ChildNodes[1].AstNode
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
                }

                return result;
            });
            MemberPathRoot = Transient("MemberPathRoot");
            MemberPathElement = Transient("MemberPathElement");
            MemberPathElementListPlus = NonTerminal("MemberPathElementListPlus", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            SimpleCallExpression = NonTerminal("CallElement", node => new CallExpression(null, node.Child(0).Token.Text, AstElementsInStarChild(node, 2)));
            SimpleIdentifierExpression = NonTerminal("IdentifierElement", node => new IdentifierExpression(node.Child(0).Token.Text));

            NewExpression = NonTerminal(
                "NewExpression",
                node => new NewExpression(
                    node.ChildNodes[1].Token.Text,
                    ((IAstElement[])node.ChildAst(CommaSeparatedExpressionListStar)) ?? new IAstElement[0],
                    (IAstElement)node.ChildAst(ObjectInitializer) 
                )
            );

            LambdaExpression = NonTerminal(
                "Lambda", node => new LambdaExpression(new[] { (IAstElement)node.ChildAst(0) }, (IAstElement)node.ChildAst(1))
            );
        }

        private void SetExpressionRules() {
            Literal.Rule = SingleQuotedString | DoubleQuotedString | Number | ListInitializer | ObjectInitializer;
            Expression.Rule = Literal | BinaryExpression
                            | SimpleCallExpression | SimpleIdentifierExpression | MemberAccessOrCallExpression
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
            MemberPathElement.Rule = SimpleIdentifierExpression | SimpleCallExpression;
            MemberPathElementListPlus.Rule = MakeStarRule(MemberPathElementListPlus, ToTerm("."), MemberPathElement);
            SimpleCallExpression.Rule = Name + "(" + CommaSeparatedExpressionListStar + ")";
            SimpleIdentifierExpression.Rule = Name;
 
            NewExpression.Rule = "new" + Name + (("(" + CommaSeparatedExpressionListStar + ")") | Empty) + (ObjectInitializer | Empty);

            LambdaExpression.Rule = Parameter + "=>" + Expression;
        }

        #endregion

        #region Statements

        public NonTerminal Statement { get; private set; }
        public NonTerminal StatementListStar { get; private set; }
        public NonTerminal StatementListPlus { get; private set; }
        public NonTerminal ImportStatement { get; private set; }
        public NonTerminal ForStatement { get; private set; }
        public NonTerminal ContinueStatement { get; private set; }
        public NonTerminal IfStatement { get; private set; }
        public NonTerminal VariableDefinition { get; private set; }
        public NonTerminal Assignment { get; private set; }

        private void ConstructStatements() {
            Statement = Transient("Statement");
            StatementListStar = new NonTerminal("StatementListStar");
            StatementListPlus = NonTerminal("StatementListPlus", node => node.ChildAsts().Cast<IAstElement>().ToArray());
            ImportStatement = NonTerminal("ImportStatement", node => new ImportStatement((CompositeName)node.ChildAst(1)));
            ForStatement = NonTerminal("ForStatement", node => new ForStatement(
                // for <1> in <3> do <5> end
                node.Child(1).Token.Text,
                (IAstElement)node.ChildAst(3),
                AstElementsInStarChild(node.Child(5), 0)
            ));
            ContinueStatement = NonTerminal("ContinueStatement", _ => new ContinueStatement());
            IfStatement = NonTerminal("IfStatement", node => new IfStatement(
                // if <1> \r\n <2>
                (IAstElement)node.ChildAst(1),
                (IAstElement)node.ChildAst(2)
            ));
            VariableDefinition = NonTerminal("VariableDefinition", node => new VariableDefinition(node.ChildNodes[1].Token.Text, null));
            Assignment = NonTerminal("Assignment", node => new Assignment((IAstElement)node.ChildAst(0), (IAstElement)node.ChildAst(2)));
        }

        private void SetStatementRules() {
            Statement.Rule = ImportStatement | ForStatement | ContinueStatement | IfStatement | VariableDefinition | Assignment
                           | SimpleCallExpression | MemberAccessOrCallExpression;
            StatementListStar.Rule = MakeStarRule(StatementListStar, NewLinePlus, Statement);
            StatementListPlus.Rule = MakePlusRule(StatementListPlus, NewLinePlus, Statement);

            ImportStatement.Rule = "import" + DotSeparatedName;
            ForStatement.Rule = "for" + Name + "in" + Expression + "do" + NewLinePlus + (StatementListPlus + NewLinePlus | Empty) + "end";
            ContinueStatement.Rule = "continue";
            IfStatement.Rule = "if" + Expression + NewLineStar + Statement;
            VariableDefinition.Rule = "let" + Name + (Empty | "=" + Expression);
            Assignment.Rule = MemberAccessOrCallExpression + "=" + Expression;
        }

        #endregion

        #region Definitions

        public NonTerminal Parameter { get; private set; }

        public void ConstructDefinitions() {
            Parameter = NonTerminal("Parameter", node => new ParameterDefinition(node.FindTokenAndGetText(), null));
        }

        public void SetDefinitionRules() {
            Parameter.Rule = Name;
        }

        #endregion

        private static NonTerminal NonTerminal(string name, Func<ParseTreeNode, object> getAstNode) {
            return new NonTerminal(name, (c, node) => node.AstNode = getAstNode(node));
        }

        private NonTerminal Transient(string name) {
            var nonTerminal = new NonTerminal(name);
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