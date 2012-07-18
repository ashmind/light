using System;
using System.Collections.Generic;
using System.Reflection;
using Light.Ast.References.Types;

namespace Light.Ast.References.Methods {
    public class AstReflectedMethod : IAstMethodReference {
        public MethodInfo Method { get; private set; }
        public IAstTypeReference DeclaringType { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }

        public AstReflectedMethod(MethodInfo method, IAstTypeReference declaringType) {
            Argument.RequireNotNull("method", method);
            this.DeclaringType = declaringType;
            this.Method = method;
            this.ReturnType = method.ReturnType != typeof(void)
                            ? new AstReflectedType(method.ReturnType)
                            : (IAstTypeReference)AstVoidType.Instance;
        }

        public string Name {
            get { return Method.Name; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Method; }
        }

        #endregion
    }
}