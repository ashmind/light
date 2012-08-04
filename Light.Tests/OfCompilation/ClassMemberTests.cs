using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ClassMemberTests {
        [Test]
        [Row(typeof(string),  "string", "x")]
        [Row(typeof(Integer), "integer", 3)]
        [Row(typeof(bool),    "boolean", true)]
        public void WriteAndReadPropertyThroughMethods<T>(string propertyType, object rawValue) {
            var value = (T)TestArgumentConverter.Convert(rawValue);
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
            ", propertyType).Trim();

            var instance = CompilationHelper.CompileAndGetInstance(code, "Test");
            instance.SetValue(value);

            Assert.AreEqual(value, instance.GetValue());
        }

        [Test]
        public void ReturnThis() {
            var code = string.Format(@"
                public class Test
                    public function GetThis()
                        return this
                    end
                end
            ").Trim();

            var instance = CompilationHelper.CompileAndGetInstance(code, "Test");
            var value = instance.GetThis();

            Assert.AreEqual(instance, value);
        }

        [Test]
        public void ReturnPropertyThroughThis() {
            var code = string.Format(@"
                public class Test
                    private integer value = 5

                    public function GetValue()
                        return this.value
                    end
                end
            ").Trim();

            var instance = CompilationHelper.CompileAndGetInstance(code, "Test");
            var value = instance.GetValue();

            Assert.AreEqual(new Integer(5), value);
        }
    }
}
