using System;
using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References.Methods {
    public class AstDefinedMethod : AstElementBase, IAstMethodReference {
        private readonly IAstTypeReference declaringType;
        public AstFunctionDefinition Definition { get; private set; }

        public AstDefinedMethod(AstFunctionDefinition definition, IAstTypeReference declaringType) {
            this.declaringType = declaringType;
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