using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast.Errors;
using Light.Ast.References.Methods;
using Light.Ast.References.Properties;
using Light.Internal;

namespace Light.Ast.References.Types {
    public class AstReflectedType : AstElementBase, IAstTypeReference {
        private readonly Reflector reflector;
        private readonly Lazy<IAstTypeReference> baseType;

        public Type ActualType { get; private set; }

        public AstReflectedType(Type type, Reflector reflector) {
            Argument.RequireNotNull("type", type);
            Argument.RequireNotNull("reflector", reflector);

            this.ActualType = type;
            this.reflector = reflector;
            this.baseType = this.ActualType.BaseType != null
                          ? new Lazy<IAstTypeReference>(() => this.reflector.Reflect(this.ActualType.BaseType))
                          : new Lazy<IAstTypeReference>(() => AstAnyType.Instance);
        }

        public IAstTypeReference BaseType {
            get { return this.baseType.Value; }
        }

        public IEnumerable<IAstTypeReference> GetInterfaces() {
            return this.ActualType.GetInterfaces().Select(this.reflector.Reflect);
        }

        public IEnumerable<IAstTypeReference> GetTypeParameters() {
            if (!this.ActualType.IsGenericTypeDefinition)
                return No.Types;

            return this.ActualType.GetGenericArguments().Select(this.reflector.Reflect);
        }

        public IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var astTypes = arguments.Select(a => a.ExpressionType).ToArray();
            var types = astTypes.Cast<AstReflectedType>().Select(t => t.ActualType).ToArray();
            var method = this.ActualType.GetMethod(name, types);
            if (method == null)
                return new AstMissingMethod(name, astTypes);

            return new AstReflectedMethod(method, reflector);
        }

        public IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            var constructor = this.ActualType.GetConstructor(arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray());
            if (constructor == null)
                return null;

            return new AstReflectedConstructor(constructor, reflector);
        }

        public IAstMemberReference ResolveMember(string name) {
            var members = this.ActualType.GetMember(name);
            if (members.Length == 0)
                return null;

            if (members.Length == 1) {
                var property = members[0] as PropertyInfo;
                if (property != null)
                    return new AstReflectedProperty(property, reflector);
            }

            if (!members.All(m => m is MethodInfo))
                throw new NotImplementedException("AstReflectedType.ResolveMember: " + members.First(m => !(m is MethodInfo)).GetType() + " is not yet supported.");

            if (members.Length == 1)
                return new AstReflectedMethod((MethodInfo)members[0], reflector);

            return new AstMethodGroup(name, members.Select(m => new AstReflectedMethod((MethodInfo)m, reflector)).ToArray());
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        public override string ToString() {
            return "{Reflected: " +  this.ActualType + "}";
        }

        #region IAstReference Members

        string IAstTypeReference.Name {
            get { return this.ActualType.Name; }
        }

        object IAstReference.Target {
            get { return this.ActualType; }
        }

        #endregion

        public override bool Equals(object obj) {
            return this.Equals(obj as AstReflectedType);
        }

        public bool Equals(AstReflectedType type) {
            return type != null
                && Equals(this.ActualType, type.ActualType);
        }

        public override int GetHashCode() {
            return this.ActualType.GetHashCode();
        }
    }
}
