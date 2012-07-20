using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace Light.Vsip.IntegrationTests {
    [TestClass]
    public class ProjectTests : VSTestsBase {
        [TestMethod]
        [HostType("VS IDE")]
        public void WinformsApplication() {
            UIThreadInvoker.Invoke((Action)delegate {
                var testUtils = new TestUtils();

                testUtils.CreateEmptySolution(TestContext.TestDir, "CSWinApp");
                Assert.AreEqual(0, testUtils.ProjectCount());

                //Create Winforms application project
                //TestUtils.CreateProjectFromTemplate("MyWindowsApp", "Windows Application", "CSharp", false);
                //Assert.AreEqual<int>(1, TestUtils.ProjectCount());

                //TODO Verify that we can debug launch the application

                //TODO Set Break point and verify that will hit

                //TODO Verify Adding new project item to project

            });
        }

    }
}
