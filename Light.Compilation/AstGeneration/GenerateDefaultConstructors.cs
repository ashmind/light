using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Processing;

namespace Light.Compilation.AstGeneration {
    public class GenerateDefaultConstructors : ProcessingStepBase<AstTypeDefinition> {
        public GenerateDefaultConstructors() : base(ProcessingStage.Compilation) {
        }

        public override IAstElement ProcessBeforeChildren(AstTypeDefinition type, ProcessingContext context) {
            if (type.Children<AstConstructorDefinition>().Any())
                return type;

            type.Members.Add(new AstConstructorDefinition());
            return type;
        }
    }
}
