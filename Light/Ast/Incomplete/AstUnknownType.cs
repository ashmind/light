using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownType : IAstTypeReference {
        public static AstUnknownType WithNoName { get; private set; }
        public string Name { get; private set; }
        
        static AstUnknownType() {
            WithNoName = new AstUnknownType("");
        }

        public AstUnknownType(string name) {
            this.Name = name;
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("Unknown type can not resolve methods.");
        }

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("Unknown type can not resolve constructors.");
        }

        #endregion

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion
    }
}
