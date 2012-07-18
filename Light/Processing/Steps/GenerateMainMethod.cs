using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References.Types;
using Light.Ast.Statements;

namespace Light.Processing.Steps {
    public class GenerateMainMethod : ProcessingStepBase<AstRoot> {
        public GenerateMainMethod() : base(ProcessingStage.PreCompilation) {
        }

        public override IAstElement ProcessAfterChildren(AstRoot root, ProcessingContext context) {
            var statements = root.Children<IAstStatement>();
            var main = new AstFunctionDefinition("Main", No.Parameters, statements, AstVoidType.Instance) {
                Compilation = { Static = true, EntryPoint = true }
            };
            if (!main.Body.Any())
                return root;

            // TODO: this should be done by GenerateReturns, but the ordering behavior is somewhat incorrect right now
            if (!main.Descendants<AstReturnStatement>().Any())
                main.Body.Add(new AstReturnStatement());

            root.Elements.RemoveWhere(s => s is IAstStatement);

            var program = new AstTypeDefinition(TypeDefintionTypes.Class, "Program", main);
            root.Elements.Add(program);

            return root;
        }
    }
}
