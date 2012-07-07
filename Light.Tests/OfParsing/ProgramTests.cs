using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class ProgramTests {
        [Test]
        public void Question() {
            AssertIsParsed("Question.light");
        }

        private static void AssertIsParsed(string programName) {
            var code = Resource.ReadAllText(typeof(ProgramTests), "Programs." + programName);
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
