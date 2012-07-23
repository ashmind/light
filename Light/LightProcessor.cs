using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AshMind.Extensions;
using Light.Ast;
using Light.Processing;
using Light.Processing.Scoping;

namespace Light {
    public class LightProcessor {
        private static readonly ISet<ProcessingStage> AllStages = Enum.GetValues(typeof(ProcessingStage)).Cast<ProcessingStage>().ToSet();

        private readonly INameSource[] topLevelNameSources;
        private readonly IProcessingStep[] steps;

        public LightProcessor(INameSource[] topLevelNameSources, IProcessingStep[] steps) {
            this.topLevelNameSources = topLevelNameSources;
            this.steps = steps.OrderBy(s => s.Stage).ToArray();
        }

        public IAstElement Process(IAstElement element) {
            return Process(element, AllStages);
        }

        public IAstElement Process(IAstElement element, ISet<ProcessingStage> stages) {
            var context = new ProcessingContext();
            foreach (var source in topLevelNameSources) {
                context.Scope.Add(source);
            }
            var steps = this.steps.Where(s => stages.Contains(s.Stage)).ToArray();
            return Process(element, context, steps);
        }

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
    }
}
