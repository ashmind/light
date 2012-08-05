using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class IdentifierExpression : AstElementBase, IAstExpression, IAstAssignable, IAstReference, IAstCallable {
        public string Name { get; private set; }

        public IdentifierExpression(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
        }

        public override string ToString() {
            return this.Name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return AstUnknownType.WithNoName; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        #region IAstCallable Members

        IAstTypeReference IAstCallable.ReturnType {
            get { return AstUnknownType.WithNoName; }
        }

        IEnumerable<IAstTypeReference> IAstCallable.ParameterTypes {
            get { throw new NotImplementedException("IdentifierExpression: IAstCallable.ParameterTypes"); }
        }

        #endregion
    }
}
