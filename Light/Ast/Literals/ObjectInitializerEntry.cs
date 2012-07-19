﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Literals {
    public class ObjectInitializerEntry : IAstElement {
        public string Name { get; private set; }
        public IAstElement Value { get; private set; }

        public ObjectInitializerEntry(string name, IAstElement value) {
            Argument.RequireNotNull("value", value);

            this.Name = name;
            this.Value = value;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Value = transform(this.Value);
        }

        #endregion

        public override string ToString() {
            return this.Name + ": " + this.Value;
        }
    }
}
