using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class CallAndMemberAccessTests {
        [Test]
        [Row("x", "x")]
        public void Simple(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("x()", "x()")]
        [Row("x.y()", "x.y()")]
        [Row("x.y.z()", "x.y.z()")]
        [Row("x.y().z()", "x.y().z()")]
        public void Call(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("x.x", "x.x")]
        [Row("x.y.z", "x.y.z")]
        public void Member(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }
    }
}
