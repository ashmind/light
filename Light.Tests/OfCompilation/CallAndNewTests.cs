using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class CallAndNewTests {
        [Test]
        public void NewExternalObjectWithNoArguments() {
            var result = CompilationHelper.CompileAndEvaluate("new Random()");
            Assert.IsInstanceOfType<Random>(result);
        }

        [Test]
        public void NewExternalObjectWithArguments() {
            var result = CompilationHelper.CompileAndEvaluate("new Version('1.1.1.1')");
            Assert.AreEqual(new Version("1.1.1.1"), result);
        }

        [Test]
        [Row("", "3", "", 3)]
        [Row("string value",  "value", "'abc'", "abc")]
        [Row("integer value", "value", "3",     3)]
        public void NearbyMethodCallOnNewObject(string parameters, string returnValue, string arguments, object expectedValue) {
            var caller = CompilationHelper.CompileAndGetInstance(string.Format(@"
                public class Callee
                    function GetValue({0})
                        return {2}
                    end
                end
                
                public class Caller
                    function GetValueFromCallee()
                        let callee = new Callee()
                        return callee.GetValue({1})
                    end
                end
            ", parameters, arguments, returnValue).Trim(), "Caller");
            Assert.AreEqual(TestArgumentConverter.Convert(expectedValue), caller.GetValueFromCallee());
        }

        [Test]
        [Row("'abc'", "abc")]
        [Row("3",     3)]
        public void NearbyMethodCallOnParameter(string returnValue, object expectedValue) {
            var compiled = CompilationHelper.CompileCode(string.Format(@"
                public class Callee
                    function GetValue()
                        return {0}
                    end
                end
                
                public class Caller
                    function GetValueFromCallee(Callee callee)
                        return callee.GetValue()
                    end
                end
            ", returnValue).Trim());

            var callee = (dynamic)Activator.CreateInstance(compiled.GetType("Callee"));
            var caller = (dynamic)Activator.CreateInstance(compiled.GetType("Caller"));
            Assert.AreEqual(TestArgumentConverter.Convert(expectedValue), caller.GetValueFromCallee(callee));
        }

        [Test]
        public void ExternalMethodCallOnValueFromNearbyCall() {
            var caller = CompilationHelper.CompileAndGetInstance(string.Format(@"
                public class Callee
                    function GetValue()
                        return 'abc'
                    end
                end
                
                public class Caller
                    function GetValueFromCallee()
                        let callee = new Callee()
                        return callee.GetValue().Substring((1).ToInt32())
                    end
                end
            ").Trim(), "Caller");

            Assert.AreEqual("bc", caller.GetValueFromCallee());
        }
    }
}
