using System;
using Light.Framework.OperatorInterfaces;

namespace Light.Framework {
    public interface INumber<T> : IEquatable<T>,
                                  IComparable<T>,
                                  IWithMinus<T>,
                                  IWithPlus<T>,
                                  IWithDivideBy<T>,
                                  IWithMultiplyBy<T>,
                                  IWithExponent<T>,
                                  IWithModulus<T>
        where T : INumber<T>
    {
    }
}