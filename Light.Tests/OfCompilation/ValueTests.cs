using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ValueTests {
        [Test]
        [Row(  "1",    1)]
        [Row("1.1",  1.1)]
        [Row("'x'",  "x")]
        [Row("true", true)]
        public void FunctionReturn(string valueString, object expectedValue) {
            var code = (@"
                public class Test
                    public function GetValue()
                        return " + valueString + @"
                    end
                end
            ").Trim();

            var instance = CompilationHelper.CompileCodeAndGetInstance(code, "Test");
            var value = instance.GetValue();

            Assert.AreEqual(expectedValue, value);
        }
    }
}
