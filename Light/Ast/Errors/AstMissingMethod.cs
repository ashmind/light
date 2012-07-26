using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Errors {
    public class AstMissingMethod : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }

        public AstMissingMethod(string name, IList<IAstTypeReference> parameterTypes) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullNotEmptyAndNotContainsNull("parameterTypes", parameterTypes);

            this.Name = name;
            this.ParameterTypes = parameterTypes.AsReadOnly();
        }

        public ReadOnlyCollection<IAstTypeReference> ParameterTypes { get; private set; }

        public IAstTypeReference ReturnType {
            get { return AstUnknownType.WithNoName; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        #region IAstMethodReference Members

        MethodLocation IAstMethodReference.Location {
            get { return MethodLocation.Unknown; }
        }

        #endregion
    }
}
