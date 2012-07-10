using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast {
    public static class AstExtensions {
        public static IEnumerable<TAstElement> Descendants<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            return ancestor.Descendants().OfType<TAstElement>();
        }

        public static IEnumerable<IAstElement> Descendants(this IAstElement ancestor) {
            foreach (var child in ancestor.Children()) {
                yield return child;
                foreach (var descendant in child.Descendants()) {
                    yield return descendant;
                }
            }
        }
    }
}
