using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing.Steps {
    public class GenerateReturns : ProcessingStepBase<MethodDefinitionBase> {
        public override IAstElement ProcessAfterChildren(MethodDefinitionBase method, ProcessingContext context) {
            if (method.Body.OfType<ReturnStatement>().Any())
                return method;

            var function = method as FunctionDefinition;
            if (function != null && !function.ReturnType.IsVoid())
                throw new NotSupportedException("GenerateReturns: function return type is " + function.ReturnType);

            method.Body.Add(new ReturnStatement());
            return method;
        }
    }
}
