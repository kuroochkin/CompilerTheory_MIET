namespace LR2;

public static class FileReader
{
    public static FiniteStateMachine GetMachineFromFile(string filePath)
    {
        var fsm = new FiniteStateMachine();

        using var reader = new StreamReader(filePath);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(new[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3) 
                continue;

            var fromState = parts[0].Trim();
            var symbol = parts[1][0];
            var toState = parts[2].Trim();

            fsm.AddTransition(fromState, symbol, toState);
            
            if (toState.StartsWith("f"))
                fsm.AddFinalState(toState);
            
        }

        return fsm;
    }
}