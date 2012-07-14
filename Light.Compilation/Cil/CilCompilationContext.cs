using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Definitions;
using Mono.Cecil;

namespace Light.Compilation.Cil {
    public class CilCompilationContext {
        private readonly Action<IAstElement, CilCompilationContext> compile;

        public MethodDefinitionBase MethodAst { get; private set; }
        public ModuleDefinition Module        { get; private set; }

        public CilCompilationContext(MethodDefinitionBase methodAst, Action<IAstElement, CilCompilationContext> compile, ModuleDefinition module) {
            this.compile = compile;
            this.MethodAst = methodAst;
            this.Module = module;
        }

        public void Compile(IAstElement element) {
            this.compile(element, this);
        }
    }
}
