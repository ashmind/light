using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Light.Ast;

namespace Light.Parsing {
    public class ParsingResult {
        public ParsingResult(IEnumerable<IAstElement> tree, IEnumerable<ParsingMessage> messages) {
            this.Tree = new ReadOnlyCollection<IAstElement>(
                (tree as IList<IAstElement>) ?? tree.ToList()
            );
            this.Messages = new ReadOnlyCollection<ParsingMessage>(
                (messages as IList<ParsingMessage>) ?? messages.ToList()
            );
        }

        public ReadOnlyCollection<IAstElement> Tree { get; private set; }
        public ReadOnlyCollection<ParsingMessage> Messages { get; private set; }

        public bool HasErrors {
            get { return this.Messages.Any(m => m.IsError); }
        }
    }
}