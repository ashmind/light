using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing.Steps {
    public class GenerateReturns : ProcessingStepBase<MethodDefinitionBase> {
        public override void ProcessAfterChildren(MethodDefinitionBase method, ProcessingContext context) {
            if (method.Body.OfType<ReturnStatement>().Any())
                return;

            var function = method as FunctionDefinition;
            if (function != null && !function.ReturnType.IsVoid())
                throw new NotSupportedException("GenerateReturns: function return type is " + function.ReturnType);

            method.Body.Add(new ReturnStatement());
        }
    }
}
