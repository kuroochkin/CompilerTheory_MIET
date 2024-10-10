using System.Collections.Generic;

namespace LR3
{
    public class StringValidator
    {
        private readonly Pda _pda;

        public StringValidator(Pda pda)
        {
            _pda = pda;
        }

        public (bool isValid, List<Configuration> configChain) IsStringValid(string input)
        {
            return _pda.ValidateString(input);
        }
    }
}