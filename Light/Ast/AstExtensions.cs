using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public static class AstExtensions {
        private static readonly AstElementTransform IdentityTransform = e => e;

        public static IEnumerable<IAstElement> Children(this IAstElement parent)
        {
            Argument.RequireNotNull("parent", parent);
            return parent.VisitOrTransformChildren(IdentityTransform);
        }

        public static IEnumerable<TAstElement> Children<TAstElement>(this IAstElement parent)
            where TAstElement : IAstElement
        {
            return parent.Children().OfType<TAstElement>();
        }

        public static TAstElement Child<TAstElement>(this IAstElement parent)
            where TAstElement : IAstElement 
        {
            Argument.RequireNotNull("parent", parent);
            return parent.Children<TAstElement>().SingleOrDefault();
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

        public static IEnumerable<TAstElement> Descendants<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("ancestor", ancestor);
            return ancestor.Descendants().OfType<TAstElement>();
        }

        public static TAstElement Descendant<TAstElement>(this IAstElement ancestor)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("ancestor", ancestor);
            return ancestor.Descendants<TAstElement>().SingleOrDefault();
        }

        public static void TransformChildren(this IAstElement parent, AstElementTransform transform) {
            Argument.RequireNotNull("parent", parent);
            Argument.RequireNotNull("transform", transform);

            var enumerator = parent.VisitOrTransformChildren(transform).GetEnumerator();
            while (enumerator.MoveNext()) { }
        }

        public static void TransformChildren<TAstElement>(this IAstElement parent, AstElementTransform<TAstElement> transform)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("transform", transform);
            parent.TransformChildren(c => {
                if (!(c is TAstElement))
                    return c;

                return transform((TAstElement)c);
            });
        }

        public static void TransformDescendants(this IAstElement ancestor, AstElementTransform transform) {
            Argument.RequireNotNull("transform", transform);

            ancestor.TransformChildren(c => {
                c.TransformDescendants(transform);
                return transform(c);
            });
        }

        public static void TransformDescendants<TAstElement>(this IAstElement ancestor, AstElementTransform<TAstElement> transform)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("transform", transform);

            ancestor.TransformDescendants(c => {
                c.TransformDescendants(transform);
                if (!(c is TAstElement))
                    return c;

                return transform((TAstElement)c);
            });
        }

        public static IEnumerable<TAstElement> Transform<TAstElement>(this IList<TAstElement> elements, AstElementTransform transform)
            where TAstElement : IAstElement
        {
            Argument.RequireNotNull("elements", elements);
            Argument.RequireNotNull("transform", transform);

            for (var i = 0; i < elements.Count; i++) {
                var transformed = (TAstElement)transform(elements[i]);
                if (!object.ReferenceEquals(transformed, elements[i]))
                    elements[i] = transformed;

                yield return elements[i];
            }
        }
    }
}
