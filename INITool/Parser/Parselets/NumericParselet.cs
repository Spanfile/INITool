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
                if (next.StartsWith("x"))
                {
                    token = Tokeniser.TakeOfType(TokenType.Word);
                    tokens.Add(token);
                    (newTokens, value) = ParseHex(token.Value);
                    return new ValueUnit(value, tokens.Concat(newTokens));
                }
                if (next.StartsWith("b"))
                {
                    tokens.Add(Tokeniser.TakeOfType(TokenType.Word));
                    (newTokens, value) = ParseBinary();
                    return new ValueUnit(value, tokens.Concat(newTokens));
                }
            }

            if (token.TokenType == TokenType.Ampersand)
            {
                var next = Tokeniser.Peek();
                if (!next.Value.StartsWith("H"))
                    throw new InvalidLiteralException(Tokeniser.CurrentPosition);

                token = Tokeniser.TakeOfType(TokenType.Word);
                tokens.Add(token);
                (newTokens, value) = ParseHex(token.Value);
                return new ValueUnit(value, tokens.Concat(newTokens));
            }

            (newTokens, value) = ParseNumeric(token.Value);
            return new ValueUnit(value, tokens.Concat(newTokens));
        }

        private (IEnumerable<Token> tokens, object value) ParseNumeric(string firstValue)
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder(firstValue);
            var isDouble = false;
            object value;

            if (firstValue == "-")
            {
                var numberToken = Tokeniser.TakeOfType(TokenType.Number);
                tokens.Add(numberToken);
                valueBuilder.Append(numberToken.Value);
            }

            if (Tokeniser.Peek().TokenType == TokenType.Period)
            {
                var periodToken = Tokeniser.Take();
                tokens.Add(periodToken);
                valueBuilder.Append(periodToken.Value);

                Token decimalToken;
                try
                {
                    decimalToken = Tokeniser.TakeOfType(TokenType.Number);
                }
                catch (InvalidTokenException e)
                {
                    throw new InvalidLiteralException(Tokeniser.CurrentPosition, e);
                }

                tokens.Add(decimalToken);
                valueBuilder.Append(decimalToken.Value);

                isDouble = true;
            }

            var valueStr = valueBuilder.ToString();

            // the ternary expression messes up the typing and causes invalid casting
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (isDouble)
            {
                if (!double.TryParse(valueStr, out var temp))
                    throw new InvalidLiteralException(Tokeniser.CurrentPosition);
                value = temp;
            }
            else
            {
                if (!long.TryParse(valueStr, out var temp))
                    throw new InvalidLiteralException(Tokeniser.CurrentPosition);
                value = temp;
            }

            return (tokens.AsEnumerable(), value);
        }

        private (IEnumerable<Token> tokens, ulong value) ParseHex(string firstValue)
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder();

            if (firstValue.Length > 1)
                valueBuilder.Append(firstValue.Substring(1, firstValue.Length - 1));

            var allowedLetters = "abcdef".ToCharArray();
            foreach (var token in Tokeniser.TakeSequentialOfType(TokenType.Word, TokenType.Number))
            {
                if (token.TokenType == TokenType.Word)
                {
                    if (token.Value.Any(c => !allowedLetters.Contains(c)))
                        throw new InvalidLiteralException(Tokeniser.CurrentPosition);
                }

                tokens.Add(token);
                valueBuilder.Append(token.Value);
            }

            var valueStr = valueBuilder.ToString();

            if (!ulong.TryParse(valueStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
                throw new InvalidLiteralException(Tokeniser.CurrentPosition);

            return (tokens.AsEnumerable(), value);
        }

        private (IEnumerable<Token> tokens, ulong value) ParseBinary()
        {
            var tokens = new List<Token>();
            var valueBuilder = new StringBuilder();

            var bin = Tokeniser.TakeOfType(TokenType.Number);
            if (bin.Value.Any(d => d != '0' && d != '1'))
                throw new InvalidLiteralException(Tokeniser.CurrentPosition);

            tokens.Add(bin);
            valueBuilder.Append(bin.Value);

            var valueStr = valueBuilder.ToString();
            ulong value;

            try
            {
                value = Convert.ToUInt64(valueStr, 2);
            }
            catch (OverflowException e)
            {
                throw new InvalidLiteralException(Tokeniser.CurrentPosition, e);
            }
            catch (FormatException e)
            {
                throw new InvalidLiteralException(Tokeniser.CurrentPosition, e);
            }

            return (tokens.AsEnumerable(), value);
        }
    }
}
