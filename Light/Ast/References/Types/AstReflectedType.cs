using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Errors;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;

namespace Light.Ast.References.Types {
    public class AstReflectedType : IAstTypeReference {
        public Type ActualType { get; private set; }

        public AstReflectedType(Type type) {
            Argument.RequireNotNull("type", type);
            this.ActualType = type;
        }

        public IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var method = this.ActualType.GetMethod(name, arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray());
            if (method == null)
                return new AstMissingMethod(name, this);

            return new AstReflectedMethod(method, this);
        }

        public override string ToString() {
            return this.ActualType.ToString();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        string IAstReference.Name {
            get { return this.ActualType.Name; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.ActualType; }
        }

        #endregion
    }
}
