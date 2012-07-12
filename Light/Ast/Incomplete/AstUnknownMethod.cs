using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownMethod : IAstMethodReference {
        public string Name { get; private set; }
        public IAstTypeReference DeclaringType { get; set; }

        public AstUnknownMethod(string name) {
            this.Name = name;
        }

        public IEnumerable<IAstElement> Children() {
            yield break;
        }

        #region IAstMethodReference Members

        IAstTypeReference IAstMethodReference.ReturnType {
            get { return null; }
        }

        #endregion
    }
}