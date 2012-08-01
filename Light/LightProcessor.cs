using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Processing;
using Light.Processing.Scoping;

namespace Light {
    public class LightProcessor {
        private readonly INameSource[] topLevelNameSources;
        private readonly IProcessingStep[] allSteps;

        public LightProcessor(INameSource[] topLevelNameSources, IEnumerable<IProcessingStep> allSteps) {
            this.topLevelNameSources = topLevelNameSources;
            this.allSteps = allSteps.OrderBy(s => s.Stage).ToArray();
        }

        public IAstElement Process(IAstElement element, ProcessingOptions options = null) {
            options = options ?? new ProcessingOptions();

            var context = new ProcessingContext(options);
            foreach (var source in topLevelNameSources) {
                context.Scope.Add(source);
            }
            var steps = this.allSteps.Where(s => options.Stages.Contains(s.Stage)).ToArray();
            return Process(element, context, steps);
        }

        // ReSharper disable LoopCanBeConvertedToQuery
        private IAstElement Process(IAstElement element, ProcessingContext context, IProcessingStep[] steps) {
            foreach (var step in steps) {
                element = step.ProcessBeforeChildren(element, context);
            }

            context.ElementStack.Push(element);
            element.TransformChildren(c => Process(c, context, steps));
            context.ElementStack.Pop();

            foreach (var step in steps) {
                element = step.ProcessAfterChildren(element, context);
            }

            return element;
        }
        // ReSharper restore LoopCanBeConvertedToQuery
    }
}
