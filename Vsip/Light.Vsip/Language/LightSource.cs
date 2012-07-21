using System;
using System.Collections.Generic;
using System.Linq;
using Light.Parsing;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Light.Vsip.Language {
    public class LightSource : Source {
        public ParsingResult Parsed { get; set; }

        public LightSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer) {
        }
    }
}
