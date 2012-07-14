using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class FunctionTests {
        [Test]
        [Row("string", "x")]
        public void CallAndReturn(string argumentAndReturnType, object value) {
            var code = string.Format(@"
                public class Test
                    public function Identity({0} x)
                        return x
                    end
                end
            ", argumentAndReturnType).Trim();

            var compiled = CompilationHelper.CompileCode(code);
            var type = compiled.GetType("Test", true);
            var instance = Activator.CreateInstance(type);
            var returned = type.GetMethod("Identity").Invoke(instance, new[] { value });

            Assert.AreEqual(returned, value);
        }
    }
}
