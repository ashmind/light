using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Definitions {
    public class AnonymousTypeMember {
        public string Name     { get; private set; }
        public string TypeName { get; private set; }

        public AnonymousTypeMember(string name, string typeName) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
            this.TypeName = typeName;
        }
    }
}
