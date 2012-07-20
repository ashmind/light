using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Light.Vsip.Language {
    public class LightSource : Source {
        public object Ast { get; set; }

        public LightSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer) {
        }
    }
}
