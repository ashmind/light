using System;
using System.Collections.Generic;
using System.Linq;
using Light.Description;
using MbUnit.Framework;

namespace Light.Tests.OfDescription {
    [TestFixture]
    public class ObjectFormatterTests {
        [Test]
        [Row(true,  "true")]
        [Row(false, "false")]
        public void Booleans(bool value, string expectedResult) {
            var result = new ObjectFormatter().Format(value);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [Row(new[] {1, 2}, "[1, 2]")]
        [Row(new object[] { new[] { 1 }, 2 }, "[[1], 2]")]
        public void Arrays(object array, string expectedResult) {
            var result = new ObjectFormatter().Format(array);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void StaticDelegate() {
            var result = new ObjectFormatter().Format((Func<double, TimeSpan>)(TimeSpan.FromMinutes));
            Assert.AreEqual("<function:TimeSpan.FromMinutes>", result);
        }
    }
}
