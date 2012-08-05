using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class FunctionTests {
        [Test]
        [Row(typeof(string), "string ", "a")]
        [Row(typeof(string), "", "a")]
        public void ReturnArgument<T>(string parameterTypeAndSpace, T value) {
            var compiled = CompileAndGetClassWith(@"
                public function Return(" + parameterTypeAndSpace + @"x)
                    return x
                end
            ");

            var returned = compiled.Return(value);
            Assert.AreEqual(returned, value);
        }

        [Test]
        [Row("integer x", "x < 10", 5)]
        [Row("x", "x < 10", 5)]
        public void CompareArgument(string parameter, string condition, object value) {
            var compiled = CompileAndGetClassWith(@"
                public function Compare(" + parameter + @")
                    return " + condition + @"
                end
            ");

            var returned = compiled.Compare(TestArgumentConverter.Convert(value));
            Assert.IsTrue(returned);
        }

        [Test]
        [Row(typeof(string), "string", "x")]
        public void AssignToVariableAndReturn<T>(string argumentType, T value) {
            var compiled = CompileAndGetClassWith(@"
                public function AssignAndReturn(" + argumentType + @" argument)
                    let local = argument
                    return local
                end
            ");

            var returned = compiled.AssignAndReturn(value);
            Assert.AreEqual(returned, value);
        }

        private dynamic CompileAndGetClassWith(string functionCode) {
            var classCode = string.Format(@"
                public class Test
                    {0}
                end
            ", functionCode).Trim();
            return CompilationHelper.CompileAndGetInstance(classCode, "Test");
        }
    }
}
