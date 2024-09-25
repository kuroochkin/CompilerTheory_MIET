namespace LR1;

public class Lexer
{
    private string _input;
    private int _pos;
    private char _currentChar;

    public Lexer(string input)
    {
        _input = input.Replace(" ", "");
        _pos = 0;
        _currentChar = _input[_pos];
    }

    private void Advance()
    {
        _pos++;
        _currentChar = _pos < _input.Length ? _input[_pos] : '\0';
    }

    private bool IsDigit(char c) => char.IsDigit(c) || c == '.';

    public List<Token> Tokenize()
{
    var tokens = new List<Token>();
    TokenType? lastTokenType = null; // Переменная для отслеживания типа последнего токена

    while (_currentChar != '\0')
    {
        if (char.IsWhiteSpace(_currentChar))
        {
            Advance(); // Игнорируем пробелы
            continue;
        }

        if (!IsDigit(_currentChar))
        {
            switch (_currentChar)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    // Проверка на несколько подряд идущих операторов
                    if (lastTokenType == TokenType.Plus || lastTokenType == TokenType.Minus || 
                        lastTokenType == TokenType.Multiply || lastTokenType == TokenType.Divide)
                    {
                        throw new Exception("Нельзя использовать несколько знаков операции подряд.");
                    }
                    
                    // Добавляем токен в зависимости от оператора
                    if (_currentChar == '+')
                        tokens.Add(new Token(TokenType.Plus));
                    else if (_currentChar == '-')
                        tokens.Add(new Token(TokenType.Minus));
                    else if (_currentChar == '*')
                        tokens.Add(new Token(TokenType.Multiply));
                    else if (_currentChar == '/')
                        tokens.Add(new Token(TokenType.Divide));

                    lastTokenType = _currentChar switch
                    {
                        '+' => TokenType.Plus,
                        '-' => TokenType.Minus,
                        '*' => TokenType.Multiply,
                        '/' => TokenType.Divide,
                        _ => lastTokenType
                    };

                    Advance();
                    break;

                case '(':
                    tokens.Add(new Token(TokenType.OpenParen));
                    lastTokenType = null; // Скобки не влияют на предыдущий токен
                    Advance();
                    break;

                case ')':
                    tokens.Add(new Token(TokenType.CloseParen));
                    lastTokenType = TokenType.CloseParen;
                    Advance();
                    break;

                case ',':
                    tokens.Add(new Token(TokenType.Comma));
                    lastTokenType = TokenType.Comma;
                    Advance();
                    break;

                default:
                    if (char.IsLetter(_currentChar))
                    {
                        // Бросаем исключение, если после числа идет некорректный символ
                        if (lastTokenType == TokenType.Number)
                            throw new Exception($"Некорректный символ");
                        
                        tokens.Add(new Token(TokenType.Function, GetFunctionName()));
                        lastTokenType = TokenType.Function;
                    }
                    else
                    {
                        throw new Exception($"Неизвестный символ: {_currentChar}");
                    }
                    break;
            }
        }
        else
        {
            // Проверка: если перед числом был другой токен Number, выбрасываем исключение
            if (lastTokenType == TokenType.Number)
            {
                throw new Exception("Ошибка: два числа подряд без оператора.");
            }

            tokens.Add(new Token(TokenType.Number, GetNumber()));
            lastTokenType = TokenType.Number; // Запоминаем, что последний токен был числом
        }
    }

    tokens.Add(new Token(TokenType.End));
    return tokens;
}

    private string GetNumber()
    {
        var number = "";
        while (IsDigit(_currentChar))
        {
            number += _currentChar;
            Advance();
        }
        return number;
    }

    private string GetFunctionName()
    {
        var functionName = "";
        while (char.IsLetter(_currentChar))
        {
            functionName += _currentChar;
            Advance();
        }
        return functionName;
    }
}