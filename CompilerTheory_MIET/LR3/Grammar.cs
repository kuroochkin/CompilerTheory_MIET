using System.Collections.Generic;

namespace LR3
{
    public class Grammar
    {
        public char StartSymbol { get; private set; }
        public Dictionary<char, List<string>> Rules { get; } 
            = new Dictionary<char, List<string>>();

        public void AddRule(char left, string right)
        {
            if (!Rules.ContainsKey(left))
                Rules[left] = new List<string>();
            
            Rules[left].Add(right);
        }
        
        public void SetStartSymbol(char start) =>
            StartSymbol = start;

        public HashSet<char> GetNonTerminals() 
            => new HashSet<char>(Rules.Keys);
        
        public HashSet<char> GetTerminals()
        {
            var nonTerminals = GetNonTerminals();
            var terminals = new HashSet<char>();

            foreach (var rule in Rules.Values)
            {
                foreach (var prod in rule)
                {
                    foreach (var c in prod)
                    {
                        if (!nonTerminals.Contains(c) && !char.IsUpper(c))
                            terminals.Add(c);
                    }
                }
            }

            return terminals;
        }
    }
}