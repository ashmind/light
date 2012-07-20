using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Light.Vsip.Internal;
using Light.Vsip.Language;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.OLE.Interop;

namespace Light.Vsip
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideServiceAttribute(typeof(LightLanguageService), ServiceName = "Light Language Service")]
    [ProvideLanguageService(typeof(LightLanguageService), "Light", 106, RequestStockColors = true)]
    [ProvideLanguageExtensionAttribute(typeof(LightLanguageService), ".light")]
    [Guid(Guids.PackageString)]
    public sealed class LightPackage : Package, IOleComponent {
        private uint oleComponentID;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public LightPackage() {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize() {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));

            // http://msdn.microsoft.com/en-us/library/bb166498.aspx

            // Proffer the service.
            var languageService = new LightLanguageService();
            languageService.SetSite(this);
            ((IServiceContainer)this).AddService(typeof(LightLanguageService), languageService, true);

            // Register a timer to call our language service during
            // idle periods.
            var oleManager = (IOleComponentManager)GetService(typeof(SOleComponentManager));
            if (oleComponentID == 0 && oleManager != null) {
                var crinfo = new OLECRINFO[1];
                crinfo[0].cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO));
                crinfo[0].grfcrf = (uint)_OLECRF.olecrfNeedIdleTime | (uint)_OLECRF.olecrfNeedPeriodicIdleTime;
                crinfo[0].grfcadvf = (uint)_OLECADVF.olecadvfModal | (uint)_OLECADVF.olecadvfRedrawOff | (uint)_OLECADVF.olecadvfWarningsOff;
                crinfo[0].uIdleTimeInterval = 1000;
                oleManager.FRegisterComponent(this, crinfo, out oleComponentID);
            }

            base.Initialize();
        }

        protected override void Dispose(bool disposing) {
            if (oleComponentID != 0) {
                var oleManager = (IOleComponentManager)GetService(typeof(SOleComponentManager));
                if (oleManager != null)
                    oleManager.FRevokeComponent(oleComponentID);

                oleComponentID = 0;
            }

            base.Dispose(disposing);
        }

        public int FDoIdle(uint grfidlef) {
            var periodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
            var service = (LightLanguageService)GetService(typeof(LightLanguageService));
            if (service != null)
                service.OnIdle(periodic);

            return 0;
        }

        #region IOleComponent Members

        int IOleComponent.FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked) {
            return 1;
        }

        int IOleComponent.FPreTranslateMessage(MSG[] pMsg) {
            return 0;
        }

        int IOleComponent.FQueryTerminate(int fPromptUser) {
            return 1;
        }

        int IOleComponent.FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam) {
            return 1;
        }

        IntPtr IOleComponent.HwndGetWindow(uint dwWhich, uint dwReserved) {
            return IntPtr.Zero;
        }

        void IOleComponent.OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved) {
        }

        void IOleComponent.OnAppActivate(int fActive, uint dwOtherThreadID) {
        }

        void IOleComponent.OnEnterState(uint uStateID, int fEnter) {
        }

        void IOleComponent.OnLoseActivation() {
        }

        void IOleComponent.Terminate() {
        }

        #endregion
    }
}
