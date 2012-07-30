using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.References.Methods;
using Light.Ast.References.Properties;

namespace Light.Ast.References.Types {
    public class AstDefinedType : AstElementBase, IAstTypeReference {
        public AstTypeDefinition Definition { get; private set; }

        public AstDefinedType(AstTypeDefinition type) {
            this.Definition = type;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        public IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var method = this.Definition.Children<AstFunctionDefinition>().SingleOrDefault(m => m.Name == name);
            return new AstDefinedMethod(method);
        }

        public IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            var constructor = this.Definition.Child<AstConstructorDefinition>();
            return new AstDefinedConstructor(constructor);
        }

        public IAstMemberReference ResolveMember(string name) {
            var member = this.Definition.Children<IAstMemberDefinition>().SingleOrDefault(m => m.Name == name);
            if (member == null)
                return null;

            var function = member as AstFunctionDefinition;
            if (function != null)
                return new AstDefinedMethod(function);

            var property = member as AstPropertyDefinition;
            if (property != null)
                return new AstDefinedProperty(property);

            throw new NotImplementedException("AstDefinedType: " + member.GetType() + " is not yet supported.");
        }

        #region IAstTypeReference Members

        string IAstTypeReference.Name {
            get { return this.Definition.Name; }
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { return AstAnyType.Instance; }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            return No.Types;
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            return No.Types;
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Definition; }
        }

        #endregion
    }
}