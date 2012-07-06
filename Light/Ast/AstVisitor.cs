using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Literals;

namespace Light.Ast {
    public abstract class AstVisitor<TContext> {
        private readonly IDictionary<Type, Action<IAstElement, TContext>> visitMethods = new Dictionary<Type, Action<IAstElement, TContext>>();

        protected AstVisitor() {
            Register<PrimitiveValue>(VisitPrimitiveValue);
            Register<BinaryExpression>(VisitBinaryExpression);
            Register<ListInitializer>(VisitListInitializer);
            Register<ObjectInitializer>(VisitObjectInitializer);
            Register<ObjectInitializerEntry>(VisitObjectInitializerEntry);
        }

        protected void Register<TAstElement>(Action<TAstElement, TContext> visit)
            where TAstElement : IAstElement
        {
            this.visitMethods.Add(typeof(TAstElement), (o, c) => visit((TAstElement)o, c));
        }

        protected void Visit(IEnumerable<IAstElement> elements, TContext context) {
            foreach (var element in elements) {
                Visit(element, context);
            }
        }

        protected void Visit(IAstElement element, TContext context) {
            var visit = this.visitMethods.GetValueOrDefault(element.GetType());
            if (visit == null)
                throw new NotSupportedException("Expression type " + element.GetType() + " is not supported.");

            visit(element, context);
        }

        protected virtual void VisitPrimitiveValue(PrimitiveValue value, TContext context) {
            throw new NotImplementedException();
        }

        protected virtual void VisitBinaryExpression(BinaryExpression binary, TContext context) {
            throw new NotImplementedException();
        }

        protected virtual void VisitListInitializer(ListInitializer initializer, TContext context) {
            throw new NotImplementedException();
        }

        protected virtual void VisitObjectInitializer(ObjectInitializer initializer, TContext context) {
            throw new NotImplementedException();
        }

        protected virtual void VisitObjectInitializerEntry(ObjectInitializerEntry entry, TContext context) {
            throw new NotImplementedException();
        }
    }
}
