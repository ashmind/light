using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithMinus<T>
        where T : IWithMinus<T> 
    {
        T Minus(T value);
    }
}
