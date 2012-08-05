using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithRangeTo<T>
        where T : IWithRangeTo<T> 
    {
        Range<T> RangeTo(T value);
    }
}
