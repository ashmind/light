using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Literals;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class InferListLiteralTypes : ProcessingStepBase<AstListInitializer> {
        public InferListLiteralTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstListInitializer initializer, ProcessingContext context) {
            var types = initializer.Elements.Select(e => e.ExpressionType).Distinct().ToArray();
            if (types.Length == 0)
                throw new NotImplementedException("InferListLiteralTypes: empty lists are not yet supported.");

            if (types.Length > 1)
                throw new NotImplementedException("InferListLiteralTypes: ambiguous list type.");

            initializer.ExpressionType = new AstReflectedType(((AstReflectedType)types[0]).ActualType.MakeArrayType());
            return initializer;
        }
    }
}
