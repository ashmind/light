using System;

namespace Light.Parsing {
    public class ParsingMessage {
        public ParsingMessage(string text, ParsingMessageKind kind, SourceLocation location) {
            this.Text = text;
            this.Kind = kind;
            this.Location = location;
        }

        public string Text { get; private set; }
        public ParsingMessageKind Kind { get; private set; }
        public SourceLocation Location { get; private set; }

        public bool IsError {
            get { return this.Kind == ParsingMessageKind.Error; }
        }

        public bool IsWarning {
            get { return this.Kind == ParsingMessageKind.Warning; }
        }

        public override string ToString() {
            return this.Kind + ": " + this.Text + " at " + this.Location;
        }
    }
}