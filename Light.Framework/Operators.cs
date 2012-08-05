using System;
using System.Collections.Generic;
using Light.Framework.OperatorInterfaces;

namespace Light.Framework {
    public static class Operators {
        public static T Plus<T>(T left, T right) 
            where T : IWithPlus<T>
        {
            return left.Plus(right);
        }

        public static T Minus<T>(T left, T right)
            where T : IWithMinus<T> 
        {
            return left.Minus(right);
        }

        public static T Divide<T>(T left, T right)
            where T : IWithDivideBy<T>
        {
            return left.DivideBy(right);
        }

        public static T Multiply<T>(T left, T right)
            where T : IWithMultiplyBy<T>
        {
            return left.MultiplyBy(right);
        }

        public static Range<T> Range<T>(T left, T right)
            where T : IWithRangeTo<T>
        {
            return left.RangeTo(right);
        }

        public static T Modulus<T>(T left, T right)
            where T : IWithModulus<T>
        {
            return left.Modulus(right);
        }

        public static bool IsGreater<T>(T left, T right)
            where T : IComparable<T>
        {
            return left.CompareTo(right) > 0;
        }

        public static bool IsLess<T>(T left, T right)
            where T : IComparable<T>
        {
            return left.CompareTo(right) < 0;
        }

        public static bool Equals<T>(T left, T right)
            where T : IEquatable<T>
        {
            return left.Equals(right);
        }
    }
}