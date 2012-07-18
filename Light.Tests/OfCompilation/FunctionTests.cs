using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class FunctionTests {
        [Test]
        [Row("1", 1)]
        [Row("1.1", 1.1)]
        [Row("'x'", "x")]
        [Row("true", true)]
        public void ReturnConstant(string valueString, object expectedValue) {
            var compiled = CompileAndGetClassWith(@"
                public function GetValue()
                    return " + valueString + @"
                end
            ");
            var value = compiled.GetValue();

            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        [Row("1+1",      2)]
        [Row("1*1",      1)]
        [Row("1/1",      1)]
        [Row("1-1",      0)]
        [Row("1>1",      false)]
        [Row("1<1",      false)]
        [Row("1==1",     true)]
        [Row("'x'+'x'",  "xx")]
        [Row("'x'=='x'", true)]
        public void ReturnResultOfBinary(string expressionString, object expectedValue) {
            var compiled = CompileAndGetClassWith(@"
                public function GetValue()
                    return " + expressionString + @"
                end
            ");
            var value = compiled.GetValue();

            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        [Row(typeof(string), "string", "x")]
        public void ReturnArgument<T>(string argumentType, T value) {
            var compiled = CompileAndGetClassWith(@"
                public function Return(" + argumentType + @" x)
                    return x
                end
            ");

            var returned = compiled.Return(value);
            Assert.AreEqual(returned, value);
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
