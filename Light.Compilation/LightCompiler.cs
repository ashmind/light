using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Mono.Cecil;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace Light.Compilation {
    public class LightCompiler {
        public void Compile(Stream resultStream, AstRoot root, CompilationArguments arguments) {
            var assembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(arguments.AssemblyName, arguments.AssemblyVersion),
                arguments.AssemblyName,
                arguments.Target == CompilationTarget.Console ? ModuleKind.Console : ModuleKind.Dll
            );

            foreach (var typeDefinition in root.Descendants<Ast.Definitions.TypeDefinition>()) {
                CompileType(assembly.MainModule, typeDefinition);
            }

            assembly.Write(resultStream);
        }

        private void CompileType(ModuleDefinition module, Ast.Definitions.TypeDefinition typeDefinition) {
            var type = new TypeDefinition("", typeDefinition.Name, TypeAttributes.Public | ToTypeAttribute(typeDefinition.DefinitionType));
            module.Types.Add(type);
        }

        private TypeAttributes ToTypeAttribute(string definitionType) {
            return definitionType == TypeDefintionTypes.Class ? TypeAttributes.Class : TypeAttributes.Interface;
        }
    }
}
