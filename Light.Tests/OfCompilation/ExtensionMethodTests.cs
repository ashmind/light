using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ExtensionMethodTests {
        [Test]
        public void ExternalExtensionMethodWithNoOtherArguments() {
            var result = CompilationHelper.CompileAndEvaluate("[1,1,1,2].Distinct()");
            Assert.AreElementsEqualIgnoringOrder(new[] { new Integer(1), new Integer(2) }, result);
        }
    }
}
