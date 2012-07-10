using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;

namespace Light.Processing {
    public class GenerateDefaultConstructors : IProcessingStep {
        public void Process(IAstElement root) {
            foreach (var type in root.Descendants<TypeDefinition>()) {
                if (type.Children<ConstructorDefinition>().Any())
                    continue;

                type.Members.Add(new ConstructorDefinition());
            }
        }
    }
}
