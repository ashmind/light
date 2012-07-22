﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class CallCompiler : CilCompilerBase<CallExpression> {
        public override void Compile(ILProcessor processor, CallExpression call, CilCompilationContext context) {
            if (call.Target != null && !(call.Target is IAstTypeReference))
                context.Compile(call.Target);

            foreach (var argument in call.Arguments) {
                context.Compile(argument);
            }

            processor.Emit(OpCodes.Call, context.ConvertReference(call.Method));
        }
    }
}
