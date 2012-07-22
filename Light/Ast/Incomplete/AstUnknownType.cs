using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownType : AstElementBase, IAstTypeReference {
        public static AstUnknownType WithNoName { get; private set; }
        public string Name { get; private set; }
        
        static AstUnknownType() {
            WithNoName = new AstUnknownType("");
        }

        public AstUnknownType(string name) {
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("Unknown type can not resolve methods.");
        }

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("Unknown type can not resolve constructors.");
        }

        IAstMemberReference IAstTypeReference.ResolveMember(string name) {
            throw new NotImplementedException("Unknown type can not resolve members.");
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override string ToString() {
            return "{UnknownType: " + (this.Name.IsNotNullOrEmpty() ? this.Name : "?") + "}";
        }
    }
}
