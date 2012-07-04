using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.Linq.Expressions;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class OperatorTests {
        [Test]
        [Row("1+2", ExpressionType.Add)]
        public void Binary(string code, ExpressionType expectedNodeType) {
            var parser = new LightParser();
            var result = parser.Parse(code);

            AssertEx.That(() => !result.HasErrors
                 && ((BinaryExpression)result.Tree).NodeType == expectedNodeType);
        }
    }
}
