using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Mono.Cecil;

namespace Light.Compilation.Definitions {
    public class PropertyOrFieldDefinitionBuilder : DefinitionBuilderBase<AstPropertyDefinition, TypeDefinition> {
        public override void Build(AstPropertyDefinition propertyOrField, TypeDefinition typeDefinition, DefinitionBuildingContext context) {
            var type = context.ConvertReference(propertyOrField.Type);
            var fieldDefinition = new FieldDefinition(propertyOrField.Name, FieldAttributes.Private, type);
            
            typeDefinition.Fields.Add(fieldDefinition);
            context.MapDefinition(propertyOrField, fieldDefinition);
        }
    }
}
