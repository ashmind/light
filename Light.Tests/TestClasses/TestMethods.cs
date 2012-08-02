using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Tests.TestClasses {
    public class TestMethods {
        public static object GenericTToObject<T>(T value) {
            throw new NotSupportedException();
        }

        public static Func<T, object> AcceptsGenericTToObject<T>(T value, Func<T, object> function) {
            return function;
        }
    }
}
