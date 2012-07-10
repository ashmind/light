using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class TypeTests {
        [Test]
        [Row("class")]
        [Row("interface")]
        public void DefinitionType(string type) {
            var parser = new LightParser();
            var result = parser.Parse("public " + type + " X\r\nend");

            AssertEx.That(() => !result.HasErrors);
            Assert.AreEqual(type, result.Root.Child<TypeDefinition>().DefinitionType);
        }
    }
}
