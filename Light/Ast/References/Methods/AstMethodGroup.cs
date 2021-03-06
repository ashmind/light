using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Light.Ast.Incomplete;

namespace Light.Ast.References.Methods {
    public class AstMethodGroup : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }
        public IAstMethodReference[] Methods { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }
        public MethodLocation Location { get; private set; }

        public AstMethodGroup(string name, IAstMethodReference[] methods) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullAndNotEmpty("methods", methods);

            this.Name = name;
            this.Methods = methods;

            var returnTypesCount = methods.Select(m => m.ReturnType).Distinct().Count();
            this.ReturnType = returnTypesCount == 1 ? methods[0].ReturnType : AstUnknownType.WithNoName;

            var locationsCount = methods.Select(m => m.Location).Distinct().Count();
            this.Location = locationsCount == 1 ? methods[0].Location : MethodLocation.Unknown;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Methods; }
        }

        #endregion

        #region IAstMethodReference Members

        IEnumerable<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { throw new NotImplementedException("AstMethodGroup.ParameterTypes"); }
        }

        bool IAstMethodReference.IsGeneric {
            get { throw new NotImplementedException("AstMethodGroup.IsGeneric"); }
        }

        IEnumerable<IAstTypeReference> IAstMethodReference.GetGenericParameterTypes() {
            throw new NotImplementedException("AstMethodGroup.GenericParameterTypes");
        }

        #endregion
    }
}