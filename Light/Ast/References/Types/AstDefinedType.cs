using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.References.Methods;

namespace Light.Ast.References.Types {
    public class AstDefinedType : IAstTypeReference {
        public AstTypeDefinition Definition { get; private set; }

        public AstDefinedType(AstTypeDefinition type) {
            this.Definition = type;
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            var method = this.Definition.Children<AstFunctionDefinition>().SingleOrDefault(m => m.Name == name);
            return new AstDefinedMethod(method, this);
        }

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            var constructor = this.Definition.Child<AstConstructorDefinition>();
            return new AstDefinedConstructor(constructor);
        }

        string IAstTypeReference.Name {
            get { return this.Definition.Name; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Definition; }
        }

        #endregion

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}