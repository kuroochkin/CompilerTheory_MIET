namespace LR2;

public class FiniteStateMachine
{
    private readonly List<Transition> _transitions = new List<Transition>();
    private readonly HashSet<string> _finalStates = new HashSet<string>();
    private readonly HashSet<string> _allStates = new HashSet<string>();

    public void AddTransition(string fromState, char symbol, string toState)
    {
        _transitions.Add(new Transition(fromState, symbol, toState));
        _allStates.Add(fromState);
        _allStates.Add(toState);
    }

    public void AddFinalState(string state)
    {
        _finalStates.Add(state);
    }

    public bool IsDeterministic()
    {
        var groupedTransitions = _transitions
            .GroupBy(t => new { t.FromState, t.Symbol })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        return !groupedTransitions.Any();
    }

    public FiniteStateMachine Determinize()
    {
        var newMachine = new FiniteStateMachine();
        var stateMap = new Dictionary<string, HashSet<string>>(); // Карта для объединённых состояний
        var statesToProcess = new Queue<HashSet<string>>();

        // Начальное состояние (множество состояний из НКА)
        var initialState = new HashSet<string> { _allStates.First() }; 
        statesToProcess.Enqueue(initialState);
        stateMap[string.Join(",", initialState)] = initialState;

        while (statesToProcess.Count > 0)
        {
            var currentStateSet = statesToProcess.Dequeue();
            var currentStateName = string.Join(",", currentStateSet);

            // Для каждого символа определить новые состояния
            var transitionsBySymbol = _transitions
                .Where(t => currentStateSet.Contains(t.FromState))
                .GroupBy(t => t.Symbol);

            foreach (var transitionGroup in transitionsBySymbol)
            {
                var newStateSet = new HashSet<string>();
                
                foreach (var transition in transitionGroup)
                {
                    newStateSet.Add(transition.ToState);
                }

                var newStateName = string.Join(",", newStateSet);

                if (!stateMap.ContainsKey(newStateName))
                {
                    stateMap[newStateName] = newStateSet;
                    statesToProcess.Enqueue(newStateSet);
                }

                newMachine.AddTransition(currentStateName, transitionGroup.Key, newStateName);

                // Если новое состояние содержит конечные состояния НКА, оно также будет конечным
                if (_finalStates.Any(finalState => newStateSet.Contains(finalState)))
                {
                    newMachine.AddFinalState(newStateName);
                }
            }
        }

        return newMachine;
    }

    public void PrintTransitions()
    {
        Console.WriteLine("Таблица переходов:");
        foreach (var transition in _transitions)
        {
            Console.WriteLine($"{transition.FromState} -- {transition.Symbol} --> {transition.ToState}");
        }
    }

    public bool AnalyzeInput(string input)
    {
        var currentStates = new HashSet<string> { _allStates.First() };

        foreach (var symbol in input)
        {
            var nextStates = new HashSet<string>();

            foreach (var state in currentStates)
            {
                var transitionsFromState = _transitions
                    .Where(t => t.FromState == state && t.Symbol == symbol)
                    .Select(t => t.ToState);

                nextStates.UnionWith(transitionsFromState);
            }

            currentStates = nextStates;
        }

        return currentStates.Any(state => _finalStates.Contains(state));
    }
}
