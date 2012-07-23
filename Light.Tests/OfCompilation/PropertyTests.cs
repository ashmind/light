using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class PropertyTests {
        [Test]
        public void ExternalObjectInstanceProperty() {
            var result = CompilationHelper.CompileAndEvaluate("'abc'.Length");
            Assert.AreEqual(3, result);
        }
    }
}
