using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class AstTypeDefinition : IAstDefinition {
        private string definitionType;
        public string Name { get; private set; }

        public string DefinitionType {
            get { return definitionType; }
            set {
                Argument.RequireNotNullAndNotEmpty("definitionType", value);
                this.definitionType = value;
            }
        }

        public IList<IAstDefinition> Members { get; private set; }

        public AstTypeDefinition(string definitionType, string name, params IAstDefinition[] members)
            : this(definitionType, name, (IEnumerable<IAstDefinition>)members)
        {
        }

        public AstTypeDefinition(string definitionType, string name, IEnumerable<IAstDefinition> members) {
            var membersList = members.ToList();
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullAndNotContainsNull("members", membersList);

            this.DefinitionType = definitionType;
            this.Name = name;
            this.Members = membersList;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.Members.Transform(transform);
        }

        #endregion
    }
}
