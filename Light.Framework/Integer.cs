using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Light.Framework {
    // This is quite stupid for now with regards to operator support, for example
    // But it is a simplest abstraction that works
    public struct Integer : IEquatable<Integer> {
        public IntegerKind Kind { get; private set; }
        public int Int32Value { get; private set; }
        public BigInteger BigIntegerValue { get; private set; }

        public Integer(int value) : this() {
            this.Int32Value = value;
        }

        public Integer(byte[] bytes) : this() {
            this.Kind = IntegerKind.BigInteger;
            this.BigIntegerValue = new BigInteger(bytes);
        }

        private Integer(BigInteger bigInteger) : this() {
            this.Kind = IntegerKind.BigInteger;
            this.BigIntegerValue = bigInteger;
        }

        public override string ToString() {
            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value.ToString();

            return this.BigIntegerValue.ToString();
        }

        public Range<Integer> RangeTo(Integer to) {
            if (this.Kind == IntegerKind.BigInteger || to.Kind == IntegerKind.BigInteger)
                throw new NotImplementedException();

            return new Range<Integer>(this, to, Enumerable.Range(this.Int32Value, to.Int32Value - this.Int32Value).Select(i => new Integer(i)));
        }

        public bool IsGreaterThan(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: comparing integers of different kinds is not supported.");

            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value > other.Int32Value;

            return this.BigIntegerValue > other.BigIntegerValue;
        }

        public bool IsLessThan(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: comparing integers of different kinds is not supported.");

            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value < other.Int32Value;

            return this.BigIntegerValue < other.BigIntegerValue;
        }

        public Integer Modulus(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: operations on integers of different kinds are not supported.");

            if (this.Kind == IntegerKind.Int32)
                return new Integer(this.Int32Value % other.Int32Value);

            return new Integer(this.BigIntegerValue % other.BigIntegerValue);
        }

        public Integer Plus(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: operations on integers of different kinds are not supported.");

            if (this.Kind == IntegerKind.Int32)
                return new Integer(this.Int32Value + other.Int32Value);

            return new Integer(this.BigIntegerValue + other.BigIntegerValue);
        }

        public Integer Minus(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: operations on integers of different kinds are not supported.");

            if (this.Kind == IntegerKind.Int32)
                return new Integer(this.Int32Value - other.Int32Value);

            return new Integer(this.BigIntegerValue - other.BigIntegerValue);
        }

        public Integer MultiplyBy(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: operations on integers of different kinds are not supported.");

            if (this.Kind == IntegerKind.Int32)
                return new Integer(this.Int32Value * other.Int32Value);

            return new Integer(this.BigIntegerValue * other.BigIntegerValue);
        }

        public Integer DivideBy(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: operations on integers of different kinds are not supported.");

            if (this.Kind == IntegerKind.Int32)
                return new Integer(this.Int32Value / other.Int32Value);

            return new Integer(this.BigIntegerValue / other.BigIntegerValue);
        }

        public static Integer Parse(string value) {
            return new Integer(BigInteger.Parse(value));
        }

        public int ToInt32() {
            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value;

            return (int)this.BigIntegerValue;
        }

        public override bool Equals(object obj) {
            if (!(obj is Integer))
                return false;
            
            return this.Equals((Integer)obj);
        }

        public bool Equals(Integer other) {
            if (this.Kind != other.Kind)
                throw new NotImplementedException("Integer: comparing integer of different kinds is not supported.");

            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value == other.Int32Value;

            return this.BigIntegerValue == other.BigIntegerValue;
        }

        public override int GetHashCode() {
            if (this.Kind == IntegerKind.Int32)
                return this.Int32Value.GetHashCode();

            return this.BigIntegerValue.GetHashCode();
        }
    }
}
