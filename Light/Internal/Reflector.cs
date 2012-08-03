using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Internal {
    public class Reflector {
        public IAstTypeReference Reflect(Type type) {
            Argument.RequireNotNull("type", type);

            if (type == typeof(object))
                return AstAnyType.Instance;

            if (type.IsGenericParameter)
                return new AstGenericPlaceholderType(type.Name, type);

            if (IsFunctionType(type))
                return ReflectFunctionType(type);

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                return ReflectGenericType(type);

            return new AstReflectedType(type, this);
        }

        private static bool IsFunctionType(Type type) {
            return type.IsGenericType
                && type.Assembly == typeof(Func<>).Assembly
                && type.Namespace == "System"
                && type.Name.StartsWith("Func");
        }

        private IAstTypeReference ReflectFunctionType(Type type) {
            var parameterTypesAndReturnType = type.GetGenericArguments();
            return new AstSpecifiedFunctionType(
                parameterTypesAndReturnType.Take(parameterTypesAndReturnType.Length - 1).Select(Reflect),
                Reflect(parameterTypesAndReturnType.Last())
            );
        }

        private IAstTypeReference ReflectGenericType(Type type) {
            return new AstGenericTypeWithArguments(
                Reflect(type.GetGenericTypeDefinition()),
                type.GetGenericArguments().Select(Reflect)
            );
        }
    }
}
