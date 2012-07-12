using Light.Ast;

namespace Light.Processing {
    public abstract class ProcessingStepBase<TAstElement> : IProcessingStep 
        where TAstElement : class, IAstElement
    {
        public virtual void ProcessBeforeChildren(TAstElement element, ProcessingContext context) {
        }

        public virtual void ProcessAfterChildren(TAstElement element, ProcessingContext context) {
        }

        void IProcessingStep.ProcessBeforeChildren(IAstElement element, ProcessingContext context) {
            var matched = element as TAstElement;
            if (matched == null)
                return;

            this.ProcessBeforeChildren(matched, context);
        }

        void IProcessingStep.ProcessAfterChildren(IAstElement element, ProcessingContext context) {
            var matched = element as TAstElement;
            if (matched == null)
                return;

            this.ProcessAfterChildren(matched, context);
        }
    }
}