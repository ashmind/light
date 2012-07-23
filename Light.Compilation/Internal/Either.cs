using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Compilation.Internal {
    public class Either<T1, T2> {
        private readonly int kind;
        private readonly T1 value1;
        private readonly T2 value2;

        static Either() {
            if (typeof(T1) == typeof(T2))
                throw new InvalidOperationException("Either requires generic parameter types to be distinct.");
        } 

        public Either(T1 value) {
            this.value1 = value;
            this.kind = 1;
        }

        public Either(T2 value) {
            this.value2 = value;
            this.kind = 2;
        }

        public T1OrT2 As<T1OrT2>()
            where T1OrT2 : class
        {
            return kind == 1 ? value1 as T1OrT2 : value2 as T1OrT2;
        }

        public Either<TNew1, TNew2> Cast<TNew1, TNew2>()
            where TNew1 : T1
            where TNew2 : T2
        {
            return this.kind == 1
                 ? new Either<TNew1, TNew2>((TNew1)this.value1)
                 : new Either<TNew1, TNew2>((TNew2)this.value2);
        }

        public static implicit operator Either<T1, T2>(T1 value) {
            return new Either<T1, T2>(value);
        }

        public static implicit operator Either<T1, T2>(T2 value) {
            return new Either<T1, T2>(value);
        }

        public static explicit operator T1(Either<T1, T2> either) {
            if (either == null)
                return (T1)(object)null;

            return either.kind == 1 ? either.value1 : (T1)(object)either.value2;
        }

        public static explicit operator T2(Either<T1, T2> either) {
            if (either == null)
                return (T2)(object)null;

            return either.kind == 2 ? either.value2 : (T2)(object)either.value1;
        }

        public Type GetActualType() {
            return kind == 1 ? this.value1.GetType() : this.value2.GetType();
        }
    }
}
