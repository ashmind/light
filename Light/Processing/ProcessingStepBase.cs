using Light.Ast;

namespace Light.Processing {
    public abstract class ProcessingStepBase<TAstElement> : IProcessingStep 
        where TAstElement : class, IAstElement
    {
        public virtual IAstElement ProcessBeforeChildren(TAstElement element, ProcessingContext context) {
            return element;
        }

        public virtual IAstElement ProcessAfterChildren(TAstElement element, ProcessingContext context) {
            return element;
        }

        IAstElement IProcessingStep.ProcessBeforeChildren(IAstElement element, ProcessingContext context) {
            var matched = element as TAstElement;
            if (matched == null)
                return element;

            return this.ProcessBeforeChildren(matched, context);
        }

        IAstElement IProcessingStep.ProcessAfterChildren(IAstElement element, ProcessingContext context) {
            var matched = element as TAstElement;
            if (matched == null)
                return element;

            return this.ProcessAfterChildren(matched, context);
        }
    }
}