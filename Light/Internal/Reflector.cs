using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Internal {
    public class Reflector {
        public IAstTypeReference Reflect(Type type) {
            Argument.RequireNotNull("type", type);
            return this.RecursionSafeReflect(type, new Dictionary<Type, IAstTypeReference>());
        }

        private IAstTypeReference RecursionSafeReflect(Type type, IDictionary<Type, IAstTypeReference> alreadyReflected) {
            var reflected = alreadyReflected.GetValueOrDefault(type);
            if (reflected != null)
                return reflected;

            if (type == typeof(object))
                return AstAnyType.Instance;

            if (type.IsGenericParameter) {
                var constraints = type.GetGenericParameterConstraints();
                return new AstGenericPlaceholderType(
                    type.Name,
                    p => {
                        alreadyReflected.Add(type, p);
                        return constraints.Select(c => this.RecursionSafeReflect(c, alreadyReflected));
                    },
                    target: type
                );
            }

            if (IsFunctionType(type))
                return ReflectFunctionType(type, alreadyReflected);

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                return ReflectGenericType(type, alreadyReflected);

            return new AstReflectedType(type, this);
        }

        private static bool IsFunctionType(Type type) {
            return type.IsGenericType
                && type.Assembly == typeof(Func<>).Assembly
                && type.Namespace == "System"
                && type.Name.StartsWith("Func");
        }

        private IAstTypeReference ReflectFunctionType(Type type, IDictionary<Type, IAstTypeReference> alreadyReflected) {
            var parameterTypesAndReturnType = type.GetGenericArguments();
            return new AstSpecifiedFunctionType(
                parameterTypesAndReturnType.Take(parameterTypesAndReturnType.Length - 1).Select(t => RecursionSafeReflect(t, alreadyReflected)),
                RecursionSafeReflect(parameterTypesAndReturnType.Last(), alreadyReflected)
            );
        }

        private IAstTypeReference ReflectGenericType(Type type, IDictionary<Type, IAstTypeReference> alreadyReflected) {
            return new AstGenericTypeWithArguments(
                RecursionSafeReflect(type.GetGenericTypeDefinition(), alreadyReflected),
                type.GetGenericArguments().Select(a => RecursionSafeReflect(a, alreadyReflected))
            );
        }
    }
}
