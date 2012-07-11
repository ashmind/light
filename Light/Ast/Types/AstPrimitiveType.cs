using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Types {
    public class AstPrimitiveType : IAstTypeReference {
        private Type type;

        public Type Type {
            get { return this.type; }
            set {
                Argument.RequireNotNull("value", value);
                this.type = value;
            }
        }

        public AstPrimitiveType(Type type) {
            this.Type = type;
        }

        public override string ToString() {
            return this.Type.ToString();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
