using Irony.Parsing;
using Light.Parsing;
using Microsoft.VisualStudio.Package;
using TokenColor = Microsoft.VisualStudio.Package.TokenColor;
using TokenTriggers = Microsoft.VisualStudio.Package.TokenTriggers;
using TokenType = Microsoft.VisualStudio.Package.TokenType;

namespace Light.Vsip.Language {
    public class LightScanner : IScanner {
        private readonly Parser parser;

        public LightScanner(LightGrammar grammar) {
            this.parser = new Parser(grammar) {
                Context = { Mode = ParseMode.VsLineScan }
            };
        }

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state) {
            // Reads each token in a source line and performs syntax coloring.  It will continue to
            // be called for the source until false is returned.
            var token = parser.Scanner.VsReadToken(ref state);

            if (token != null && token.Terminal != Grammar.CurrentGrammar.Eof && token.Category != TokenCategory.Error)
            {
                tokenInfo.StartIndex = token.Location.Position;
                tokenInfo.EndIndex = tokenInfo.StartIndex + token.Length - 1;
                var editorInfo = token.KeyTerm != null ? token.KeyTerm.EditorInfo : token.EditorInfo;

                tokenInfo.Color = (TokenColor)editorInfo.Color;
                tokenInfo.Type = (TokenType)editorInfo.Type;
                tokenInfo.Trigger = (TokenTriggers)editorInfo.Triggers;

                return true;
            }

            return false;
        }

        public void SetSource(string source, int offset) {
            parser.Scanner.VsSetSource(source, offset);
        }
    }
}