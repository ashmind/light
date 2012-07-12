using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;

namespace Light.Ast {
    public abstract class AstVisitor<TContext> {
        private readonly IDictionary<Type, Action<IAstElement, TContext>> visitMethods = new Dictionary<Type, Action<IAstElement, TContext>>();

        protected AstVisitor() {
            Register<AstRoot>(VisitAstRoot);
            Register<PrimitiveValue>(VisitPrimitiveValue);
            Register<BinaryExpression>(VisitBinaryExpression);
            Register<IdentifierExpression>(VisitIdentifierExpression);
            Register<ListInitializer>(VisitListInitializer);
            Register<ObjectInitializer>(VisitObjectInitializer);
            Register<ObjectInitializerEntry>(VisitObjectInitializerEntry);
            Register<CallExpression>(VisitCallExpression);
            Register<IndexerExpression>(VisitIndexerExpression);
            Register<MemberExpression>(VisitMemberExpression);
        }

        protected void Register<TAstElement>(Action<TAstElement, TContext> visit)
            where TAstElement : IAstElement
        {
            this.visitMethods.Add(typeof(TAstElement), (o, c) => visit((TAstElement)o, c));
        }

        protected virtual void Visit(IAstElement element, TContext context) {
            var visit = this.visitMethods.GetValueOrDefault(element.GetType());
            if (visit == null) {
                VisitNotRecognized(element, context);
                return;
            }

            visit(element, context);
        }

        protected virtual void VisitAstRoot(AstRoot root, TContext context) {
            foreach (var element in root.Elements) {
                Visit(element, context);
            }
        }

        protected virtual void VisitPrimitiveValue(PrimitiveValue value, TContext context) {
        }

        protected virtual void VisitBinaryExpression(BinaryExpression binary, TContext context) {
        }

        protected virtual void VisitListInitializer(ListInitializer initializer, TContext context) {
        }

        protected virtual void VisitObjectInitializer(ObjectInitializer initializer, TContext context) {
        }

        protected virtual void VisitObjectInitializerEntry(ObjectInitializerEntry entry, TContext context) {
        }

        protected virtual void VisitCallExpression(CallExpression call, TContext context) {
        }

        protected virtual void VisitIndexerExpression(IndexerExpression indexer, TContext context) {
        }

        protected virtual void VisitIdentifierExpression(IdentifierExpression identifier, TContext context) {
        }

        protected virtual void VisitMemberExpression(MemberExpression member, TContext context) {
        }

        protected virtual void VisitNotRecognized(IAstElement element, TContext context) {
            throw new NotSupportedException("Expression type " + element.GetType() + " is not supported.");
        }
    }
}
