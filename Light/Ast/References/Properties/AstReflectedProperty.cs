using System.Collections.Generic;
using System.Reflection;
using Light.Ast.References.Types;

namespace Light.Ast.References.Properties {
    public class AstReflectedProperty : AstElementBase, IAstPropertyReference {
        public PropertyInfo Property { get; private set; }

        public AstReflectedProperty(PropertyInfo property) {
            Argument.RequireNotNull("property", property);
            this.Property = property;
            this.Type = new AstReflectedType(property.PropertyType);
        }

        public IAstTypeReference Type { get; private set; }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Property; }
        }

        #endregion
    }
}