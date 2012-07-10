using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Light.Ast;
using Light.Compilation;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProgramTests {
        [Test]
        public void HelloWorld() {
            Assert.DoesNotThrow(() => CompilationHelper.CompileFromResource("Programs.HelloWorld.light", CompilationTarget.Console));
        }
    }
}
