using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AshMind.Extensions;
using Light.Ast.References.Types;
using Light.Internal;

namespace Light.Ast.References.Methods {
    public class AstReflectedMethod : AstElementBase, IAstMethodReference {
        private readonly Reflector reflector;
        public MethodInfo Method { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }
        public MethodLocation Location { get; private set; }

        private readonly Lazy<ReadOnlyCollection<IAstTypeReference>> parameterTypes;
        private readonly Lazy<ReadOnlyCollection<IAstTypeReference>> genericParameterTypes;

        public AstReflectedMethod(MethodInfo method, Reflector reflector) {
            Argument.RequireNotNull("reflector", reflector);
            Argument.RequireNotNull("method", method);

            this.reflector = reflector;
            this.Method = method;
            this.ReturnType = method.ReturnType != typeof(void)
                            ? reflector.Reflect(method.ReturnType)
                            : AstVoidType.Instance;

            this.parameterTypes = new Lazy<ReadOnlyCollection<IAstTypeReference>>(
                () => method.GetParameters()
                            .Select(p => reflector.Reflect(p.ParameterType))
                            .ToArray()
                            .AsReadOnly()
            );

            this.genericParameterTypes = new Lazy<ReadOnlyCollection<IAstTypeReference>>(
                () => method.IsGenericMethodDefinition ? method.GetGenericArguments().Select(reflector.Reflect).ToArray().AsReadOnly() : No.Types
            );

            this.Location = method.IsDefined<ExtensionAttribute>(false) ? MethodLocation.Extension : MethodLocation.Target;
        }

        public string Name {
            get { return this.Method.Name; }
        }

        public ReadOnlyCollection<IAstTypeReference> ParameterTypes {
            get { return this.parameterTypes.Value; }
        }

        public bool IsGeneric {
            get { return this.Method.IsGenericMethodDefinition || this.Method.IsGenericMethod; }
        }

        public ReadOnlyCollection<IAstTypeReference> GenericParameterTypes {
            get { return this.genericParameterTypes.Value; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        public IAstMethodReference WithGenericArguments(IEnumerable<IAstTypeReference> genericArguments) {
            var argumentsAsTypes = genericArguments.Select(a => ((AstReflectedType)a).ActualType).ToArray();
            return new AstReflectedMethod(this.Method.MakeGenericMethod(argumentsAsTypes), this.reflector);
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Method; }
        }

        #endregion

        public override string ToString() {
            return "{Reflected: " + Method + "}";
        }
    }
}