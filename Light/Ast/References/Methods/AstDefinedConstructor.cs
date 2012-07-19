using System;
using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References.Methods {
    internal class AstDefinedConstructor : IAstConstructorReference {
        public AstConstructorDefinition Definition { get; private set; }

        public AstDefinedConstructor(AstConstructorDefinition definition) {
            this.Definition = definition;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Definition; }
        }

        #endregion

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}