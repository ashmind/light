using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;

namespace Light.Vsip.IntegrationTests {
    [TestClass]
    public class SolutionTests : VSTestsBase {
        [TestMethod]
        [HostType("VS IDE")]
        public void CreateEmptySolution() {
            UIThreadInvoker.Invoke((Action)delegate {
                var testUtils = new TestUtils();
                testUtils.CloseCurrentSolution(__VSSLNSAVEOPTIONS.SLNSAVEOPT_NoSave);
                testUtils.CreateEmptySolution(TestContext.TestDir, "EmptySolution");
            });
        }
    }
}
