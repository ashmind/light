using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class DelegateProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            var functionType = astReference as IAstFunctionTypeReference;
            if (functionType == null)
                return null;

            var types = new List<IAstTypeReference>(functionType.GetParameterTypes());
            var delegateTypeName = "Action";
            if (!(functionType.ReturnType is AstVoidType)) {
                delegateTypeName = "Func";
                types.Add(functionType.ReturnType);
            }
            delegateTypeName += "`" + types.Count;

            var delegateOpenType = module.Import(Type.GetType("System." + delegateTypeName, true));
            var delegateType = new GenericInstanceType(delegateOpenType);
            types.Select(t => recursive.Convert(t, module, recursive).As<TypeReference>())
                 .ForEach(delegateType.GenericArguments.Add);

            return delegateType;
        }
    }
}
