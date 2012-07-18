using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Errors;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveOperatorReferences : ProcessingStepBase<BinaryExpression> {
        private class KnownTypes {
            public static readonly IAstTypeReference Integer = new AstReflectedType(typeof(int));
        }

        private static readonly string[] NotMapped = new string[0];
        private static readonly IDictionary<string, string[]> NameMap = new Dictionary<string, string[]> {
            { "+", new[] { "Plus", "op_Addition" } }
        };

        private AstBuiltInOperator[] BuiltInOperators = {
            new AstBuiltInOperator("+", KnownTypes.Integer, KnownTypes.Integer)
        };

        public ResolveOperatorReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(BinaryExpression binary, ProcessingContext context) {
            if (!(binary.Operator is AstUnknownMethod))
                return binary;

            var builtIn = BuiltInOperators.FirstOrDefault(o => o.Name == binary.Operator.Name
                                                            && o.DeclaringType.Equals(binary.Left.ExpressionType));

            binary.Operator = builtIn ?? ResolveOperatorAsMethod(binary);
            return binary;
        }

        private static IAstMethodReference ResolveOperatorAsMethod(BinaryExpression binary) {
            var operatorName = binary.Operator.Name;
            var names = NameMap.GetValueOrDefault(operatorName, NotMapped).Concat(operatorName);
            var declaringType = binary.Left.ExpressionType;

            var resolved = names.Select(n => declaringType.ResolveMethod(n, new[] {binary.Left, binary.Right}))
                                .FirstOrDefault(r => !(r is AstMissingMethod));

            resolved = resolved ?? new AstMissingMethod(operatorName, declaringType);
            return resolved;
        }
    }
}
