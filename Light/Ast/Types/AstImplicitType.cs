using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Types {
    public class AstImplicitType : IAstTypeReference {
        public static AstImplicitType Instance { get; private set; }

        static AstImplicitType() {
            Instance = new AstImplicitType();
        }

        private AstImplicitType() {
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
