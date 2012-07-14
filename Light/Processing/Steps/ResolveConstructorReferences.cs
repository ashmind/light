using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;

namespace Light.Processing.Steps {
    public class ResolveConstructorReferences : ProcessingStepBase<NewExpression> {
        public ResolveConstructorReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(NewExpression @new, ProcessingContext context) {
            if (@new.Constructor != null)
                return @new;

            @new.Constructor = @new.Type.ResolveConstructor(@new.Arguments);
            return @new;
        }
    }
}
