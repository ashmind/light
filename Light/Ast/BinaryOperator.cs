namespace Light.Ast {
    public class BinaryOperator {
        public string Symbol { get; private set; }

        public BinaryOperator(string symbol) {
            Symbol = symbol;
        }
    }
}