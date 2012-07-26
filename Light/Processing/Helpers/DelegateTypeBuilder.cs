using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Processing.Helpers {
    public class DelegateTypeBuilder {
        public IAstTypeReference BuildType(IEnumerable<IAstTypeReference> parameterTypes, IAstTypeReference returnType) {
            var types = new List<IAstTypeReference>(parameterTypes);
            var delegateTypeName = "Action";
            if (!(returnType is AstVoidType)) {
                delegateTypeName = "Func";
                types.Add(returnType);
            }
            delegateTypeName += "`" + types.Count;

            var delegateType = Type.GetType("System." + delegateTypeName, true).MakeGenericType(
                types.Cast<AstReflectedType>().Select(t => t.ActualType).ToArray()
            );

            return new AstReflectedType(delegateType);
        }
    }
}
