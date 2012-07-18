using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Errors;
using Light.Ast.References.Methods;

namespace Light.Ast.References.Types {
    public class AstReflectedType : IAstTypeReference {
        public Type ActualType { get; private set; }

        public AstReflectedType(Type type) {
            Argument.RequireNotNull("type", type);
            this.ActualType = type;
        }

        public virtual IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var types = arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray();
            var method = this.ActualType.GetMethod(name, types);
            if (method == null)
                return new AstMissingMethod(name, this);

            return new AstReflectedMethod(method, this);
        }

        public IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            var constructor = this.ActualType.GetConstructor(arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray());
            if (constructor == null)
                return null;

            return new AstReflectedConstructor(constructor);
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

        string IAstTypeReference.Name {
            get { return this.ActualType.Name; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.ActualType; }
        }

        #endregion

        public override bool Equals(object obj) {
            return this.Equals(obj as AstReflectedType);
        }

        public bool Equals(AstReflectedType type) {
            return type != null
                && Equals(this.ActualType, type.ActualType);
        }

        public override int GetHashCode() {
            return this.ActualType.GetHashCode();
        }
    }
}
