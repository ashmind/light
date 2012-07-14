using Light.Ast;

namespace Light.Processing {
    public interface IProcessingStep {
        IAstElement ProcessBeforeChildren(IAstElement element, ProcessingContext context);
        IAstElement ProcessAfterChildren(IAstElement element, ProcessingContext context);

        ProcessingStage Stage { get; }
    }
}