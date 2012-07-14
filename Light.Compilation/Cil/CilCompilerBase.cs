using System;
using Light.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public abstract class CilCompilerBase<TAstElement> : ICilCompiler
        where TAstElement : IAstElement
    {
        public abstract void Compile(ILProcessor processor, TAstElement element, CilCompilationContext context);

        void ICilCompiler.Compile(ILProcessor processor, IAstElement element, CilCompilationContext context) {
            this.Compile(processor, (TAstElement)element, context);
        }

        bool ICilCompiler.CanCompile(ILProcessor processor, IAstElement element) {
            return element is TAstElement;
        }
    }
}