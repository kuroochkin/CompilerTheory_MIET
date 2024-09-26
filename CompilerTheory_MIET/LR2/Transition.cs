namespace LR2;

public class Transition
{
    public string FromState { get; set; } // Текущее состояние
    public char Symbol { get; set; }      // Символ с ленты
    public string ToState { get; set; }   // Состояние, в которое переходим

    public Transition(string fromState, char symbol, string toState)
    {
        FromState = fromState;
        Symbol = symbol;
        ToState = toState;
    }
}