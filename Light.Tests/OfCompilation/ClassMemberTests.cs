using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ClassMemberTests {
        [Test]
        [Row(typeof(string), "string", "x")]
        public void WriteAndReadPropertyThroughMethods<T>(string propetyType, T value) {
            var code = string.Format(@"
                public class Test
                    private {0} x
                    
                    public function SetValue({0} value)
                        x = value
                    end

                    public function GetValue()
                        return x
                    end
                end
            ", propetyType).Trim();

            var instance = CompilationHelper.CompileAndGetInstance(code, "Test");
            instance.SetValue(value);

            Assert.AreEqual(instance.GetValue(), value);
        }
    }
}
