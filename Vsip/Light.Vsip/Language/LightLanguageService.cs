using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Irony.Ast;
using Irony.Parsing;
using Light.Parsing;
using Light.Vsip.Internal;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Light.Vsip.Language {
    [Guid(Guids.LanguageServiceString)]
    public class LightLanguageService : LanguageService {
        private readonly LightGrammar grammar;
        private readonly Parser parser;

        private LanguagePreferences preferences;
        private LightScanner scanner;

        public LightLanguageService() {
            grammar = new LightGrammar();
            parser = new Parser(grammar);
        }

        public override LanguagePreferences GetLanguagePreferences() {
            if (this.preferences == null) {
                this.preferences = new LanguagePreferences(this.Site, typeof(LightLanguageService).GUID, this.Name);
                this.preferences.Init();
            }

            return this.preferences;
        }

        public override IScanner GetScanner(IVsTextLines buffer) {
            if (this.scanner == null)
                this.scanner = new LightScanner(grammar);

            return this.scanner;
        }

        public override Source CreateSource(IVsTextLines buffer) {
            return new LightSource(this, buffer, this.GetColorizer(buffer));
        }

        public override void OnIdle(bool periodic) {
            var source = GetSource(this.LastActiveTextView);
            if (source != null && source.LastParseTime >= Int32.MaxValue >> 12)
                source.LastParseTime = 0;

            base.OnIdle(periodic);
        }

        public override AuthoringScope ParseSource(ParseRequest req) {
            Debug.Print("ParseSource at ({0}:{1}), reason {2}", req.Line, req.Col, req.Reason);
            var source = (LightSource)this.GetSource(req.FileName);

            switch (req.Reason) {
                case ParseReason.Check:
                    // This is where you perform your syntax highlighting.
                    // Parse entire source as given in req.Text.
                    // Store results in the AuthoringScope object.
                    var parsed = parser.Parse(req.Text, req.FileName);
                    source.Ast = parsed.Root.AstNode;

                    if (parsed.ParserMessages.Count > 0) {
                        foreach (var error in parsed.ParserMessages) {
                            var span = new TextSpan();
                            span.iStartLine = span.iEndLine = error.Location.Line;
                            span.iStartIndex = error.Location.Column;
                            span.iEndIndex = error.Location.Position;
                            req.Sink.AddError(req.FileName, error.Message, span, Severity.Error);
                        }
                    }
                    break;

                case ParseReason.DisplayMemberList:
                    // Parse the line specified in req.Line for the two
                    // tokens just before req.Col to obtain the identifier
                    // and the member connector symbol.
                    // Examine existing parse tree for members of the identifier
                    // and return a list of members in your version of the
                    // Declarations class as stored in the AuthoringScope
                    // object.
                    break;

                case ParseReason.MethodTip:
                    // Parse the line specified in req.Line for the token
                    // just before req.Col to obtain the name of the method
                    // being entered.
                    // Examine the existing parse tree for all method signatures
                    // with the same name and return a list of those signatures
                    // in your version of the Methods class as stored in the
                    // AuthoringScope object.
                    break;
            }

            return new LightAuthoringScope(source.Ast);
        }

        public override string GetFormatFilterList() {
            return "";
        }

        public override string Name {
            get { return "Light"; }
        }
    }
}
