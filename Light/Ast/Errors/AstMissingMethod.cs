using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Errors {
    public class AstMissingMethod : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }

        public AstMissingMethod(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
        }

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
    }
}
