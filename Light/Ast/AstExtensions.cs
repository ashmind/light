using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public static class AstExtensions {
        public static TAstElement Child<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("ancestor", ancestor);
            return ancestor.Children<TAstElement>().SingleOrDefault();
        }

        public static IEnumerable<TAstElement> Children<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("ancestor", ancestor);
            return ancestor.Children().OfType<TAstElement>();
        }

        public static IEnumerable<TAstElement> Descendants<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("ancestor", ancestor);
            return ancestor.Descendants().OfType<TAstElement>();
        }

        public static IEnumerable<IAstElement> Descendants(this IAstElement ancestor) {
            Argument.RequireNotNull("ancestor", ancestor);
            foreach (var child in ancestor.Children()) {
                yield return child;
                foreach (var descendant in child.Descendants()) {
                    yield return descendant;
                }
            }
        }
    }
}
