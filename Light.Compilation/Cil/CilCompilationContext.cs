using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Ast.Statements;
using Light.Compilation.References;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class CilCompilationContext : IReferenceContext {
        private readonly IDictionary<AstVariableDefinition, VariableDefinition> locals = new Dictionary<AstVariableDefinition, VariableDefinition>();
        private readonly Action<IAstElement, CilCompilationContext> compile;
        private readonly IReferenceContext referenceContext;

        public MethodDefinition Method { get; private set; }
        public AstMethodDefinitionBase MethodAst { get; private set; }

        public CilCompilationContext(MethodDefinition method, AstMethodDefinitionBase methodAst, Action<IAstElement, CilCompilationContext> compile, IReferenceContext referenceContext) {
            this.compile = compile;
            this.referenceContext = referenceContext;
            this.Method = method;
            this.MethodAst = methodAst;
        }

        public void Compile(IAstElement element) {
            this.compile(element, this);
        }

        public void MapDefinition(AstVariableDefinition variableAst, VariableDefinition variable) {
            locals.Add(variableAst, variable);
        }

        public TypeReference ConvertReference(IAstTypeReference type) {
            return this.referenceContext.ConvertReference(type);
        }

        public MethodReference ConvertReference(IAstConstructorReference constructor) {
            return this.referenceContext.ConvertReference(constructor);
        }

        public MethodReference ConvertReference(IAstMethodReference method) {
            return this.referenceContext.ConvertReference(method);
        }

        public FieldReference ConvertReference(AstPropertyReference property) {
            return this.referenceContext.ConvertReference(property);
        }

        public VariableDefinition ConvertReference(AstVariableReference variable) {
            return locals.GetValueOrDefault(variable.Variable);
        }
    }
}
