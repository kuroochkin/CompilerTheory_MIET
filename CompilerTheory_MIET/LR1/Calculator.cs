namespace LR1;

public static class Calculator
{
    public static double Calculate(string input)
    {
        var lexer = new Lexer(input);
        var tokens = lexer.Tokenize();
        var parser = new Parser(tokens);
        return parser.ParseExpression();
    }
}