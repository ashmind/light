﻿using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Light.Internal {
    public static class IronyExtensions {
        public static ParseTreeNode Child(this ParseTreeNode node, int index) {
            return node.ChildNodes.ElementAtOrDefault(index);
        }

        public static ParseTreeNode Child(this ParseTreeNode node, BnfTerm term) {
            return node.ChildNodes.SingleOrDefault(n => n.Term == term);
        }

        public static ParseTreeNode ChildBefore(this ParseTreeNode node, BnfTerm term) {
            return node.ChildNodes.TakeWhile(n => n.Term != term).Last();
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

        public static TAstNode ChildAst<TAstNode>(this ParseTreeNode node, NonTerminal<TAstNode> term) {
            return (TAstNode)node.ChildAst((BnfTerm)term);
        }
        public static IEnumerable<object> ChildAsts(this ParseTreeNode node) {
            return node.ChildNodes.Select(c => c.AstNode);
        }

        public static IEnumerable<TAstNode> ChildAsts<TAstNode>(this ParseTreeNode node) {
            return node.ChildNodes.Select(c => (TAstNode)c.AstNode);
        }
    }
}
