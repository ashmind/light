using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AshMind.Extensions;

namespace Light.Ast.References.Methods {
    public class AstBuiltInOperator : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }
        public IAstTypeReference OperandType { get; private set; }
        public IAstTypeReference ReturnType { get; private set; }

        private readonly ReadOnlyCollection<IAstTypeReference> parameterTypes;

        public AstBuiltInOperator(string name, IAstTypeReference operandType, IAstTypeReference resultType) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNull("operandType", operandType);
            Argument.RequireNotNull("resultType", resultType);

            this.Name = name;
            this.OperandType = operandType;
            this.ReturnType = resultType;

            this.parameterTypes = new[] {this.OperandType, this.OperandType}.AsReadOnly();
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        #region IAstMethodReference Members

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { return this.parameterTypes; }
        }

        #endregion
    }
}