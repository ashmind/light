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
        public void NearbyMethodCallWithNoArguments() {
            var caller = CompilationHelper.CompileAndGetInstance(@"
                public class Callee
                    function Return3()
                        return 3
                    end
                end
                
                public class Caller
                    function ReturnFromCallee()
                        let callee = new Callee()
                        return callee.Return3()
                    end
                end
            ".Trim(), "Caller");
            Assert.AreEqual(3, caller.ReturnFromCallee());
        }
    }
}
