namespace INITool.Parser.Parselets
{
    internal abstract class Parselet
    {
        protected Parser Parser;
        protected Tokeniser.Tokeniser Tokeniser;
        protected IniOptions Options;

        protected Parselet(Parser parser, Tokeniser.Tokeniser tokeniser, IniOptions options)
        {
            Parser = parser;
            Tokeniser = tokeniser;
            Options = options;
        }
    }
}
