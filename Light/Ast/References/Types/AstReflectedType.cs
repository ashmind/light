using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast.Errors;
using Light.Ast.References.Methods;

namespace Light.Ast.References.Types {
    public class AstReflectedType : AstElementBase, IAstTypeReference {
        public Type ActualType { get; private set; }

        public AstReflectedType(Type type) {
            Argument.RequireNotNull("type", type);
            this.ActualType = type;
        }

        public IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var astTypes = arguments.Select(a => a.ExpressionType).ToArray();
            var types = astTypes.Cast<AstReflectedType>().Select(t => t.ActualType).ToArray();
            var method = this.ActualType.GetMethod(name, types);
            if (method == null)
                return new AstMissingMethod(name, astTypes);

            return new AstReflectedMethod(method);
        }

        public IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            var constructor = this.ActualType.GetConstructor(arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray());
            if (constructor == null)
                return null;

            return new AstReflectedConstructor(constructor);
        }

        public IAstMemberReference ResolveMember(string name) {
            var members = this.ActualType.GetMember(name);
            if (members.Length == 0)
                return null;

            if (!members.All(m => m is MethodInfo))
                throw new NotImplementedException("AstReflectedType.ResolveMember: " + members.First(m => !(m is MethodInfo)).GetType() + " is not yet supported.");

            if (members.Length == 1)
                return new AstReflectedMethod((MethodInfo)members[0]);

            return new AstMethodGroup(name, members.Select(m => new AstReflectedMethod((MethodInfo)m)).ToArray());
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        public override string ToString() {
            return this.ActualType.ToString();
        }

        #region IAstReference Members

        string IAstTypeReference.Name {
            get { return this.ActualType.Name; }
        }

        #endregion

        #region IAstReference Members

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
