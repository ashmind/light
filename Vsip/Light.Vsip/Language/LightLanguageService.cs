using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Irony.Parsing;
using Light.Ast;
using Light.Parsing;
using Light.Processing;
using Light.Vsip.Internal;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Light.Vsip.Language {
    [Guid(Guids.LanguageServiceString)]
    public class LightLanguageService : LanguageService {
        private static readonly HashSet<ProcessingStage> ProcessingStages = new HashSet<ProcessingStage> {
            ProcessingStage.ScopeDefinition,
            ProcessingStage.ReferenceResolution
        };

        private readonly LightGrammar grammar;
        private readonly LightParser parser;
        private readonly LightProcessor processor;

        private LanguagePreferences preferences;
        private LightScanner scanner;

        public LightLanguageService(LightGrammar grammar, LightParser parser, LightProcessor processor) {
            this.grammar = grammar;
            this.parser = parser;
            this.processor = processor;
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
                    var parsed = ParseAndProcess(req.Text);
                    if (parsed.Messages.Count > 0) {
                        foreach (var message in parsed.Messages) {
                            var span = new TextSpan();
                            span.iStartLine = span.iEndLine = message.Location.Line;
                            span.iStartIndex = message.Location.Column - 1;
                            span.iEndIndex = message.Location.Column;
                            req.Sink.AddError(req.FileName, message.Text, span, Severity.Error);
                        }
                    }
                    source.Parsed = parsed;
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

            return new LightAuthoringScope(source.Parsed != null ? source.Parsed.Root : null);
        }

        private ParsingResult ParseAndProcess(string text) {
            var parsed = parser.Parse(text);
            try {
                processor.Process(parsed.Root, new ProcessingOptions(ProcessingStages));
            }
            catch {
                // here lies the evil
                // temporary, until good error reporting is implemented in processor steps
            }

            return parsed;
        }

        public override string GetFormatFilterList() {
            return "";
        }

        public override string Name {
            get { return "Light"; }
        }
    }
}
