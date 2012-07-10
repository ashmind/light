using Light.Ast;

namespace Light.Processing {
    public interface IProcessingStep {
        void Process(IAstElement root);
    }
}