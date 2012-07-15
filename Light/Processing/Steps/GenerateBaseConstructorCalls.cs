using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.Statements;

namespace Light.Processing.Steps {
    public class GenerateBaseConstructorCalls : ProcessingStepBase<AstConstructorDefinition> {
        private static readonly ConstructorInfo ObjectConstructor = typeof(object).GetConstructor(new Type[0]);
        private static readonly IAstConstructorReference ObjectConstructorAst = new AstReflectedConstructor(ObjectConstructor);

        public GenerateBaseConstructorCalls() : base(ProcessingStage.PreCompilation) {
        }

        public override Ast.IAstElement ProcessAfterChildren(AstConstructorDefinition element, ProcessingContext context) {
            element.Body.Insert(0, new AstBaseConstructorCall(ObjectConstructorAst));
            return element;
        }
    }
}
