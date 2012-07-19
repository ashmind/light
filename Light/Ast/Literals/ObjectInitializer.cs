using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Ast.Literals {
    public class ObjectInitializer : IAstExpression {
        public ReadOnlyCollection<IAstElement> Elements { get; private set; }

        public ObjectInitializer(params IAstElement[] elements) {
            Argument.RequireNotNullAndNotContainsNull("elements", elements);
            Elements = elements.AsReadOnly();
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("ObjectInitializer.ExpressionType"); }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }

        #endregion

        public override string ToString() {
            return "{" + string.Join(", ", this.Elements) + "}";
        }
    }
}
