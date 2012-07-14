using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;

namespace Light.Processing.Steps {
    public class GenerateDefaultConstructors : ProcessingStepBase<TypeDefinition> {
        public override IAstElement ProcessBeforeChildren(TypeDefinition type, ProcessingContext context) {
            if (type.Children<ConstructorDefinition>().Any())
                return type;

            type.Members.Add(new ConstructorDefinition());
            return type;
        }
    }
}
