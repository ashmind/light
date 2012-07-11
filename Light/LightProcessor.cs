using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Processing;

namespace Light {
    public class LightProcessor {
        private readonly IProcessingStep[] steps;

        // TODO: Better discovery/IoC support
        public LightProcessor()
            : this(new IProcessingStep[] {
                new GenerateDefaultConstructors(),
                new GenerateReturns(),
                new InferFunctionReturnTypes()
            })
        {
        }

        public LightProcessor(IProcessingStep[] steps) {
            this.steps = steps;
        }

        public void Process(IAstElement root) {
            foreach (var step in steps) {
                step.Process(root);
            }
        }
    }
}
