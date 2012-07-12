using System;
using Light.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public abstract class CilCompilerBase<TAstElement> : ICilCompiler
        where TAstElement : IAstElement
    {
        public abstract void Compile(ILProcessor processor, TAstElement element, Action<IAstElement> recursiveCompile, ModuleDefinition module);

        void ICilCompiler.Compile(ILProcessor processor, IAstElement element, Action<IAstElement> recursiveCompile, ModuleDefinition module) {
            this.Compile(processor, (TAstElement)element, recursiveCompile, module);
        }

        bool ICilCompiler.CanCompile(ILProcessor processor, IAstElement element) {
            return element is TAstElement;
        }
    }
}