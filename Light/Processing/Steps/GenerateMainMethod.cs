using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References.Types;
using Light.Ast.Types;

namespace Light.Processing.Steps {
    public class GenerateMainMethod : ProcessingStepBase<AstRoot> {
        public override void ProcessAfterChildren(AstRoot root, ProcessingContext context) {
            var statements = root.Children<IAstStatement>();
            var main = new FunctionDefinition("Main", Enumerable.Empty<IAstElement>(), statements, AstVoidType.Instance);
            root.Elements.RemoveWhere(s => s is IAstStatement);

            var program = new TypeDefinition(TypeDefintionTypes.Class, "Program", main);
            root.Elements.Add(program);
        }
    }
}
