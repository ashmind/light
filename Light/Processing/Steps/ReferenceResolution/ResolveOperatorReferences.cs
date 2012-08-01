using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Light.Internal;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveOperatorReferences : ProcessingStepBase<BinaryExpression> {
        #region Maps

        private static readonly Reflector Reflector = new Reflector(); // temporary

        private static class KnownTypes {
            public static readonly IAstTypeReference Boolean = new AstReflectedType(typeof(bool), Reflector);
            public static readonly IAstTypeReference String = new AstReflectedType(typeof(string), Reflector);
        }

        private static readonly string[] NotMapped = new string[0];
        private static readonly IDictionary<string, string[]> NameMap = new Dictionary<string, string[]> {
            { "+",   new[] { "Plus",       "op_Addition" } },
            { "-",   new[] { "Minus",      "op_Subtraction" } },
            { "*",   new[] { "MultiplyBy", "op_Multiply" } },
            { "/",   new[] { "DivideBy",   "op_Division" } },
            { "mod", new[] { "Modulus" } },

            { "==",  new[] { "Equals" } },

            { ">",   new[] { "IsGreaterThan" } },
            { "<",   new[] { "IsLessThan" } },

            { "..",  new[] { "RangeTo" } },
        };

        private readonly AstBuiltInOperator[] BuiltInOperators = {
            new AstBuiltInOperator("or",  KnownTypes.Boolean, KnownTypes.Boolean)
        };

        private static readonly IAstMethodReference StringConcat = new AstReflectedMethod(
            typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }), Reflector); // new Reflector() is temporary here

        #endregion

        private readonly OverloadResolver overloadResolver;

        public ResolveOperatorReferences(OverloadResolver overloadResolver) : base(ProcessingStage.ReferenceResolution) {
            this.overloadResolver = overloadResolver;
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
                                                     && o.OperandType.Equals(binary.Left.ExpressionType));
        }

        private IAstMethodReference ResolveOperatorAsMethod(BinaryExpression binary) {
            var operatorName = binary.Operator.Name;
            var names = NameMap.GetValueOrDefault(operatorName, NotMapped).Concat(operatorName);
            var declaringType = binary.Left.ExpressionType;

            var resolved = names.Select(declaringType.ResolveMember)
                                .Cast<IAstMethodReference>()
                                .FirstOrDefault(r => r != null);

            if (resolved == null)
                throw new NotImplementedException("ResolveOperatorReferences: Failed to resolve " + binary.Operator.Name);

            var group = resolved as AstMethodGroup;
            if (group != null)
                resolved = overloadResolver.ResolveMethodGroup(group, binary.Left, new[] {binary.Right});

            //resolved = resolved ?? new AstMissingMethod(operatorName, new[] { binary.Left.ExpressionType, binary.Right.ExpressionType });
            return resolved;
        }
    }
}
