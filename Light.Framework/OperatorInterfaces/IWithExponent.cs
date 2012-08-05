using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithExponent<T>
        where T : IWithExponent<T> 
    {
        T Exponent(T value);
    }
}
