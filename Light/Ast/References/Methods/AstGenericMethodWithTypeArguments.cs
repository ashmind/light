using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Processing.Helpers;

namespace Light.Ast.References.Methods {
    public class AstGenericMethodWithTypeArguments : AstElementBase, IAstMethodReference {
        public IAstMethodReference Actual { get; private set; }
        public ReadOnlyCollection<IAstTypeReference> ParameterTypes { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }
        public ReadOnlyCollection<IAstTypeReference> GenericArgumentTypes { get; private set; }

        public AstGenericMethodWithTypeArguments(IAstMethodReference actual, IList<IAstTypeReference> typeArguments, GenericTypeHelper genericHelper) {
            Argument.RequireNotNull("actual", actual);
            Argument.RequireNotNull("typeArguments", typeArguments);

            this.Actual = actual;
            this.GenericArgumentTypes = typeArguments.AsReadOnly();

            var genericParameterTypes = actual.GetGenericParameterTypes().ToArray();
            this.ParameterTypes = actual.ParameterTypes.Select(t => ApplyArgumentTypes(genericHelper, t, genericParameterTypes)).ToArray().AsReadOnly();
            this.ReturnType = ApplyArgumentTypes(genericHelper, actual.ReturnType, genericParameterTypes);
        }

        private IAstTypeReference ApplyArgumentTypes(GenericTypeHelper genericHelper, IAstTypeReference type, IAstTypeReference[] genericParameterTypes) {
            return genericHelper.RemapArgumentTypes(type, t => {
                var parameterIndex = genericParameterTypes.IndexOf(t);
                if (parameterIndex < 0)
                    return t;

                return this.GenericArgumentTypes[parameterIndex];
            });
        }

        public string Name {
            get { return this.Actual.Name; }
        }

        public bool IsGeneric {
            get { return true; }
        }
        
        public MethodLocation Location {
            get { return this.Actual.Location; }
        }

        public IEnumerable<IAstTypeReference> GetGenericParameterTypes() {
            return this.Actual.GetGenericParameterTypes();
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Actual.VisitOrTransformChildren(transform);
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion
    }
}