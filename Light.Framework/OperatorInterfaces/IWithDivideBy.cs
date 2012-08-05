using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithDivideBy<T>
        where T : IWithDivideBy<T> 
    {
        T DivideBy(T value);
    }
}
