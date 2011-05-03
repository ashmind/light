using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Irony.Parsing;

namespace Light {
    public class LightGrammar : Grammar {
        public LightGrammar() {
            var operators = new Dictionary<string, ExpressionType> {
                { "+", ExpressionType.Add }
            };

            // Literals
            var number = new NumberLiteral("number", NumberOptions.Default, (c, node) => node.AstNode = Expression.Constant(node.Token.Value));
            
            // NonTerminals
            var expression = NonTerminal("expression", node => node.FirstChild.AstNode);
            var binaryExpression = NonTerminal("binaryExpression", node => Expression.MakeBinary(
                (ExpressionType)node.ChildNodes[1].AstNode,
                (Expression)node.ChildNodes[0].AstNode,
                (Expression)node.ChildNodes[2].AstNode
            ));
            var binaryOperator = NonTerminal("binaryOperator", node => operators[node.FindTokenAndGetText()]);

            expression.Rule = number | binaryExpression;
            binaryExpression.Rule = expression + binaryOperator + expression;
            binaryOperator.Rule = operators.Keys.Select(k => (BnfExpression)ToTerm(k))
                                                .Aggregate((a, b) => a | b);

            this.Root = expression;
            this.LanguageFlags = LanguageFlags.CreateAst | LanguageFlags.NewLineBeforeEOF;
        }

        private static NonTerminal NonTerminal(string name, Func<ParseTreeNode, object> getAstNode) {
            return new NonTerminal(name, (c, node) => node.AstNode = getAstNode(node));
        }
    }
}