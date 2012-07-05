using System;
using System.Collections.Generic;
using System.Linq;

using Irony.Parsing;

using Light.Ast;
using Light.Ast.Literals;

namespace Light {
    public class LightGrammar : Grammar {
        public LightGrammar() {
            // Literals
            var number = new NumberLiteral("number", NumberOptions.AllowSign, (c, node) => node.AstNode = new PrimitiveValue(node.Token.Value));
            
            // NonTerminals
            var expression = NonTerminal("expression", node => node.FirstChild.AstNode);
            var binaryExpression = NonTerminal("binaryExpression", node => new BinaryExpression(
                (IAstElement)node.ChildNodes[0].AstNode,
                (BinaryOperator)node.ChildNodes[1].AstNode,
                (IAstElement)node.ChildNodes[2].AstNode
            ));
            var binaryOperator = NonTerminal("binaryOperator", node => new BinaryOperator(node.FindTokenAndGetText()));

            expression.Rule = number | binaryExpression;
            binaryExpression.Rule = expression + binaryOperator + expression;
            binaryOperator.Rule = new[] { "+", "-", "*", "/" }.Select(k => (BnfExpression)ToTerm(k)).Aggregate((a, b) => a | b);

            this.Root = expression;
            this.LanguageFlags = LanguageFlags.CreateAst | LanguageFlags.NewLineBeforeEOF;
        }

        private static NonTerminal NonTerminal(string name, Func<ParseTreeNode, object> getAstNode) {
            return new NonTerminal(name, (c, node) => node.AstNode = getAstNode(node));
        }
    }
}