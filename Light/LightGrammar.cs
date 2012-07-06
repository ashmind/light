using System;
using System.Collections.Generic;
using System.Linq;

using Irony.Parsing;

using Light.Ast;
using Light.Ast.Literals;
using Light.Ast.Names;
using Light.Ast.Statements;

namespace Light {
    public class LightGrammar : Grammar {
        public Terminal Dot { get; private set; }

        // top level:
        public NonTerminal TopLevelElement { get; private set; }
        public NonTerminal TopLevelElementList { get; private set; }

        public LightGrammar() {
            var lineComment = new CommentTerminal("line_comment", "#", "\n", "\r\n");
            NonGrammarTerminals.Add(lineComment);

            ConstructAll();
            SetAllRules();

            MarkPunctuation("[", "]", "(", ")", "{", "}", ":");

            this.Root = TopLevelElementList;
            this.LanguageFlags = LanguageFlags.CreateAst;
        }

        private void ConstructAll() {
            Dot = new KeyTerm(".", ".");

            ConstructIdentifiers();
            ConstructExpressions();
            ConstructStatements();

            TopLevelElement = NonTerminal("TopLevelElement", node => node.FirstChild.AstNode);
            TopLevelElementList = NonTerminal("TopLevelElementList", node => node.ChildNodes.Select(n => (IAstElement)n.AstNode).ToArray());
        }

        private void SetAllRules() {
            SetExpressionRules();
            SetStatementRules();
            SetIdentifierRules();

            TopLevelElement.Rule = Statement;
            TopLevelElementList.Rule = MakePlusRule(TopLevelElementList, NewLinePlus, TopLevelElement);
        }

        #region Identifiers

        public IdentifierTerminal Name { get; private set; }
        public NonTerminal DotSeparatedName { get; private set; }

        public void ConstructIdentifiers() {
            Name = new IdentifierTerminal("Name");
            DotSeparatedName = NonTerminal("DotSeparatedName", n => new CompositeName(n.ChildNodes.Select(c => c.Token.Text).ToArray()));
        }

        public void SetIdentifierRules() {
            DotSeparatedName.Rule = MakePlusRule(DotSeparatedName, Dot, Name);
        }

        #endregion

        #region Expressions

        public NumberLiteral Number { get; private set; }
        public StringLiteral SingleQuotedString { get; private set; }
        public StringLiteral DoubleQuotedString { get; private set; }

        public NonTerminal Expression { get; private set; }
        public NonTerminal IdentifierExpression { get; private set; }
        public NonTerminal BinaryExpression { get; private set; }
        public NonTerminal BinaryOperator { get; private set; }
        public NonTerminal CommaSeparatedExpressionListStar { get; private set; }
        public NonTerminal ListInitializer { get; private set; }
        public NonTerminal ObjectInitializer { get; private set; }
        public NonTerminal ObjectInitializerElementList { get; private set; }
        public NonTerminal ObjectInitializerElement { get; private set; }
        public NonTerminal NewExpression { get; private set; }

        private void ConstructExpressions() {
            Number = new NumberLiteral("Number", NumberOptions.AllowSign, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            SingleQuotedString = new StringLiteral("SingleQuotedString", "'", StringOptions.None, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            DoubleQuotedString = new StringLiteral("DoubleQuotedString", "\"", StringOptions.None, (c, node) => node.AstNode = new StringWithInterpolation((string)node.Token.Value));

            Expression = Transient("Expression");
            IdentifierExpression = NonTerminal("Identifier", node => new IdentifierExpression((CompositeName)node.ChildNodes[0].AstNode));
            BinaryExpression = NonTerminal("BinaryExpression", node => new BinaryExpression(
                (IAstElement) node.ChildNodes[0].AstNode,
                (BinaryOperator) node.ChildNodes[1].AstNode,
                (IAstElement) node.ChildNodes[2].AstNode
            ));

            BinaryOperator = NonTerminal("BinaryOperator", node => new BinaryOperator(node.FindTokenAndGetText()));

            CommaSeparatedExpressionListStar = new NonTerminal("CommaSeparatedExpressionListStar");
            ListInitializer = NonTerminal("ListInitializer", node => new ListInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializer = NonTerminal("ObjectInitializer", node => new ObjectInitializer(AstElementsInStarChild(node, 0)));
            ObjectInitializerElementList = new NonTerminal("ObjectInitializerElementList");
            ObjectInitializerElement = NonTerminal("ObjectInitializerElement", node => new ObjectInitializerEntry(
                node.ChildNodes[0].Token.Text,
                (IAstElement)node.ChildNodes[1].AstNode
            ));

            NewExpression = NonTerminal(
                "NewExpression",
                node => new NewExpression(node.ChildNodes[1].Token.Text, AstElementsInStarChild(node, 2), (IAstElement)node.ChildNodes[3].AstNode)
            );
        }

        private void SetExpressionRules() {
            Expression.Rule = SingleQuotedString | DoubleQuotedString | Number | BinaryExpression | ListInitializer | ObjectInitializer | NewExpression | IdentifierExpression;
            IdentifierExpression.Rule = DotSeparatedName;

            BinaryExpression.Rule = Expression + BinaryOperator + NewLineStar + Expression;
            BinaryOperator.Rule = new[] {"+", "-", "*", "/"}.Select(k => (BnfExpression) ToTerm(k)).Aggregate((a, b) => a | b);

            ListInitializer.Rule = "[" + NewLineStar + CommaSeparatedExpressionListStar + NewLineStar + "]";
            CommaSeparatedExpressionListStar.Rule = MakeStarRule(CommaSeparatedExpressionListStar, ToTerm(",") + NewLineStar, Expression);

            ObjectInitializer.Rule = "{" + NewLineStar + ObjectInitializerElementList + NewLineStar + "}";
            ObjectInitializerElementList.Rule = MakeStarRule(ObjectInitializerElementList, ToTerm(",") + NewLineStar, ObjectInitializerElement);
            ObjectInitializerElement.Rule = Name + ":" + Expression;

            NewExpression.Rule = "new" + Name + "(" + CommaSeparatedExpressionListStar + ")" + (ObjectInitializer | Empty);
        }

        #endregion

        #region Statements

        public NonTerminal Statement { get; private set; }
        public NonTerminal ImportStatement { get; private set; }
        public NonTerminal VariableDefinition { get; private set; }

        private void ConstructStatements() {
            Statement = Transient("Statement");
            ImportStatement = NonTerminal("ImportStatement", node => new ImportStatement((CompositeName)node.ChildNodes[1].AstNode));
            VariableDefinition = NonTerminal("VariableDefinition", node => new VariableDefinition(node.ChildNodes[1].Token.Text, null));
        }

        private void SetStatementRules() {
            ImportStatement.Rule = "import" + DotSeparatedName;
            VariableDefinition.Rule = "let" + Name + (Empty | "=" + Expression);
            Statement.Rule = ImportStatement | VariableDefinition | Expression;
        }

        #endregion

        private static NonTerminal NonTerminal(string name, Func<ParseTreeNode, object> getAstNode) {
            return new NonTerminal(name, (c, node) => node.AstNode = getAstNode(node));
        }

        //private static NonTerminal NonTerminalPlus(string name, BnfExpression listMember, Func<ParseTreeNode, object> getAstNode) {
        //    var nonTerminal = NonTerminalWithoutRule(name, getAstNode);
        //    MakePlusRule(nonTerminal, listMember);
        //    return nonTerminal;
        //}

        //private static NonTerminal NonTerminalPlus(string name, BnfExpression listMember, BnfExpression delimiter, Func<ParseTreeNode, object> getAstNode) {
        //    var nonTerminal = NonTerminalWithoutRule(name, getAstNode);
        //    MakePlusRule(nonTerminal, delimiter, listMember);
        //    return nonTerminal;
        //}

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

        private object NotImplemented() {
            throw new NotImplementedException();
        }
    }
}