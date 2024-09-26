namespace LR2;

/// <summary>
/// Класс перехода из одного состояние в другое
/// </summary>
public class Transition
{
    /// <summary>
    /// Текущее состояние
    /// </summary>
    public string FromState { get; set; } 
    
    /// <summary>
    /// Символ с ленты
    /// </summary>
    public char Symbol { get; set; }
    
    /// <summary>
    /// Состояние, в которое переходим
    /// </summary>
    public string ToState { get; set; }   

    public Transition(string fromState, char symbol, string toState)
    {
        FromState = fromState;
        Symbol = symbol;
        ToState = toState;
    }
}