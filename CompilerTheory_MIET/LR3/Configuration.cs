using System.Collections.Generic;

namespace LR3
{
    public class Configuration
    {
        /// <summary>
        /// Текущее состояние
        /// </summary>
        public string CurrentState { get; set; }
        
        /// <summary>
        /// Непрочитанные символы
        /// </summary>
        public Stack<char> Stack { get; set; }
        
        /// <summary>
        /// Магазин
        /// </summary>
        public string RemainingInput { get; set; }

        public Configuration(string state, Stack<char> stack, string input)
        {
            CurrentState = state;
            // Создаем копию стека
            Stack = new Stack<char>(new Stack<char>(stack));
            RemainingInput = input;
        }

        public override string ToString()
        {
            var stackContent = Stack.Count > 0 ? string.Concat(Stack.ToArray()) : "lambda";
            var inputContent = string.IsNullOrEmpty(RemainingInput) ? "lambda" : RemainingInput;
            return $"({CurrentState}, {inputContent}, {stackContent})";
        }
    }
}