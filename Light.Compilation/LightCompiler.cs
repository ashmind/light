using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Compilation.Internal;
using Mono.Cecil;
using Mono.Cecil.Cil;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace Light.Compilation {
    public class LightCompiler {
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
            var constructorAst = memberAst as Ast.Definitions.ConstructorDefinition;
            if (constructorAst != null) {
                var constructor = CecilHelper.CreateConstructor(module);
                CompileBody(constructor, constructorAst.Body, module);

                constructor.Attributes |= MethodAttributes.Public;
                type.Methods.Add(constructor);
            }
        }

        private void CompileBody(MethodDefinition method, IList<IAstElement> bodyAst, ModuleDefinition module) {
            var body = method.Body.GetILProcessor();
            body.Emit(OpCodes.Ldarg_0);
            body.Emit(OpCodes.Call, module.Import(typeof(object).GetConstructor(new Type[0])));
            body.Emit(OpCodes.Ret);
        }

        private TypeAttributes ToTypeAttribute(string definitionType) {
            return definitionType == TypeDefintionTypes.Class ? TypeAttributes.Class : TypeAttributes.Interface;
        }
    }
}
