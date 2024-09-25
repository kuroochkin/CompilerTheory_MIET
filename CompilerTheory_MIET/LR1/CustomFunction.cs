namespace LR1;

public static class CustomFunction
{
    public static double Calculate(string functionName, double arg1, double arg2)
    {
        switch (functionName)
        {
            case "log" when arg1 <= 0 || arg2 <= 0:
                throw new Exception("Невозможный логарифм");
            case "log":
                return Math.Log(arg1, arg2);
            case "pow":
                return Math.Pow(arg1, arg2);
            default:
                throw new Exception($"Неизвестная функция: {functionName}");
        }
    }
}