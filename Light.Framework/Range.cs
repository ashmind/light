using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework {
    public class Range<T> : IEnumerable<T>, IRange {
        private readonly IEnumerable<T> enumerable;

        public T From { get; private set; }
        public T To   { get; private set; }

        public Range(T from, T to, IEnumerable<T> enumerable) {
            this.enumerable = enumerable;
            this.From = @from;
            this.To = to;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return this.enumerable.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return this.enumerable.GetEnumerator();
        }

        #endregion

        #region IRange Members

        object IRange.From {
            get { return this.From; }
        }

        object IRange.To {
            get { return this.To; }
        }

        #endregion
    }
}
