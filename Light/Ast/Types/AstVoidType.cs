using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Types {
    public class AstVoidType : IAstTypeReference {
        public static AstVoidType Instance { get; private set; }

        static AstVoidType() {
            Instance = new AstVoidType();
        }

        private AstVoidType() {
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
