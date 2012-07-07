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
        [Row("x(a)", "x(a)")]
        [Row("x.y(a)", "x.y(a)")]
        [Row("x.y.z(a)", "x.y.z(a)")]
        [Row("x.y(a).z(b)", "x.y(a).z(b)")]
        [Row("x.x(x.x(x.x()))", "x.x(x.x(x.x()))")]
        public void Call(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("x.x", "x.x")]
        [Row("x.y.z", "x.y.z")]
        public void Member(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("x.x[1]",     "x.x[1]")]
        [Row("x.y[1].z",   "x.y[1].z")]
        [Row("x.y[x.y].z", "x.y[x.y].z")]
        [Row("x.y[1].z()", "x.y[1].z()")]
        public void Indexer(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }
    }
}
