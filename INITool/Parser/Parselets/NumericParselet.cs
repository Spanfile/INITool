using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal class NumericParselet : Parselet, IPrefixParselet
    {
        public NumericParselet(Parser parser, Tokeniser.Tokeniser tokeniser) : base(parser, tokeniser)
        {
        }

        public Unit Parse(Token token)
        {
            var tokens = new List<Token> {token};

            IEnumerable<Token> newTokens;
            object value;
            // if the first token is a zero, depending on the next one this might either be a hex or a binary literal
            if (token.Value == "0")
            {
                var next = Tokeniser.Peek().Value;
                if (next == "x")
                {
                    tokens.Add(Tokeniser.Take());
                    (newTokens, value) = ParseHex();
                    return new ValueUnit(value, tokens.Concat(newTokens));
                }
                if (next == "b")
                {
                    tokens.Add(Tokeniser.Take());
                    (newTokens, value) = ParseBinary();
                    return new ValueUnit(value, tokens.Concat(newTokens));
                }
            }

            if (token.TokenType == TokenType.Ampersand)
            {
                if (Tokeniser.Peek().Value != "H")
                    throw new InvalidLiteralException();

                tokens.Add(Tokeniser.Take());
                (newTokens, value) = ParseHex();
                return new ValueUnit(value, tokens.Concat(newTokens));
            }

            (newTokens, value) = ParseNumeric(token.Value);
            return new ValueUnit(value, tokens.Concat(newTokens));
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private (IEnumerable<Token> tokens, object value) ParseNumeric(string firstValue)
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder(firstValue);
            var isDouble = false;
            object value;

            // this doesn't use Tokeniser.TakeSequentialOfType because the criteria may change mid-iteration
            var allowedTokens = new[] { TokenType.Digit, TokenType.Period };
            while (allowedTokens.Contains(Tokeniser.Peek().TokenType))
            {
                var next = Tokeniser.Take();
                tokens.Add(next);
                valueBuilder.Append(next.Value);

                if (next.TokenType != TokenType.Period)
                    continue;

                // value is double: disallow periods
                allowedTokens = new[] { TokenType.Digit };
                isDouble = true;
            }

            var valueStr = valueBuilder.ToString();

            // the ternary expression messes up the typing and causes invalid casting
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (isDouble)
                value = double.Parse(valueStr);
            else
                value = long.Parse(valueStr);

            return (tokens.AsEnumerable(), value);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private (IEnumerable<Token> tokens, ulong value) ParseHex()
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder();

            var allowedTokens = new[] { TokenType.Digit, TokenType.Letter };
            var allowedLetters = "abcdef".ToCharArray();
            while (allowedTokens.Contains(Tokeniser.Peek().TokenType))
            {
                var next = Tokeniser.Take();

                if (next.TokenType == TokenType.Letter)
                {
                    if (!allowedLetters.Contains(next.Value[0]))
                        throw new InvalidLiteralException();
                }

                tokens.Add(next);
                valueBuilder.Append(next.Value);
            }

            var valueStr = valueBuilder.ToString();

            if (!ulong.TryParse(valueStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong value))
                throw new InvalidLiteralException();

            return (tokens.AsEnumerable(), value);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private (IEnumerable<Token> tokens, ulong value) ParseBinary()
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder();

            while (Tokeniser.Peek().TokenType == TokenType.Digit)
            {
                var next = Tokeniser.Take();

                if (next.Value != "0" && next.Value != "1")
                    throw new InvalidLiteralException();

                tokens.Add(next);
                valueBuilder.Append(next.Value);
            }

            var valueStr = valueBuilder.ToString();
            ulong value;

            try
            {
                value = Convert.ToUInt64(valueStr, 2);
            }
            catch (OverflowException e)
            {
                throw new InvalidLiteralException(e);
            }
            catch (FormatException e)
            {
                throw new InvalidLiteralException(e);
            }

            return (tokens.AsEnumerable(), value);
        }
    }
}
