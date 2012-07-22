using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Light.Ast.Incomplete;

namespace Light.Ast.References.Methods {
    public class AstMethodGroup : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }
        public IAstMethodReference[] Methods { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }

        public AstMethodGroup(string name, IAstMethodReference[] methods) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNull("methods", methods);

            this.Name = name;
            this.Methods = methods;
            this.ReturnType = AstUnknownType.WithNoName;
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

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { throw new NotImplementedException("AstMethodGroup.ParameterTypes"); }
        }

        #endregion
    }
}