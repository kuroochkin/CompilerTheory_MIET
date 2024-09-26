// namespace LR2;
//
// public static class InputAnalyzer
// {
//     public static bool Execute(FiniteStateMachine fsm, string input)
//     {
//         var currentStates = new HashSet<string> { "q0" }; // Начальное состояние
//
//         foreach (var symbol in input)
//         {
//             var nextStates = new HashSet<string>();
//
//             foreach (var state in currentStates)
//             {
//                 var transitionsFromState = fsm.GetTransitions()
//                     .Where(t => t.FromState == state && t.Symbol == symbol)
//                     .Select(t => t.ToState);
//
//                 nextStates.UnionWith(transitionsFromState);
//             }
//
//             currentStates = nextStates;
//         }
//
//         return currentStates.Any(state => fsm.GetFinalStates().Contains(state));
//     }
// }