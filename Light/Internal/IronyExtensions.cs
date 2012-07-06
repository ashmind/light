using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace Light.Internal {
    public static class IronyExtensions {
        public static ParseTreeNode Child(this ParseTreeNode node, int index) {
            return node.ChildNodes.ElementAtOrDefault(index);
        }

        public static ParseTreeNode Child(this ParseTreeNode node, BnfTerm term) {
            return node.ChildNodes.SingleOrDefault(n => n.Term == term);
        }

        public static object ChildAst(this ParseTreeNode node, int index) {
            var child = node.Child(index);
            if (child == null)
                return null;

            return child.AstNode;
        }

        public static object ChildAst(this ParseTreeNode node, BnfTerm term) {
            var child = node.Child(term);
            if (child == null)
                return null;

            return child.AstNode;
        }

        public static IEnumerable<object> ChildAsts(this ParseTreeNode node) {
            return node.ChildNodes.Select(c => c.AstNode);
        }
    }
}
