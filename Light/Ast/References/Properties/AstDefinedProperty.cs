using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References.Properties {
    public class AstDefinedProperty : AstElementBase, IAstPropertyReference {
        public AstPropertyDefinition Property { get; private set; }

        public AstDefinedProperty(AstPropertyDefinition property) {
            Argument.RequireNotNull("property", property);
            this.Property = property;
        }

        public IAstTypeReference Type {
            get { return this.Property.Type; }
        }

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