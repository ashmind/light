﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing.Steps {
    public class GenerateReturns : ProcessingStepBase<AstMethodDefinitionBase> {
        public GenerateReturns() : base(ProcessingStage.PreCompilation) {
        }

        public override IAstElement ProcessAfterChildren(AstMethodDefinitionBase method, ProcessingContext context) {
            if (method.Body.OfType<AstReturnStatement>().Any())
                return method;

            var function = method as AstFunctionDefinition;
            if (function != null && !function.ReturnType.IsVoid())
                throw new NotImplementedException("GenerateReturns: function return type is " + function.ReturnType);

            method.Body.Add(new AstReturnStatement());
            return method;
        }
    }
}
