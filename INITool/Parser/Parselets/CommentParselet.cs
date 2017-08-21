using System;
using System.Collections.Generic;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class CommentParselet : Parselet, IPrefixParselet
    {
        public CommentParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
        }

        public Unit Parse(Token token)
        {
            var commentBuilder = new StringBuilder();

            while (true)
            {
                while (Tokeniser.Peek().TokenType != TokenType.Newline)
                    commentBuilder.Append(Tokeniser.Take().Value);

                // take the trailing newline
                Tokeniser.Take();

                var temp = Tokeniser.Peek();
                if (temp.TokenType == TokenType.Semicolon || temp.TokenType == TokenType.Hash)
                {
                    // take the comment mark
                    Tokeniser.Take();
                    commentBuilder.Append("\n");
                    continue;
                }

                break;
            }

            var unit = Parser.ParseUnit();
            unit.Comment = commentBuilder.ToString();
            return unit;
        }
    }
}
