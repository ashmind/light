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

        public string Name {
            get { return this.Definition.Name; }
        }

        public bool IsGeneric {
            get { return false; } // not supported yet
        }

        public ReadOnlyCollection<IAstTypeReference> GenericParameterTypes {
            get { return No.Types; } // not supported yet
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

        MethodLocation IAstMethodReference.Location {
            get { throw new NotImplementedException("AstDefinedMethod.IsExtension"); }
        }

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { return this.parameterTypes; }
        }

        IAstMethodReference IAstMethodReference.WithGenericArguments(IEnumerable<IAstTypeReference> genericArguments) {
            throw new NotImplementedException("AstDefinedMethod.WithGenericArguments");
        }

        #endregion
    }
}