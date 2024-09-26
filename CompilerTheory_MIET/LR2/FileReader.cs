namespace LR2;

public static class FileReader
{
    public static FiniteStateMachine GetMachineFromFile(string filePath)
    {
        var fsm = new FiniteStateMachine();

        using var reader = new StreamReader(filePath);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(new[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3) 
                continue; // Игнорировать неверные строки

            string fromState = parts[0].Trim();
            char symbol = parts[1][0];
            string toState = parts[2].Trim();

            fsm.AddTransition(fromState, symbol, toState);
            if (toState.StartsWith("f")) // Проверка, является ли состояние конечным
            {
                fsm.AddFinalState(toState);
            }
        }

        return fsm;
    }
}