using System;
using System.Collections.Generic;
using System.Linq;
using Light.Vsip.Internal;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Light.Vsip.IntegrationTests {
    [TestClass]
    public class PackageTests : VSTestsBase {
        [TestMethod]
        [HostType("VS IDE")]
        public void PackageLoadTest() {
            UIThreadInvoker.Invoke((Action)delegate {
                //Get the Shell Service
                var shellService = VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell)) as IVsShell;
                Assert.IsNotNull(shellService);

                //Validate package load
                IVsPackage package;
                var packageGuid = Guids.Package;
                Assert.AreEqual(0, shellService.LoadPackage(ref packageGuid, out package));
                Assert.IsNotNull(package, "Package failed to load");
            });
        }
    }
}
