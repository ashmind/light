using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Internal;

namespace Light.Description {
    public class TypeFormatter {
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

            return type.Name;
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