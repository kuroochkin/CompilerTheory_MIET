using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LR3
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Pda
    {
        /// <summary>
        /// Множество состояний
        /// </summary>
        private HashSet<string> _s { get; }
        
        /// <summary>
        /// Входной алфавит
        /// </summary>
        private HashSet<char> _p { get; } 
        
        /// <summary>
        /// Алфавит стека
        /// </summary>
        private HashSet<char> _z { get; } 
        
        /// <summary>
        /// Множество функций
        /// </summary>
        private List<Transition> _functions { get; set; }
        
        /// <summary>
        /// Вершина стека
        /// </summary>
        private string _z0 { get; } 

        /// <summary>
        /// Грамматика
        /// </summary>
        private readonly Grammar _grammar;
        
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Transition> transitions;
        
        private const string _startState = "q0";
        private const string _acceptState = "q1";

        public Pda(Grammar grammar)
        {
            _grammar = grammar;
            _s = new HashSet<string> { _startState, _acceptState };
            _p = grammar.GetTerminals();
            _z = new HashSet<char>(grammar.GetNonTerminals().Concat(_p));
            _z0 = grammar.StartSymbol.ToString();
            _functions = new List<Transition>();
            transitions = new List<Transition>();
            BuildTransitions();
        }

        private void BuildTransitions()
        {
            // Развертывание продукций грамматики (ε-переходы)
            foreach (var (nonTerminal, value) in _grammar.Rules)
            {
                foreach (var pushString in value.Select(production => new string(production.Reverse().ToArray())))
                {
                    transitions.Add(new Transition(
                        currentState: _startState,
                        inputSymbol: '\0', // ε-переход
                        stackTop: nonTerminal,
                        nextState: _startState,
                        stackPush: pushString
                    ));
                }
            }

            // Считывание терминалов
            foreach (var terminal in _p)
            {
                transitions.Add(new Transition(
                    currentState: _startState,
                    inputSymbol: terminal,
                    stackTop: terminal,
                    nextState: _startState,
                    stackPush: "" // Удаляем терминал из стека
                ));
            }

            // Переход в конечное состояние, если стек пуст и ввод прочитан
            transitions.Add(new Transition(
                currentState: _startState,
                inputSymbol: '\0',
                stackTop: '\0', // Специальный символ, обозначающий пустой стек
                nextState: _acceptState,
                stackPush: "" // Не меняем стек
            ));

            _functions = transitions;
        }

        /// <summary>
        /// Проверяет, допустима ли строка для данного PDA.
        /// Возвращает цепочку конфигураций и заключение о допустимости.
        /// </summary>
       public (bool isAccepted, List<Configuration> configChain) ValidateString(string input)
        {
            Stack<char> initialStack = new Stack<char>();
            initialStack.Push(_grammar.StartSymbol);
            Configuration initialConfig = new Configuration(_startState, initialStack, input);

            Queue<Configuration> queue = new Queue<Configuration>();
            queue.Enqueue(initialConfig);

            List<Configuration> configChain = new List<Configuration>();
            HashSet<string> visited = new HashSet<string>();

            while (queue.Count > 0)
            {
                var currentConfig = queue.Dequeue();
                configChain.Add(currentConfig);

                // Проверяем, достигли ли мы конечного состояния
                if (currentConfig.CurrentState == _acceptState &&
                    string.IsNullOrEmpty(currentConfig.RemainingInput) &&
                    currentConfig.Stack.Count == 0)
                {
                    return (true, configChain);
                }

                string configKey = $"{currentConfig.CurrentState}|{new string(currentConfig.Stack.Reverse().ToArray())}|{currentConfig.RemainingInput}";
                if (visited.Contains(configKey))
                    continue;
                visited.Add(configKey);

                char stackTop = currentConfig.Stack.Count > 0 ? currentConfig.Stack.Peek() : '\0';

                var applicableTransitions = transitions.Where(t =>
                    t.CurrentState == currentConfig.CurrentState &&
                    (t.StackTop == stackTop || (t.StackTop == '\0' && currentConfig.Stack.Count == 0))
                ).ToList();

                foreach (var transition in applicableTransitions)
                {
                    Configuration newConfig = new Configuration(
                        transition.NextState,
                        new Stack<char>(currentConfig.Stack.Reverse()), // Клонируем стек
                        currentConfig.RemainingInput
                    );

                    // Применяем переход
                    if (transition.StackTop != '\0' && newConfig.Stack.Count > 0)
                    {
                        newConfig.Stack.Pop(); // Удаляем символ со стека
                    }

                    if (!string.IsNullOrEmpty(transition.StackPush))
                    {
                        for (int i = transition.StackPush.Length - 1; i >= 0; i--)
                        {
                            newConfig.Stack.Push(transition.StackPush[i]); // Добавляем символы в стек
                        }
                    }

                    // Если переход читает символ, проверяем соответствие
                    if (transition.InputSymbol != '\0')
                    {
                        if (newConfig.RemainingInput.Length == 0 || newConfig.RemainingInput[0] != transition.InputSymbol)
                            continue; // Символ не совпадает
                        newConfig.RemainingInput = newConfig.RemainingInput.Substring(1); // Убираем считанный символ
                    }

                    queue.Enqueue(newConfig);
                }
            }

            return (false, configChain);
        }

        /// <summary>
        /// Возвращает строковое представление множества состояний _s.
        /// </summary>
        public string GetStates()
        {
            return "S = {" + string.Join(", ", _s) + "}";
        }

        /// <summary>
        /// Возвращает строковое представление входного алфавита P.
        /// </summary>
        public string GetInputAlphabet()
        {
            return "P = {" + string.Join(", ", _p) + "}";
        }

        /// <summary>
        /// Возвращает строковое представление алфавита стека _z.
        /// </summary>
        public string GetStackAlphabet()
        {
            return "Z = {" + string.Join(", ", _z) + "}";
        }

        /// <summary>
        /// Возвращает строковое представление множества переходов _functions.
        /// </summary>
        public string GetTransitions()
        {
            return "_functions = {\n\t" + string.Join("\n\t", _functions.Select(t => t.ToString())) + "\n}";
        }

        /// <summary>
        /// Возвращает строковое представление начального символа стека _z0.
        /// </summary>
        public string GetInitialStackSymbol()
        {
            return $"z0 = {_z0}";
        }
    }
}