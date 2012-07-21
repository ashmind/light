using System;
using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References.Methods {
    public class AstDefinedConstructor : AstElementBase, IAstConstructorReference {
        public AstConstructorDefinition Definition { get; private set; }

        public AstDefinedConstructor(AstConstructorDefinition definition) {
            this.Definition = definition;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Definition; }
        }

        #endregion
    }
}