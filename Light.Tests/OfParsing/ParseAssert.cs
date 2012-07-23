using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Light.Ast;
using Light.BuiltIn;
using Light.Description;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    public static class ParseAssert {
        public static void IsParsedTo(string code, string expectedResult, bool includeExpressionType = false) {
            var parser = new LightParser();
            var result = parser.Parse(code);
            AssertEx.That(() => !result.HasErrors);

            var resultString = new AstToCodeTransformer().Transform(result.Root);
            var resultExpression = result.Root as IAstExpression;
            if (resultExpression != null && includeExpressionType)
                resultString = resultString + ": " + new TypeFormatter(new BuiltInTypeMap()).Format(resultExpression.ExpressionType);
            
            Assert.AreEqual(expectedResult, resultString);
        }

        public static void IsParsedTo<TAstElement>(string code, Expression<Func<TAstElement, bool>> condition) 
            where TAstElement : class, IAstElement
        {
            IsParsedTo(code, root => root.Child<TAstElement>() ?? root as TAstElement, condition);
        }

        public static void IsParsedTo<TAstElement>(string code, Func<IAstElement, TAstElement> getElementFromResult, Expression<Func<TAstElement, bool>> condition) {
            var result = new LightParser().Parse(code);
            AssertEx.That(() => !result.HasErrors);

            var element = getElementFromResult(result.Root);
            Assert.IsNotNull(element);
            AssertEx.That(Expression.Lambda<Func<bool>>(Expression.Invoke(condition, Expression.Constant(element))));
        }
    }
}
