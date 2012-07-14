using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Processing;
using Light.Processing.Scoping;

namespace Light {
    public class LightProcessor {
        private readonly INameSource[] topLevelNameSources;
        private readonly IProcessingStep[] steps;

        public LightProcessor(INameSource[] topLevelNameSources, IProcessingStep[] steps) {
            this.topLevelNameSources = topLevelNameSources;
            this.steps = steps;
        }

        public IAstElement Process(IAstElement element) {
            var context = new ProcessingContext();
            foreach (var source in topLevelNameSources) {
                context.Scope.Add(source);
            }

            return Process(element, context);
        }

        private IAstElement Process(IAstElement element, ProcessingContext context) {
            foreach (var step in steps) {
                element = step.ProcessBeforeChildren(element, context);
            }
            element.TransformChildren(c => Process(c, context));
            foreach (var step in steps) {
                element = step.ProcessAfterChildren(element, context);
            }

            return element;
        }
    }
}
