using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Semantic;

namespace Light {
    public class LightProcessor {
        private readonly IProcessingStep[] steps;

        public LightProcessor(IProcessingStep[] steps) {
            this.steps = steps;
        }

        public void Process(IAstElement[] tree) {
            
        }
    }
}
