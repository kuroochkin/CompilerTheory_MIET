using LR4;

var path = "D:\\proga\\CompilerTheory_MIET\\CompilerTheory_MIET\\LR4\\program.txt";

var lines = File.ReadAllLines(path);

var pushdownAutomaton = new PushdownAutomaton();

foreach (var item in lines)
{
    TransitionFunction.TryParse(item, out var result);
    pushdownAutomaton.AddRangeTransitionFunction(result);
}

foreach (var item in pushdownAutomaton.Sigma)
    Console.WriteLine(item);

var code = File.ReadAllText("D:\\proga\\CompilerTheory_MIET\\CompilerTheory_MIET\\LR4\\code.txt");

SyntaxValidator.Validate(code);