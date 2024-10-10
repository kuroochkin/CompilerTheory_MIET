namespace LR3
{
    public class Transition
    {
        public string CurrentState { get; set; }
        public char InputSymbol { get; set; } // '\0' для ε-переходов
        public char StackTop { get; set; }
        public string NextState { get; set; }
        public string StackPush { get; set; } // Может быть пустой строкой для pop

        public Transition(string currentState, char inputSymbol, char stackTop, string nextState, string stackPush)
        {
            CurrentState = currentState;
            InputSymbol = inputSymbol;
            StackTop = stackTop;
            NextState = nextState;
            StackPush = stackPush;
        }

        public override string ToString()
        {
            string input = InputSymbol == '\0' ? "lambda" : InputSymbol.ToString();
            string stackPop = StackTop == '\0' ? "lambda" : StackTop.ToString();
            string stackPushStr = string.IsNullOrEmpty(StackPush) ? "lambda" : StackPush;
            return $"б({CurrentState}, {input}, {stackPop}) = ({NextState}, {stackPushStr})";
        }
    }
}