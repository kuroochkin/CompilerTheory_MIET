using System.Globalization;

namespace LR1;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _position;
    private Token _currentToken;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;
        _currentToken = _tokens[_position];
    }

    private void Advance()
    {
        _position++;
        _currentToken = _tokens[_position];
    }

    public double ParseExpression()
    {
        ValidateParentheses();
        return ParseAddSubtract();
    }

    private double ParseAddSubtract()
    {
        var value = ParseMultiplyDivide();

        while (_currentToken.Type == TokenType.Plus || _currentToken.Type == TokenType.Minus)
        {
            switch (_currentToken.Type)
            {
                case TokenType.Plus:
                    Advance();
                    value += ParseMultiplyDivide();
                    break;
                case TokenType.Minus:
                    Advance();
                    value -= ParseMultiplyDivide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return value;
    }

    private double ParseMultiplyDivide()
    {
        var value = ParseFactor();

        while (_currentToken.Type == TokenType.Multiply || _currentToken.Type == TokenType.Divide)
        {
            switch (_currentToken.Type)
            {
                case TokenType.Multiply:
                    Advance();
                    value *= ParseFactor();
                    break;
                
                case TokenType.Divide:
                {
                    Advance();
                    var divisor = ParseFactor();
                    
                    if (divisor == 0)
                        throw new DivideByZeroException("Деление на ноль");
                    
                    value /= divisor;
                    break;
                }
            }
        }

        return value;
    }

    private double ParseFactor()
    {
        switch (_currentToken.Type)
        {
            case TokenType.Number:
            {
                var number = double.Parse(_currentToken.Value, CultureInfo.InvariantCulture);
                Advance();
                return number;
            }
            case TokenType.OpenParen:
            {
                Advance();
                var value = ParseExpression();
                Match(TokenType.CloseParen);
                return value;
            }
            case TokenType.Minus:
            {
                Advance(); // Пропускаем унарный минус
                var value = ParseFactor(); // Рекурсивно вызываем ParseFactor для получения значения
                return -value; // Возвращаем отрицательное значение
            }
            case TokenType.Function:
            {
                var functionName = _currentToken.Value;
                Advance();
                Match(TokenType.OpenParen);
                var arg1 = ParseExpression();
                Match(TokenType.Comma);
                var arg2 = ParseExpression();
                Match(TokenType.CloseParen);
                return CustomFunction.Calculate(functionName, arg1, arg2);
            }
            default:
                throw new Exception("Некорректный фактор");
        }
    }

    private void ValidateParentheses()
    {
        var openCount = 0;
        
        foreach (var token in _tokens)
        {
            switch (token.Type)
            {
                case TokenType.OpenParen:
                    openCount++;
                    break;
                case TokenType.CloseParen:
                    openCount--;
                    break;
            }
        }
        
        if (openCount != 0)
            throw new Exception("Несоответствие количества открывающих и закрывающих скобок");
    }
    
    private void Match(TokenType type)
    {
        if (_currentToken.Type == type)
        {
            Advance();
            return;
        }

        throw new Exception($"Ожидался токен {type}, но был {_currentToken.Type}");
    }
}