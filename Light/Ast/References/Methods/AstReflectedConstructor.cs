using System.Collections.Generic;
using System.Reflection;

namespace Light.Ast.References.Methods {
    public class AstReflectedConstructor : IAstConstructorReference {
        public ConstructorInfo Constructor { get; private set; }

        public AstReflectedConstructor(ConstructorInfo constructor) {
            this.Constructor = constructor;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Constructor; }
        }

        #endregion
    }
}