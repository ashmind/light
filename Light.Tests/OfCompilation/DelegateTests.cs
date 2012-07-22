using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class DelegateTests {
        [Test]
        public void ExternalStaticMethodAsDelegate() {
            var result = CompilationHelper.CompileAndEvaluate("TimeSpan.FromMinutes");
            Assert.IsInstanceOfType<Func<double, TimeSpan>>(result);
        }
    }
}
