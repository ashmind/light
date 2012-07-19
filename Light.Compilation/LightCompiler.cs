using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Compilation.Cil;
using Light.Compilation.Definitions;
using Light.Compilation.Internal;
using Light.Compilation.References;
using Mono.Cecil;
using Mono.Cecil.Cil;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace Light.Compilation {
    public class LightCompiler {
        private readonly IDefinitionBuilder[] builders;
        private readonly ICilCompiler[] cilCompilers;
        private readonly IReferenceProvider[] referenceProviders;

        public LightCompiler(IDefinitionBuilder[] builders, ICilCompiler[] cilCompilers, IReferenceProvider[] referenceProviders) {
            this.builders = builders;
            this.cilCompilers = cilCompilers;
            this.referenceProviders = referenceProviders;
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

            var module = assembly.MainModule;
            var context = new DefinitionBuildingContext(module, this.referenceProviders);
            
            foreach (var typeAst in root.Descendants<AstTypeDefinition>()) {
                DefineType(module, typeAst, context);
            }

            foreach (var type in module.Types) {
                foreach (var method in type.Methods) {
                    CompileMethod(method, context.GetAst(method), context);
                }
            }

            assembly.Write(toStream);
        }

        private void DefineType(ModuleDefinition module, Ast.Definitions.AstTypeDefinition typeAst, DefinitionBuildingContext context) {
            var type = new TypeDefinition("", typeAst.Name, TypeAttributes.Public | ToTypeAttribute(typeAst.DefinitionType)) {
                BaseType = module.Import(typeof(object))
            };
            module.Types.Add(type);

            foreach (var memberAst in typeAst.Members) {
                DefineMember(type, memberAst, context);
            }
            context.MapDefinition(typeAst, type);
        }

        private void DefineMember(TypeDefinition type, IAstDefinition memberAst, DefinitionBuildingContext context) {
            var functionAst = memberAst as AstMethodDefinitionBase;
            if (functionAst != null) {
                DefineFunction(type, functionAst, context);
                return;
            }

            var builder = this.builders.SingleOrDefault(c => c.CanBuild(memberAst, type));
            if (builder == null)
                throw new NotImplementedException("LightCompiler: No DefinitionBuilder for " + memberAst + " under " + type + ".");

            builder.Build(memberAst, type, context);
        }

        private void DefineFunction(TypeDefinition type, AstMethodDefinitionBase methodAst, DefinitionBuildingContext context) {
            MethodDefinition method;
            if (methodAst is Ast.Definitions.AstFunctionDefinition) {
                var functionAst = methodAst as AstFunctionDefinition;
                var returnType = context.ConvertReference(functionAst.ReturnType);

                var attributes = MethodAttributes.Public;
                if (methodAst.Compilation.Static)
                    attributes |= MethodAttributes.Static;

                method = new MethodDefinition(functionAst.Name, attributes, returnType);
                if (methodAst.Compilation.EntryPoint)
                    type.Module.EntryPoint = method;
            }
            else if (methodAst is Ast.Definitions.AstConstructorDefinition) {
                method = CecilHelper.CreateConstructor(type);
                method.Attributes |= MethodAttributes.Public;
            }
            else {
                throw new NotImplementedException("LightCompiler.CompileFunction: cannot compile " + methodAst + ".");
            }

            type.Methods.Add(method);
            context.MapDefinition(methodAst, method);
            DefineParameters(method, methodAst, context);
        }

        private void DefineParameters(MethodDefinition method, AstMethodDefinitionBase methodAst, DefinitionBuildingContext context) {
            foreach (var parameter in methodAst.Parameters) {
                method.Parameters.Add(new ParameterDefinition(parameter.Name, ParameterAttributes.None, context.ConvertReference(parameter.Type)));
            }
        }

        private void CompileMethod(MethodDefinition method, AstMethodDefinitionBase methodAst, DefinitionBuildingContext parentContext) {
            var body = method.Body.GetILProcessor();
            var context = new CilCompilationContext(method, methodAst, (e, c) => CompileCil(body, e, c), parentContext);

            foreach (var element in methodAst.Body) {
                CompileCil(body, element, context);
            }
        }

        private void CompileCil(ILProcessor body, IAstElement element, CilCompilationContext context) {
            var compiler = this.cilCompilers.SingleOrDefault(c => c.CanCompile(body, element));
            if (compiler == null)
                throw new NotImplementedException("LightCompiler: No CilCompiler for " + element);

            compiler.Compile(body, element, context);
        }

        private TypeAttributes ToTypeAttribute(string definitionType) {
            return definitionType == TypeDefintionTypes.Class ? TypeAttributes.Class : TypeAttributes.Interface;
        }
    }
}
