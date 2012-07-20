using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Light.Vsip.Language {
    public class LightAuthoringScope : AuthoringScope {
        private readonly object ast;

        public LightAuthoringScope(object ast) {
            this.ast = ast;
        }

        // ParseReason.QuickInfo
        public override string GetDataTipText(int line, int col, out TextSpan span) {
            span = new TextSpan();
            return null;
        }

        // ParseReason.CompleteWord
        // ParseReason.DisplayMemberList
        // ParseReason.MemberSelect
        // ParseReason.MemberSelectAndHilightBraces
        public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason) {
            //IList<Declaration> declarations;
            //switch (reason) {
            //    case ParseReason.CompleteWord:
            //        declarations = resolver.FindCompletions(parseResult, line, col);
            //        break;
            //    case ParseReason.DisplayMemberList:
            //    case ParseReason.MemberSelect:
            //    case ParseReason.MemberSelectAndHighlightBraces:
            //        declarations = resolver.FindMembers(parseResult, line, col);
            //        break;
            //    default:
            //        throw new ArgumentException("reason");
            //}

            //return new Declarations(declarations);
            return null;
        }

        // ParseReason.GetMethods
        public override Microsoft.VisualStudio.Package.Methods GetMethods(int line, int col, string name) {
            //return new Methods(resolver.FindMethods(parseResult, line, col, name));
            return null;
        }

        // ParseReason.Goto
        public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span) {
            span = new TextSpan();
            return null;
        }
    }

}
