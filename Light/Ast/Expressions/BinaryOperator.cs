using System;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class BinaryOperator {
        public string Symbol { get; private set; }

        public IAstTypeReference ResultType {
            get { throw new NotImplementedException("BinaryOperator.ResultType"); }
        }

        public BinaryOperator(string symbol) {
            Symbol = symbol;
        }
    }
}