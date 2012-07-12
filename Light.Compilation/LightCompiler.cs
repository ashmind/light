using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Compilation.Cil;
using Light.Compilation.Internal;
using Light.Compilation.Types;
using Mono.Cecil;
using Mono.Cecil.Cil;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace Light.Compilation {
    public class LightCompiler {
        private readonly ITypeResolver[] typeResolvers;
        private readonly ICilCompiler[] cilCompilers;

        public LightCompiler(ITypeResolver[] typeResolvers, ICilCompiler[] cilCompilers) {
            this.typeResolvers = typeResolvers;
            this.cilCompilers = cilCompilers;
        }

        public void Compile(AstRoot root, Stream toStream, CompilationArguments arguments) {
            Argument.RequireNotNull("toStream", toStream);
            Argument.RequireNotNull("root", root);
            Argument.RequireNotNull("arguments", arguments);

            var assembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(arguments.AssemblyName, arguments.AssemblyVersion),
                arguments.AssemblyName,
                arguments.Target == CompilationTarget.Console ? ModuleKind.Console : ModuleKind.Dll
            );

            foreach (var typeAst in root.Descendants<Ast.Definitions.TypeDefinition>()) {
                CompileType(assembly.MainModule, typeAst);
            }

            assembly.Write(toStream);
        }

        private void CompileType(ModuleDefinition module, Ast.Definitions.TypeDefinition typeAst) {
            var type = new TypeDefinition("", typeAst.Name, TypeAttributes.Public | ToTypeAttribute(typeAst.DefinitionType)) {
                BaseType = module.Import(typeof(object))
            };
            foreach (var memberAst in typeAst.Members) {
                CompileMember(type, memberAst, module);
            }

            module.Types.Add(type);
        }

        private void CompileMember(TypeDefinition type, IAstElement memberAst, ModuleDefinition module) {
            var functionAst = memberAst as MethodDefinitionBase;
            if (functionAst != null) {
                CompileFunction(type, functionAst, module);
            }
            else {
                throw new NotImplementedException("LightCompiler.CompileMember: cannot compile " + memberAst + ".");
            }
        }

        private void CompileFunction(TypeDefinition type, MethodDefinitionBase methodAst, ModuleDefinition module) {
            MethodDefinition method;
            if (methodAst is Ast.Definitions.FunctionDefinition) {
                var functionAst = methodAst as FunctionDefinition;
                var returnType = this.typeResolvers.Select(r => r.Resolve(functionAst.ReturnType, module))
                                     .FirstOrDefault(t => t != null);
                if (returnType == null)
                    throw new NotSupportedException("Cannot resolve AST type " + functionAst.ReturnType);

                var attributes = MethodAttributes.Public;
                if (methodAst.Compilation.Static)
                    attributes |= MethodAttributes.Static;

                method = new MethodDefinition(functionAst.Name, attributes, returnType);
                if (methodAst.Compilation.EntryPoint)
                    module.EntryPoint = method;
            }
            else if (methodAst is Ast.Definitions.ConstructorDefinition) {
                method = CecilHelper.CreateConstructor(module);
                method.Attributes |= MethodAttributes.Public;
            }
            else {
                throw new NotImplementedException("LightCompiler.CompileFunction: cannot compile " + methodAst + ".");
            }

            CompileBody(method, methodAst.Body, module);
            type.Methods.Add(method);
        }

        private void CompileBody(MethodDefinition method, IEnumerable<IAstStatement> bodyAst, ModuleDefinition module) {
            var body = method.Body.GetILProcessor();
            foreach (var element in bodyAst) {
                CompileCil(body, element, module);
            }
        }

        private void CompileCil(ILProcessor body, IAstElement element, ModuleDefinition module) {
            var compiler = this.cilCompilers.SingleOrDefault(c => c.CanCompile(body, element));
            if (compiler == null)
                throw new NotImplementedException("LightCompiler: No CilCompiler for " + element);

            compiler.Compile(body, element, e => CompileCil(body, e, module), module);
        }

        private TypeAttributes ToTypeAttribute(string definitionType) {
            return definitionType == TypeDefintionTypes.Class ? TypeAttributes.Class : TypeAttributes.Interface;
        }
    }
}
