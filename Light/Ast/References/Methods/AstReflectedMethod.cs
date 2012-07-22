using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AshMind.Extensions;
using Light.Ast.References.Types;

namespace Light.Ast.References.Methods {
    public class AstReflectedMethod : AstElementBase, IAstMethodReference {
        public MethodInfo Method { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }
        public ReadOnlyCollection<IAstTypeReference> ParameterTypes { get; private set; }

        public AstReflectedMethod(MethodInfo method) {
            Argument.RequireNotNull("method", method);
            this.Method = method;
            this.ReturnType = method.ReturnType != typeof(void)
                            ? new AstReflectedType(method.ReturnType)
                            : (IAstTypeReference)AstVoidType.Instance;

            this.ParameterTypes = method.GetParameters()
                                        .Select(p => (IAstTypeReference)new AstReflectedType(p.ParameterType))
                                        .ToArray()
                                        .AsReadOnly();
        }

        public string Name {
            get { return this.Method.Name; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Method; }
        }

        #endregion
    }
}