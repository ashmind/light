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
        #region Maps

        private static class KnownTypes {
            public static readonly IAstTypeReference Integer = new AstReflectedType(typeof(int));
            public static readonly IAstTypeReference Boolean = new AstReflectedType(typeof(bool));
            public static readonly IAstTypeReference String = new AstReflectedType(typeof(string));
        }

        private static readonly string[] NotMapped = new string[0];
        private static readonly IDictionary<string, string[]> NameMap = new Dictionary<string, string[]> {
            { "+",  new[] { "Plus",       "op_Addition" } },
            { "-",  new[] { "Minus",      "op_Subtraction" } },
            { "*",  new[] { "MultiplyBy", "op_Multiply" } },
            { "/",  new[] { "DivideBy",   "op_Division" } },
            { "==", new[] { "Equals" } }
        };

        private readonly AstBuiltInOperator[] BuiltInOperators = {
            new AstBuiltInOperator("+",  KnownTypes.Integer, KnownTypes.Integer),
            new AstBuiltInOperator("-",  KnownTypes.Integer, KnownTypes.Integer),
            new AstBuiltInOperator("*",  KnownTypes.Integer, KnownTypes.Integer),
            new AstBuiltInOperator("/",  KnownTypes.Integer, KnownTypes.Integer),
            new AstBuiltInOperator("==", KnownTypes.Integer, KnownTypes.Boolean),
            new AstBuiltInOperator(">",  KnownTypes.Integer, KnownTypes.Boolean),
            new AstBuiltInOperator("<",  KnownTypes.Integer, KnownTypes.Boolean)
        };

        private static readonly IAstMethodReference StringConcat = new AstReflectedMethod(
            typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }), KnownTypes.String
        );

        #endregion

        public ResolveOperatorReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(BinaryExpression binary, ProcessingContext context) {
            if (!(binary.Operator is AstUnknownMethod))
                return binary;

            binary.Operator = ResolveSpecialCases(binary)
                           ?? ResolveBuiltInOperator(binary)
                           ?? ResolveOperatorAsMethod(binary);
            return binary;
        }

        private IAstMethodReference ResolveSpecialCases(BinaryExpression binary) {
            if (binary.Left.ExpressionType.Equals(KnownTypes.String) && binary.Right.ExpressionType.Equals(KnownTypes.String) && binary.Operator.Name == "+")
                return StringConcat;

            return null;
        }

        private AstBuiltInOperator ResolveBuiltInOperator(BinaryExpression binary) {
            return BuiltInOperators.FirstOrDefault(o => o.Name == binary.Operator.Name
                                                     && o.DeclaringType.Equals(binary.Left.ExpressionType));
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
