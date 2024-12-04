using System;
using System.Collections.Generic;

public static class SyntaxValidator
{
    public static void Validate(string code)
    {
        Stack<char> bracketStack = new Stack<char>();
        List<string> errors = new List<string>();

        int line = 1; // Нумерация строк для вывода ошибок
        int column = 1; // Нумерация столбцов для вывода ошибок

        bool inString = false; // флаг для отслеживания строк
        bool inChar = false; // флаг для отслеживания символьных литералов

        for (int i = 0; i < code.Length; i++)
        {
            char currentChar = code[i];

            // Пропускаем символы конца строки
            if (currentChar == '\n')
            {
                line++;
                column = 1;
                continue;
            }

            if (inString)
            {
                if (currentChar == '"') inString = false; // Закрываем строковый литерал
                column++;
                continue;
            }
            if (inChar)
            {
                if (currentChar == '\'') inChar = false; // Закрываем символьный литерал
                column++;
                continue;
            }

            // Обрабатываем строковые и символьные литералы
            if (currentChar == '"')
            {
                inString = true;
            }
            else if (currentChar == '\'')
            {
                inChar = true;
            }

            // Обрабатываем открывающие и закрывающие скобки
            if (currentChar == '{' || currentChar == '[' || currentChar == '(')
            {
                bracketStack.Push(currentChar);
            }
            else if (currentChar == '}' || currentChar == ']' || currentChar == ')')
            {
                if (bracketStack.Count == 0)
                {
                    errors.Add($"Error! Index: {i}. Symbol: '{currentChar}'.");
                }
                else
                {
                    char lastBracket = bracketStack.Pop();
                    if (!IsMatchingBracket(lastBracket, currentChar))
                    {
                        errors.Add($"Error! Index: {i}. Symbol: '{currentChar}'.");
                    }
                }
            }

            // Проверка на некорректное использование символов в выражениях
            if (currentChar == '/' && i + 1 < code.Length && code[i + 1] == '/')
            {
                errors.Add($"Error! Index: {i}. Symbol: '{currentChar}{code[i + 1]}'.");
            }

            if (currentChar == ';' && i > 0 && code[i - 1] == '/')
            {
                errors.Add($"Error! Index: {i}. Symbol: '{code[i - 1]}{currentChar}'.");
            }

            column++;
        }

        // Проверка на несоответствие количества открывающих и закрывающих скобок
        if (bracketStack.Count > 0)
        {
            errors.Add($"Error! Index: {code.Length}. Symbol: '{bracketStack.Peek()}'.");
        }

        // Вывод ошибок, если они есть
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine($"Number of errors: {errors.Count}");
        }
        else
        {
            Console.WriteLine("Number of errors: 0");
        }
    }

    private static bool IsMatchingBracket(char openBracket, char closeBracket)
    {
        return (openBracket == '{' && closeBracket == '}') ||
               (openBracket == '[' && closeBracket == ']') ||
               (openBracket == '(' && closeBracket == ')');
    }
}