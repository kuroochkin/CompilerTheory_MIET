using System;
using System.IO;

namespace LR3
{
    public class GrammarReader
    {
        public static Grammar ReadGrammar(string filePath)
        {
            Grammar grammar = new Grammar();
            string[] lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Разделяем по первому '>'
                int firstGreaterThan = line.IndexOf('>');
                if (firstGreaterThan == -1)
                    throw new Exception($"Некорректная строка грамматики (нет '>'): {line}");

                string leftPart = line.Substring(0, firstGreaterThan).Trim();
                string rightPart = line.Substring(firstGreaterThan + 1).Trim();

                if (leftPart.Length != 1 || !char.IsUpper(leftPart[0]))
                    throw new Exception($"Некорректный нетерминал слева от '>': {leftPart}");

                char left = leftPart[0];

                // Разделяем правые части по '|'
                var productions = rightPart.Split('|');
                foreach (var prod in productions)
                {
                    string production = prod.Trim();
                    
                    // Убедитесь, что правые части могут содержать буквы и цифры
                    foreach (char c in production)
                    {
                        if (!char.IsUpper(c) && !char.IsLower(c) && !char.IsDigit(c))
                            throw new Exception($"Некорректный символ в правой части: {c}");
                    }

                    // Учитываем, что символы '>' после первого трактуются как терминальные
                    // Все символы после первого '>' уже включены в правую часть
                    grammar.AddRule(left, production);
                }
            }

            // Предполагаем, что стартовым символом является первый нетерминал
            if (lines.Length > 0)
            {
                string firstLine = lines[0].Trim();
                int firstGT = firstLine.IndexOf('>');
                if (firstGT > 0)
                {
                    char startSymbol = firstLine[0];
                    grammar.SetStartSymbol(startSymbol);
                }
                else
                {
                    throw new Exception("Не удалось определить стартовый символ грамматики.");
                }
            }
            else
            {
                throw new Exception("Файл грамматики пуст.");
            }

            return grammar;
        }
    }
}