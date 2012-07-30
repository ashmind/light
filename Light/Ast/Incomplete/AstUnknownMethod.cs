using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownMethod : AstElementBase, IAstMethodReference {
        public string Name { get; private set; }

        public AstUnknownMethod(string name) {
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstMethodReference Members

        IAstTypeReference IAstMethodReference.ReturnType {
            get { return AstUnknownType.WithNoName; }
        }

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.ParameterTypes {
            get { throw new NotImplementedException("AstUnknownMethod.ParameterTypes"); }
        }

        MethodLocation IAstMethodReference.Location {
            get { return MethodLocation.Unknown; }
        }

        bool IAstMethodReference.IsGeneric {
            get { throw new NotImplementedException("AstUnknownMethod.IsGeneric"); }
        }

        ReadOnlyCollection<IAstTypeReference> IAstMethodReference.GenericParameterTypes {
            get { throw new NotImplementedException("AstUnknownMethod.GenericParameterTypes"); }
        }

        IAstMethodReference IAstMethodReference.WithGenericArguments(IEnumerable<IAstTypeReference> genericArguments) {
            throw new NotImplementedException("AstUnknownMethod.WithGenericArguments");
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override string ToString() {
            return this.Name;
        }
    }
}