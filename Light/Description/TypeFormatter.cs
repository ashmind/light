using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AshMind.Extensions;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.BuiltIn;
using Light.Framework;
using Light.Internal;

namespace Light.Description {
    public class TypeFormatter {
        private readonly BuiltInTypeMap builtIn;

        public TypeFormatter(BuiltInTypeMap builtIn) {
            this.builtIn = builtIn;
        }

        public string Format(IAstTypeReference type) {
            var reflected = type as AstReflectedType;
            if (reflected != null)
                return Format(reflected.ActualType);

            return null;
        }

        public string Format(Type type) {
            if (typeof(Delegate).IsAssignableFrom(type))
                return FormatDelegate(type);

            if (type.IsArray)
                return "[" + Format(type.GetElementType()) + "]";

            var alias = this.builtIn.GetAliasByType(type);
            if (alias != null)
                return alias;

            if (type.IsGenericTypeDefinedAs(typeof(Range<>)))
                return "range<" + Format(type.GetGenericArguments()[0]) + ">";

            if (type.IsPublic)
                return type.FullName;

            var enumerable = type.GetInterfaces().FirstOrDefault(i => i.IsGenericTypeDefinedAs(typeof(IEnumerable<>)));
            if (enumerable != null)
                return Format(enumerable.GetGenericArguments()[0]) + "*";

            return type.FullName;
        }

        private string FormatDelegate(Type type) {
            var builder = new StringBuilder();
            var invoke = type.GetMethod("Invoke");
            var parameters = invoke.GetParameters();

            if (parameters.Length > 1)
                builder.Append("(");

            builder.AppendJoin(",", parameters.Select(p => Format(p.ParameterType)));

            if (parameters.Length > 1)
                builder.Append(")");

            builder.Append(" => ");
            builder.Append(Format(invoke.ReturnType));
            return builder.ToString();
        }
    }
}