using LR1;

while (true)
{
    Console.WriteLine("Введите арифметическое выражение (или введите 'exit' для завершения):");
    var input = Console.ReadLine();

    if (input?.Trim().ToLower() == "exit")
    {
        Console.WriteLine("Выход из программы.");
        break;
    }
    
    try
    {
        if (input != null)
        {
            var result = Calculator.Calculate(input);
            Console.WriteLine($"Результат: {result}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

    Console.WriteLine();
}