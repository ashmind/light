using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;

namespace Light.Ast.References.Methods {
    public class AstDefinedMethod : AstElementBase, IAstMethodReference {
        public AstFunctionDefinition Definition { get; private set; }

        private readonly ReadOnlyCollection<IAstTypeReference> parameterTypes;

        public AstDefinedMethod(AstFunctionDefinition definition) {
            this.Definition = definition;
            this.parameterTypes = definition.Parameters.Select(p => p.Type).ToArray().AsReadOnly();
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

        string IAstMethodReference.Name {
            get { return this.Definition.Name; }
        }

        MethodLocation IAstMethodReference.Location {
            get { throw new NotImplementedException("AstDefinedMethod.IsExtension"); }
        }

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { return this.parameterTypes; }
        }

        #endregion
    }
}