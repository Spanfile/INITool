using System;
using System.Collections.Generic;
using System.Text;

namespace INITool.Parser.Parselets
{
    internal abstract class Parselet
    {
        protected Parser Parser;
        protected Tokeniser.Tokeniser Tokeniser;

        protected Parselet(Parser parser, Tokeniser.Tokeniser tokeniser)
        {
            Parser = parser;
            Tokeniser = tokeniser;
        }
    }
}
