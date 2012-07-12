using Light.Ast;

namespace Light.Processing {
    public interface IProcessingStep {
        void ProcessBeforeChildren(IAstElement element, ProcessingContext context);
        void ProcessAfterChildren(IAstElement element, ProcessingContext context);
    }
}