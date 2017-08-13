using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace INITool.Parser.Tokeniser
{
    internal class Tokeniser : IDisposable
    {
        public bool EndOfInput { get; private set; }

        private readonly Dictionary<Regex, TokenType> tokenDictionary;

        private readonly TextReader textReader;
        private readonly List<Token> tokens;
        private int iterIndex;

        public Tokeniser(TextReader textReader)
        {
            this.textReader = textReader;

            tokens = new List<Token>();
            iterIndex = 0;
            EndOfInput = false;

            tokenDictionary = new Dictionary<Regex, TokenType>();
            PopulateTokenDictionary(new Dictionary<string, TokenType>
            {
                { @"[ \t\f\v]", TokenType.Whitespace },
                { @"\n", TokenType.Newline }, // TODO: handle newline which also contains a carriage return
                { ";", TokenType.Semicolon },
                { "#", TokenType.Hash },
                { @"\[", TokenType.LeftSquareBracket },
                { @"\]", TokenType.RightSquareBracket },
                { "=", TokenType.Equals },
                { "'", TokenType.SingleQuote },
                { "\"", TokenType.DoubleQuote },
                { @"\.", TokenType.Period },
                { "-", TokenType.Dash },
                { "&", TokenType.Ampersand },
                { "@", TokenType.At },
                { @"\d", TokenType.Digit },
                { @"[a-zA-Z]", TokenType.Letter }
            });
        }

        public void Dispose()
        {
            textReader.Dispose();
        }

        public Token Take()
        {
            Token toReturn;
            if (iterIndex < tokens.Count)
            {
                toReturn = tokens[iterIndex];
                iterIndex += 1;
                return toReturn;
            }

            toReturn = Peek();
            // don't increment iteration index if there's no more tokens
            if (toReturn.TokenType != TokenType.Empty)
                iterIndex += 1;

            return toReturn;
        }

        public Token TakeOfType(params TokenType[] types)
        {
            var upcoming = Peek();
            if (!types.Contains(upcoming.TokenType))
                throw new UnexpectedTokenException($"invalid token: got {upcoming}, excepted any of types {string.Join(", ", types)}");

            return Take();
        }

        public Token TakeAnyOtherThan(params TokenType[] types)
        {
            var upcoming = Peek();
            if (types.Contains(upcoming.TokenType))
                throw new UnexpectedTokenException($"invalid token: got {upcoming}, excepted any other than types {string.Join(", ", types)}");

            return Take();
        }

        public IEnumerable<Token> TakeSequentialOfType(params TokenType[] types)
        {
            var takenTokens = new List<Token>();

            while (types.Contains(Peek().TokenType))
                takenTokens.Add(Take());

            return takenTokens.AsEnumerable();
        }

        public Token Peek()
        {
            if (EndOfInput)
                return Token.Empty;

            if (iterIndex < tokens.Count)
                return tokens[iterIndex];

            if (TokeniseNext())
                return tokens[iterIndex];

            EndOfInput = true;
            return Token.Empty;
        }

        private bool TokeniseNext()
        {
            if (textReader.Peek() == -1)
                return false;

            var value = (char)textReader.Read();
            tokens.Add(new Token(MatchCharacterToToken(value), value.ToString()));
            return true;
        }

        private void PopulateTokenDictionary(Dictionary<string, TokenType> regexToTokenDictionary)
        {
            foreach (var regexTokenPair in regexToTokenDictionary)
                tokenDictionary.Add(new Regex(regexTokenPair.Key, RegexOptions.Compiled | RegexOptions.CultureInvariant), regexTokenPair.Value);
        }

        private TokenType MatchCharacterToToken(char character)
        {
            var charStr = character.ToString();
            var matchingRegex = tokenDictionary.Keys.SingleOrDefault(re => re.IsMatch(charStr));

            return matchingRegex == null ? TokenType.Unknown : tokenDictionary[matchingRegex];
        }
    }
}
