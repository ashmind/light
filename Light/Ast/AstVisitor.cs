using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AshMind.Extensions;

namespace Light.Ast {
    public abstract class AstVisitor {
        private readonly IDictionary<Type, Func<Expression, object>> visitMethods = new Dictionary<Type, Func<Expression, object>>();

        protected AstVisitor() {
            Register<ConstantExpression>(VisitConstant);
            Register<BinaryExpression>(VisitBinary);
        }

        protected void Register<TExpression>(Func<TExpression, object> visit)
            where TExpression : Expression
        {
            this.visitMethods.Add(typeof(TExpression), o => visit((TExpression)o));
        }

        protected object Visit(Expression expression) {
            var visit = this.visitMethods.GetValueOrDefault(expression.GetType());
            if (visit == null)
                throw new NotSupportedException("Expression type " + expression.GetType() + " is not supported.");

            return visit(expression);
        }

        protected virtual object VisitConstant(ConstantExpression constant) {
            return null;
        }

        protected virtual object VisitBinary(BinaryExpression binary) {
            return null;
        }
    }
}
