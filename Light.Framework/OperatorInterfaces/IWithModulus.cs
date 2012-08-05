using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithModulus<T>
        where T : IWithModulus<T> 
    {
        T Modulus(T value);
    }
}
