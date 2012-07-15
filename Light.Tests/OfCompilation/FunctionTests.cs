using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class FunctionTests {
        [Test]
        [Row(typeof(string), "string", "x")]
        public void CallAndReturn<T>(string argumentType, T value) {
            var code = string.Format(@"
                public class Test
                    public function Identity({0} x)
                        return x
                    end
                end
            ", argumentType).Trim();

            var instance = CompilationHelper.CompileAndGetInstance(code, "Test");
            var returned = instance.Identity(value);

            Assert.AreEqual(returned, value);
        }
    }
}
