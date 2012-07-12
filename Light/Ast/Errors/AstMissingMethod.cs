using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Errors {
    public class AstMissingMethod : IAstMethodReference {
        public string Name { get; private set; }
        public IAstTypeReference DeclaringType { get; private set; }

        public AstMissingMethod(string name, IAstTypeReference declaringType) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNull("declaringType", declaringType);

            this.Name = name;
            this.DeclaringType = declaringType;
        }

        public IAstTypeReference ReturnType {
            get { return null; }
        }

        public IEnumerable<IAstElement> Children() {
            throw new NotImplementedException();
        }
    }
}
