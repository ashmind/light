using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Parsing {
    public struct SourceSpan {
        public SourceSpan(SourceLocation start, SourceLocation end) : this() {
            this.Start = start;
            this.End = end;
        }

        public SourceLocation Start { get; private set; }
        public SourceLocation End   { get; private set; }

        public int Length {
            get { return this.End.Position - this.Start.Position; }
        }
    }
}
