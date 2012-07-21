using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Parsing {
    public struct SourceLocation {
        private readonly string source;

        public SourceLocation(int line, int column, int position, string source) : this() {
            this.Line = line;
            this.Column = column;
            this.Position = position;

            this.source = source;
        }

        public int Line     { get; private set; }
        public int Column   { get; private set; }
        public int Position { get; private set; }

        public override string ToString() {
            var precedingSource = this.source.Substring(0, this.Position).Trim();
            if (precedingSource.Length == 0)
                return string.Format("line {0}, column {1}", this.Line + 1, this.Column);

            var indexOfLastWhitespace = precedingSource.LastIndexOfAny(new[] {' ', '\r', '\n', '\t'});
            precedingSource = indexOfLastWhitespace >= 0
                            ? precedingSource.Substring(indexOfLastWhitespace + 1, precedingSource.Length - indexOfLastWhitespace - 1)
                            : precedingSource;
            return string.Format("line {0}, column {1}, after {2}", this.Line + 1, this.Column, precedingSource);
        }
    }
}
