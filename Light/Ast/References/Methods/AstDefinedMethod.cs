using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References.Types;

namespace Light.Ast.References.Methods {
    public class AstDefinedMethod : AstElementBase, IAstMethodReference {
        public AstFunctionDefinition Definition { get; private set; }
        public IList<IAstTypeReference> GenericArgumentTypes { get; private set; }
        private readonly ReadOnlyCollection<IAstTypeReference> parameterTypes;

        public AstDefinedMethod(AstFunctionDefinition definition) {
            this.Definition = definition;
            this.GenericArgumentTypes = new List<IAstTypeReference>();
            this.parameterTypes = definition.Parameters.Select(p => p.Type).ToArray().AsReadOnly();
        }

        public string Name {
            get { return this.Definition.Name; }
        }

        public bool IsGeneric {
            get { return this.GetGenericParameterTypes().Any(); }
        }

        public IEnumerable<IAstTypeReference> GetGenericParameterTypes() {
            return this.Definition.Parameters.Select(p => p.Type).Where(p => p is AstGenericPlaceholderType);
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

        #endregion
    }
}