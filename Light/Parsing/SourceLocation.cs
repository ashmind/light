using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Parsing {
    public class SourceLocation {
        private readonly int position;
        private readonly string source;

        public SourceLocation(int line, int column, int position, string source) {
            this.Line = line;
            this.Column = column;

            this.position = position;
            this.source = source;
        }

        public int Line      { get; private set; }
        public int Column    { get; private set; }

        public override string ToString() {
            var precedingSource = this.source.Substring(0, this.position).Trim();
            if (precedingSource.Length == 0)
                return string.Format("line {0}, column {1}", this.Line + 1, this.Column);

            precedingSource = precedingSource.Length > 25 ? precedingSource.Substring(precedingSource.Length - 25, 25) : precedingSource;
            return string.Format("line {0}, column {1}, after {2}", this.Line + 1, this.Column, precedingSource);
        }
    }
}
