using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ValueTests {
        [Test]
        [Row("1", 1)]
        [Ignore("WIP")]
        public void FunctionReturn(string valueString, object expectedValue) {
            var code = (@"
                public class Test
                    public function GetValue()
                        return " + valueString + @"
                    end
                end
            ").Trim();
            var compiled = CompilationHelper.CompileCode(code);
            var type = compiled.GetType("Test", true);
            var instance = Activator.CreateInstance(type);
            var value = type.GetMethod("GetValue").Invoke(instance, null);

            Assert.AreEqual(expectedValue, value);
        }
    }
}
