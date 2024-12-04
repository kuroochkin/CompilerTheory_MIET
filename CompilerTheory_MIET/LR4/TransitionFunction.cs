namespace LR4
{
    internal class TransitionFunction
    {
        public State CurrentState { get; }
        public State NextState { get; }
        public string Symbol { get; }
        public string SymbolFromH { get; }
        public List<string> Action { get; set; }

        public TransitionFunction(State currentState, State nextState, string symbol, string symbolFromH, List<string> action)
        {
            CurrentState = currentState;
            NextState = nextState;
            Symbol = symbol;
            SymbolFromH = symbolFromH;
            Action = action;
        }

        public override string ToString()
        {
            string action = "";
            foreach (var item in Action)
            {
                action += item;
            }
            string str = "(" + CurrentState.Name + ", " + Symbol + ", " + SymbolFromH + ") = (" + NextState.Name + ", " + action + ")";
            return str;
        }

        public static bool TryParse(string inputStr, out HashSet<TransitionFunction> transitionFunction)
        {
            var startState = new State("s0");
            var emptyAction = new List<string>();
            emptyAction.Add("`");

            transitionFunction = null;
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            var splitString = inputStr.Split(": ", StringSplitOptions.None);
            if (splitString.Length != 2)
            {
                return false;
            }

            string symbolFromH = splitString[0].Trim(new char[] { '<', '>' });
            var rules = splitString[1].Split(" | ");

            transitionFunction = new HashSet<TransitionFunction>();
            for (int i = 0; i < rules.Length; i++)
            {
                var actions = new List<string>();
                for (int j = 0; j < rules[i].Length; j++)
                {
                    if (rules[i][j] == '<')
                    {
                        int startIndex = j + 1;
                        while (rules[i][j] != '>')
                        {
                            j++;
                        }
                        actions.Insert(0,rules[i][startIndex..j]);
                        continue;
                    }
                    if(rules[i][j] == '‘')
                    {
                        int startIndex = j + 1;
                        while (rules[i][j] != '’')
                        {
                            j++;
                        }
                        var action = rules[i][startIndex..j];
                        actions.Insert(0,action);

                        TransitionFunction func2 = new TransitionFunction(startState, startState, action, action, emptyAction);
                        transitionFunction.Add(func2);
                        continue;
                    }
                    actions.Insert(0,rules[i][j].ToString());
                }
                if (rules[i].Length == 0)
                {
                    actions.Insert(0, "~");
                }
                TransitionFunction func = new TransitionFunction(startState, startState, "`", symbolFromH, actions);
                transitionFunction.Add(func);
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrentState, NextState, Symbol, SymbolFromH, Action);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TransitionFunction))
            {
                return false;
            }
            return object.Equals(CurrentState, (obj as TransitionFunction).CurrentState) && object.Equals(NextState, (obj as TransitionFunction).NextState) &&
                Action == (obj as TransitionFunction).Action && Symbol == (obj as TransitionFunction).Symbol &&
                SymbolFromH == (obj as TransitionFunction).SymbolFromH;
        }
    }
}