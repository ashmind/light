using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class InferParameterTypes : ProcessingStepBase<AstParameterDefinition> {
        public InferParameterTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstParameterDefinition parameter, ProcessingContext context) {
            if (!parameter.Type.IsImplicit())
                return parameter;

            // TODO: support more than one parameter:)
            parameter.Type = new AstGenericPlaceholderType("T");
            return parameter;
        }
    }
}
