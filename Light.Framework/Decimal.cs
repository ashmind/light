using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Framework {
    public struct Decimal : INumber<Decimal> {
        private readonly decimal value;

        public Decimal(decimal value) {
            this.value = value;
        }

        public Decimal(int[] bits) {
            this.value = new decimal(bits);
        }

        public bool Equals(Decimal other) {
            throw new NotImplementedException();
        }

        public int CompareTo(Decimal other) {
            throw new NotImplementedException();
        }

        public Decimal Minus(Decimal other) {
            return new Decimal(this.value - other.value);
        }

        public Decimal Plus(Decimal other) {
            return new Decimal(this.value + other.value);
        }

        public Decimal DivideBy(Decimal other) {
            return new Decimal(this.value / other.value);
        }

        public Decimal MultiplyBy(Decimal other) {
            throw new NotImplementedException();
        }

        public Decimal Exponent(Decimal other) {
            return new Decimal((decimal)Math.Pow((double)this.value, (double)other.value));
        }

        public Decimal Modulus(Decimal other) {
            throw new NotImplementedException();
        }

        public Range<Decimal> RangeTo(Decimal other) {
            throw new NotImplementedException();
        }

        public int[] GetBits() {
            return System.Decimal.GetBits(this.value);
        }

        public double ToDouble() {
            return (double)this.value;
        }

        public override string ToString() {
            return this.value.ToString();
        }
    }
}
