using System;

namespace LR3
{
    class Program
    {
        static void Main(string[] args)
        {
            string grammarFilePath = "D:\\proga\\CompilerTheory_MIET\\CompilerTheory_MIET\\LR3\\test2.txt";
            Grammar grammar = null;

            try
            {
                grammar = GrammarReader.ReadGrammar(grammarFilePath);
                Console.WriteLine("Грамматика успешно прочитана.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении грамматики: {ex.Message}");
                return;
            }

            // Создание PDA
            Pda pda = new Pda(grammar);

            // Вывод множеств S, Σ, Γ, Functions, Z0
            Console.WriteLine("Множества и компоненты PDA:");
            Console.WriteLine(pda.GetStates());
            Console.WriteLine(pda.GetInputAlphabet());
            Console.WriteLine(pda.GetStackAlphabet());
            Console.WriteLine(pda.GetInitialStackSymbol());
            Console.WriteLine(pda.GetTransitions());
            Console.WriteLine();

            StringValidator validator = new StringValidator(pda);

            Console.WriteLine("Введите строки для проверки (пустая строка для завершения):");
            while (true)
            {
                Console.Write("Строка: ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;

                var (isValid, configChain) = validator.IsStringValid(input);

                Console.WriteLine("\nЦепочка конфигураций PDA:");
                foreach (var config in configChain)
                {
                    Console.WriteLine(config.ToString());
                }

                Console.WriteLine($"\nЗаключение: Строка \"{input}\" {(isValid ? "допустима" : "недопустима")} автоматом.\n");
            }
        }
    }
}
