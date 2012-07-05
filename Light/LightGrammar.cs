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
            ConstructAll();
            SetAllRules();

            MarkPunctuation("[", "]");

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

        public NonTerminal Expression { get; private set; }
        public NonTerminal BinaryExpression { get; private set; }
        public NonTerminal BinaryOperator { get; private set; }
        public NonTerminal CommaSeparatedExpressionListStar { get; private set; }
        public NonTerminal ListInitializer { get; private set; }

        private void ConstructExpressions() {
            Number = new NumberLiteral("Number", NumberOptions.AllowSign, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            SingleQuotedString = new StringLiteral("SingleQuotedString", "'", StringOptions.None, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));

            Expression = Transient("Expression");
            BinaryExpression = NonTerminal("BinaryExpression", node => new BinaryExpression(
                (IAstElement) node.ChildNodes[0].AstNode,
                (BinaryOperator) node.ChildNodes[1].AstNode,
                (IAstElement) node.ChildNodes[2].AstNode
            ));

            BinaryOperator = NonTerminal("BinaryOperator", node => new BinaryOperator(node.FindTokenAndGetText()));

            CommaSeparatedExpressionListStar = Transient("CommaSeparatedExpressionListStar");
            ListInitializer = NonTerminal("ListInitializer", node => new ListInitializer(
                node.ChildNodes.Count > 0 ? node.FirstChild.ChildNodes.Select(c => (IAstElement)c.AstNode).ToArray() : new IAstElement[0]
            ));
        }

        private void SetExpressionRules() {
            Expression.Rule = SingleQuotedString | Number | BinaryExpression | ListInitializer;
            BinaryExpression.Rule = Expression + BinaryOperator + Expression;
            BinaryOperator.Rule = new[] {"+", "-", "*", "/"}.Select(k => (BnfExpression) ToTerm(k)).Aggregate((a, b) => a | b);

            CommaSeparatedExpressionListStar.Rule = MakeStarRule(CommaSeparatedExpressionListStar, ToTerm(","), Expression);
            ListInitializer.Rule = "[" + CommaSeparatedExpressionListStar + "]";
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

        private NonTerminal Transient(string name) {
            var nonTerminal = new NonTerminal(name);
            MarkTransient(nonTerminal);
            return nonTerminal;
        }

        private object NotImplemented() {
            throw new NotImplementedException();
        }
    }
}