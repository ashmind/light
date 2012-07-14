using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public interface ICilCompiler
    {
        void Compile(ILProcessor processor, IAstElement element, CilCompilationContext context);
        bool CanCompile(ILProcessor processor, IAstElement element);
    }
}
