using System.Collections;

namespace Light.Framework {
    public interface IRange : IEnumerable {
        object From { get; }
        object To { get; }
    }
}