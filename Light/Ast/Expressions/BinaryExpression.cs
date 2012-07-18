﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class BinaryExpression : IAstExpression {
        private IAstExpression left;
        private IAstMethodReference @operator;
        private IAstExpression right;

        public BinaryExpression(IAstExpression left, IAstMethodReference @operator, IAstExpression right) {
            this.Left = left;
            this.Operator = @operator;
            this.Right = right;
        }

        public IAstExpression Left {
            get { return this.left; }
            set {
                Argument.RequireNotNull("value", value);
                this.left = value;
            }
        }

        public IAstMethodReference Operator {
            get { return this.@operator; }
            set {
                Argument.RequireNotNull("value", value);
                this.@operator = value;
            }
        }

        public IAstExpression Right {
            get { return this.right; }
            set {
                Argument.RequireNotNull("value", value);
                this.right = value;
            }
        }

        public References.IAstTypeReference ExpressionType {
            get { return Operator.ReturnType; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Left = (IAstExpression)transform(this.Left);
            yield return this.Operator = (IAstMethodReference)transform(this.Operator);
            yield return this.Right = (IAstExpression)transform(this.Right);
        }

        #endregion
    }
}
