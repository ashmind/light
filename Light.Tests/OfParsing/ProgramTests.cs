using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class ProgramTests {
        [Test]
        [Row("Question.light")]
        public void Simple(string name) {
            var code = Resource.ReadAllText(typeof(ProgramTests), "Programs." + name);
            var parsed = new LightParser().Parse(code);

            Assert.IsFalse(
                parsed.HasErrors,
                "Errors while parsing: {0}{1}",
                    Environment.NewLine,
                    string.Join(Environment.NewLine, parsed.Messages)
            );
        }
    }
}
