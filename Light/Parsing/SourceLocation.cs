using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Parsing {
    public class SourceLocation {
        public SourceLocation(int column) {
            this.Column = column;
        }

        public int Column { get; private set; }
    }
}
