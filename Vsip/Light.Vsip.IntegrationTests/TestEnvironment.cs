using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MbUnit.Framework;

namespace Light.Vsip.IntegrationTests {
    [AssemblyFixture]
    public static class TestEnvironment {
        public static string WorkingDirectory { get; private set; }

        [FixtureSetUp]
        private static void BeforeAllTests() {
            WorkingDirectory = Path.Combine(Path.GetTempPath(), "Light.Vsip.Tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(WorkingDirectory);
        }

        [FixtureTearDown]
        private static void AfterAllTests() {
            Directory.Delete(WorkingDirectory);
        }
    }
}
