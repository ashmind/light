using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Parsing {
    public class SourceLocation {
        public SourceLocation(int line, int column) {
            this.Line = line;
            this.Column = column;
        }

        public int Line { get; private set; }
        public int Column { get; private set; }

        public override string ToString() {
            return string.Format("line {0}, column {1}", this.Line, this.Column);
        }
    }
}
