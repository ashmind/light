using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework.OperatorInterfaces {
    public interface IWithPlus<T> 
        where T : IWithPlus<T> 
    {
        T Plus(T value);
    }
}
