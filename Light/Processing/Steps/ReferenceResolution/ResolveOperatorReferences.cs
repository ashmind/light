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
using Light.Framework;
using Light.Internal;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveOperatorReferences : ProcessingStepBase<BinaryExpression> {
        #region Maps

        private static readonly Reflector Reflector = new Reflector(); // temporary

        private static readonly IAstTypeReference OperatorsType = new AstReflectedType(typeof(Operators), Reflector);

        private static class KnownTypes {
            public static readonly IAstTypeReference Boolean = new AstReflectedType(typeof(bool), Reflector);
            public static readonly IAstTypeReference String = new AstReflectedType(typeof(string), Reflector);
        }

        private static readonly IDictionary<string, string> NameMap = new Dictionary<string, string> {
            { "+",   "Plus"      },
            { "-",   "Minus"     },
            { "*",   "Multiply"  },
            { "/",   "Divide"    },
            { "mod", "Modulus"   },

            { "==",  "Equals"    },
            { ">",   "IsGreater" },
            { "<",   "IsLess"    },

            { "..",  "Range"     },
        };

        private readonly AstBuiltInOperator[] BuiltInOperators = {
            new AstBuiltInOperator("or",  KnownTypes.Boolean, KnownTypes.Boolean)
        };

        private static readonly IAstMethodReference StringConcat = new AstReflectedMethod(
            typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }), Reflector);

        #endregion

        private readonly MethodCallResolver methodCallResolver;

        public ResolveOperatorReferences(MethodCallResolver methodCallResolver) : base(ProcessingStage.ReferenceResolution) {
            this.methodCallResolver = methodCallResolver;
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
            var operatorName = NameMap[binary.Operator.Name];
            var resolved = (IAstMethodReference)OperatorsType.ResolveMember(operatorName);

            if (resolved == null)
                throw new NotImplementedException("ResolveOperatorReferences: Failed to resolve " + binary.Operator.Name);

            return methodCallResolver.Resolve(resolved, OperatorsType, new[] { binary.Left, binary.Right });
        }
    }
}
