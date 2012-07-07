using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Light.Ast;

namespace Light.Parsing {
    public class ParsingResult {
        public ParsingResult(IAstElement root, IEnumerable<ParsingMessage> messages) {
            this.Root = root;
            this.Messages = new ReadOnlyCollection<ParsingMessage>(
                (messages as IList<ParsingMessage>) ?? messages.ToList()
            );
        }

        public IAstElement Root { get; private set; }
        public ReadOnlyCollection<ParsingMessage> Messages { get; private set; }

        public bool HasErrors {
            get { return this.Messages.Any(m => m.IsError); }
        }
    }
}