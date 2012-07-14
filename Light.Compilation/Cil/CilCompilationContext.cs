using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Compilation.References;
using Mono.Cecil;

namespace Light.Compilation.Cil {
    public class CilCompilationContext : IReferenceContext {
        private readonly Action<IAstElement, CilCompilationContext> compile;
        private readonly IReferenceContext referenceContext;

        public MethodDefinitionBase MethodAst { get; private set; }

        public CilCompilationContext(MethodDefinitionBase methodAst, Action<IAstElement, CilCompilationContext> compile, IReferenceContext referenceContext) {
            this.compile = compile;
            this.referenceContext = referenceContext;
            this.MethodAst = methodAst;
        }

        public void Compile(IAstElement element) {
            this.compile(element, this);
        }

        public TypeReference ConvertTypeReference(IAstTypeReference type) {
            return this.referenceContext.ConvertTypeReference(type);
        }

        public MethodReference ConvertMethodReference(IAstMethodReference method) {
            return this.referenceContext.ConvertMethodReference(method);
        }

        public FieldReference ConvertFieldReference(AstPropertyReference property) {
            return this.referenceContext.ConvertFieldReference(property);
        }
    }
}
