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

        public IEnumerable<IAstElement> Children() {
            return this.Elements;
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("ObjectInitializer.ExpressionType"); }
        }
    }
}
