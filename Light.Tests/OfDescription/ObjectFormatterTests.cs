using System;
using System.Collections.Generic;
using System.Linq;
using Light.Description;
using Light.Framework;
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
        public void Enumerable() {
            var enumerable = new[] { 6, 5, 4, 3, 2, 1 }.OrderBy(x => x);
            var result = new ObjectFormatter { AllowPotentialSideEffects = true }.Format(enumerable);
            Assert.AreEqual("(1, 2, 3, 4, 5, …)", result);
        }

        [Test]
        public void Range() {
            var range = new Range<int>(0, 10, System.Linq.Enumerable.Range(0, 11));
            var result = new ObjectFormatter().Format(range);
            Assert.AreEqual("0..10", result);
        }

        [Test]
        public void StaticDelegate() {
            var result = new ObjectFormatter().Format((Func<double, TimeSpan>)(TimeSpan.FromMinutes));
            Assert.AreEqual("<function:TimeSpan.FromMinutes>", result);
        }
    }
}
