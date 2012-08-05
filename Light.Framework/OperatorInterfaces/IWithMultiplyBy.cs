using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithMultiplyBy<T>
        where T : IWithMultiplyBy<T> 
    {
        T MultiplyBy(T value);
    }
}
