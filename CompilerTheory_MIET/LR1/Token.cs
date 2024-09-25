namespace LR1;

/// <summary>
/// Тип лексеммы
/// </summary>
public enum TokenType
{
    Number,
    Plus,
    Minus,
    Multiply,
    Divide,
    OpenParen,
    CloseParen,
    Function,
    Comma,
    End
}

/// <summary>
/// Лексемма
/// </summary>
public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }
}