using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class CallAndNewTests {
        [Test]
        public void NewExternalObject() {
            var result = CompilationHelper.CompileAndEvaluate("new Random()");
            Assert.IsInstanceOfType<Random>(result);
        }
    }
}
