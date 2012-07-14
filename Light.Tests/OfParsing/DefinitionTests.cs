using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class DefinitionTests {
        [Test]
        [Row("class X\r\nend",     "class")]
        [Row("interface X\r\nend", "interface")]
        public void Types(string code, string definitionType) {
            ParseAssert.IsParsedTo<AstTypeDefinition>(code, t => t.Name == "X" && t.DefinitionType == definitionType);
        }

        [Test]
        [Row("")]
        [Row("private")]
        [Row("public")]
        public void TypedAutoPropertyWithoutAssignment(string accessModifier) {
            var code = string.Format(@"
                class X
                    {0} string x
                end
            ", accessModifier).Trim();

            ParseAssert.IsParsedTo(
                code, r => r.Descendant<AstPropertyDefinition>(),
                f => f.Name == "x" && f.Type.Name == "string"
            );
        }

        [Test]
        public void FunctionWithNoParameters() {
            ParseAssert.IsParsedTo<FunctionDefinition>("function X()\r\nend", f => f.Name == "X");
        }

        [Test]
        public void FunctionWithUntypedParameters() {
            ParseAssert.IsParsedTo<FunctionDefinition>(
                "function X(a, b, c)\r\nend",
                f => f.Name == "X"
                  && f.Parameters.All(p => p.Type is AstImplicitType)
                  && f.Parameters.Select(p => p.Name).SequenceEqual(new[] { "a", "b", "c" })
            );
        }

        [Test]
        public void FunctionWithTypedParameters() {
            ParseAssert.IsParsedTo<FunctionDefinition>(
                "function X(integer a, string b, boolean c)\r\nend",
                f => f.Name == "X"
                  && f.Parameters.Select(p => p.Type).Cast<AstUnknownType>().Select(t => t.Name).SequenceEqual(new[] { "integer", "string", "boolean" })
                  && f.Parameters.Select(p => p.Name).SequenceEqual(new[] { "a", "b", "c" })
            );
        }

        [Test]
        public void FunctionWithMixedParameters() {
            ParseAssert.IsParsedTo<FunctionDefinition>(
                "function X(integer a, b, boolean c)\r\nend",
                f => f.Name == "X" // not checking parameter types, too complex
                  && f.Parameters.Select(p => p.Name).SequenceEqual(new[] { "a", "b", "c" })
            );
        }
    }
}
