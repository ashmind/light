using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Processing;

namespace Light {
    public class LightProcessor {
        private readonly IProcessingStep[] steps;

        public LightProcessor(IProcessingStep[] steps) {
            this.steps = steps;
        }

        public void Process(IAstElement element) {
            var context = new ProcessingContext();
            Process(element, context);
        }

        private void Process(IAstElement element, ProcessingContext context) {
            foreach (var step in steps) {
                step.ProcessBeforeChildren(element, context);
            }
            foreach (var child in element.Children()) {
                Process(child, context);
            }
            foreach (var step in steps) {
                step.ProcessAfterChildren(element, context);
            }
        }
    }
}
