using System;
using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References.Methods {
    internal class AstDefinedMethod : IAstMethodReference {
        private readonly IAstTypeReference declaringType;
        public AstFunctionDefinition Definition { get; private set; }

        public AstDefinedMethod(AstFunctionDefinition definition, IAstTypeReference declaringType) {
            this.declaringType = declaringType;
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

        #region IAstMethodReference Members

        IAstTypeReference IAstMethodReference.ReturnType {
            get { return this.Definition.ReturnType; }
        }

        IAstTypeReference IAstMethodReference.DeclaringType {
            get { return this.declaringType; }
        }

        string IAstMethodReference.Name {
            get { return this.Definition.Name; }
        }

        #endregion
    }
}