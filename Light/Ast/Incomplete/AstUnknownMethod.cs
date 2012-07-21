using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownMethod : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }
        public IAstTypeReference DeclaringType { get; set; }

        public AstUnknownMethod(string name) {
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstMethodReference Members

        IAstTypeReference IAstMethodReference.ReturnType {
            get { return AstUnknownType.WithNoName; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override string ToString() {
            return this.Name;
        }
    }
}