// namespace LR2;
//
// public static class Determinizer
// {
//     public static FiniteStateMachine Execute(FiniteStateMachine fsm)
//     {
//         var newMachine = new FiniteStateMachine();
//         var stateMap = new Dictionary<string, string>();
//         var statesToProcess = new Queue<string>();
//         var initialState = "q0"; // Начальное состояние (измените при необходимости)
//
//         statesToProcess.Enqueue(initialState);
//         stateMap[initialState] = initialState;
//
//         while (statesToProcess.Count > 0)
//         {
//             var currentState = statesToProcess.Dequeue();
//             var transitionsFromCurrent = fsm.GetTransitions()
//                 .Where(t => t.FromState == currentState)
//                 .ToList();
//
//             foreach (var transitionGroup in transitionsFromCurrent.GroupBy(t => t.Symbol))
//             {
//                 var newStateName = $"{currentState}->{transitionGroup.Key}";
//                 var toStates = transitionGroup.Select(t => t.ToState).Distinct();
//
//                 if (!stateMap.ContainsKey(newStateName))
//                 {
//                     stateMap[newStateName] = newStateName;
//                     statesToProcess.Enqueue(newStateName);
//                 }
//
//                 foreach (var toState in toStates)
//                 {
//                     newMachine.AddTransition(stateMap[currentState], transitionGroup.Key, stateMap[newStateName]);
//
//                     if (fsm.GetFinalStates().Contains(toState))
//                     {
//                         newMachine.AddFinalState(newStateName);
//                     }
//                 }
//             }
//         }
//
//         return newMachine;
//     }
// }