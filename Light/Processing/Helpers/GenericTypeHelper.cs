using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Processing.Helpers {
    public class GenericTypeHelper {
        public IAstTypeReference RemapArgumentTypes(IAstTypeReference type, Func<IAstTypeReference, IAstTypeReference> remap) {
            if (type is AstGenericPlaceholderType)
                return remap(type);

            var function = type as AstSpecifiedFunctionType;
            if (function != null)
                return RemapArgumentTypesForFunction(function, remap);

            var generic = type as AstGenericTypeWithArguments;
            if (generic != null)
                return RemapArgumentTypesForGeneric(generic, remap);

            var parameters = type.GetTypeParameters().ToArray();
            if (parameters.Any())
                return RemapArgumentTypesFromParameters(type, parameters, remap);

            return type;
        }

        private IAstTypeReference RemapArgumentTypesForFunction(AstSpecifiedFunctionType function, Func<IAstTypeReference, IAstTypeReference> remap) {
            var parameterTypes = function.ParameterTypes;
            var remapped = RemapAll(parameterTypes, remap);

            var newReturnType = remap(function.ReturnType);
            var changed = remapped.Item2 || newReturnType != function.ReturnType;
            return !changed ? function : new AstSpecifiedFunctionType(remapped.Item1, newReturnType);
        }

        private IAstTypeReference RemapArgumentTypesForGeneric(AstGenericTypeWithArguments generic, Func<IAstTypeReference, IAstTypeReference> remap) {
            var arguments = generic.TypeArguments;
            var remapped = RemapAll(arguments, remap);
            return !remapped.Item2 ? generic : new AstGenericTypeWithArguments(generic.PrimaryType, remapped.Item1);
        }

        private IAstTypeReference RemapArgumentTypesFromParameters(IAstTypeReference type, IAstTypeReference[] parameters, Func<IAstTypeReference, IAstTypeReference> remap) {
            var remapped = RemapAll(parameters, remap);
            return !remapped.Item2 ? type : new AstGenericTypeWithArguments(type, remapped.Item1);
        }

        private static Tuple<IList<IAstTypeReference>, bool> RemapAll(IList<IAstTypeReference> types, Func<IAstTypeReference, IAstTypeReference> remap) {
            var changed = false;
            var newTypes = new IAstTypeReference[types.Count];
            for (var i = 0; i < types.Count; i++) {
                newTypes[i] = remap(types[i]);
                changed = changed || newTypes[i] != types[i];
            }
            return Tuple.Create((IList<IAstTypeReference>)newTypes, changed);
        }
    }
}
