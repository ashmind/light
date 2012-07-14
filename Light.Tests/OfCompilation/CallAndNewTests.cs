using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class CallAndNewTests {
        [Test]
        public void NewExternalObjectWithoutArguments() {
            var result = CompilationHelper.CompileAndEvaluate("new Random()");
            Assert.IsInstanceOfType<Random>(result);
        }

        [Test]
        public void NewExternalObjectWithArguments() {
            var result = CompilationHelper.CompileAndEvaluate("new Version('1.1.1.1')");
            Assert.AreEqual(new Version("1.1.1.1"), result);
        }
    }
}
