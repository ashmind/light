using System;
using System.Collections.Generic;

namespace Light.Ast.References.Methods {
    public class AstBuiltInOperator : IAstMethodReference {
        public string Name { get; private set; }
        public IAstTypeReference DeclaringType { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }

        public AstBuiltInOperator(string name, IAstTypeReference leftType, IAstTypeReference resultType) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
            this.DeclaringType = leftType; // irrelevant
            this.ReturnType = resultType;
        }

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