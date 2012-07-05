using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Literals;

namespace Light.Ast {
    public abstract class AstVisitor<TContext> {
        private readonly IDictionary<Type, Func<IAstElement, TContext, object>> visitMethods = new Dictionary<Type, Func<IAstElement, TContext, object>>();

        protected AstVisitor() {
            Register<PrimitiveValue>(VisitPrimitiveValue);
            Register<BinaryExpression>(VisitBinaryExpression);
        }

        protected void Register<TAstElement>(Func<TAstElement, TContext, object> visit)
            where TAstElement : IAstElement
        {
            this.visitMethods.Add(typeof(TAstElement), (o, c) => visit((TAstElement)o, c));
        }

        protected object Visit(IAstElement element, TContext context) {
            var visit = this.visitMethods.GetValueOrDefault(element.GetType());
            if (visit == null)
                throw new NotSupportedException("Expression type " + element.GetType() + " is not supported.");

            return visit(element, context);
        }

        protected virtual object VisitPrimitiveValue(PrimitiveValue value, TContext context) {
            return null;
        }

        protected virtual object VisitBinaryExpression(BinaryExpression binary, TContext context) {
            return null;
        }
    }
}
