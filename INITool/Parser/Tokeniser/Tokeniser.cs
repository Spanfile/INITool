using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using INITool.Parser.Tokeniser.Matchers;

namespace INITool.Parser.Tokeniser
{
    internal class Tokeniser : IDisposable
    {
        public bool EndOfInput { get; private set; }
        public Position CurrentPosition { get; private set; }
        private Position ReadPosition => new Position(readLine, readColumn);

        private readonly List<Matcher.MatcherDelegate> matchers;

        private readonly TextReader textReader;
        private readonly List<Token> tokens;
        private int iterIndex;

        private int readLine;
        private int readColumn;

        public Tokeniser(TextReader textReader)
        {
            this.textReader = textReader;

            matchers = new List<Matcher.MatcherDelegate> {
                WhitespaceMatcher.Match,
                NewlineMatcher.Match,
                SingleCharacterMatcher.Match,
                WordMatcher.Match,
                NumberMatcher.Match,
                EscapeSequenceMatcher.Match,
                UnknownMatcher.Match // unknown matcher has to be last
            };

            tokens = new List<Token>();
            iterIndex = 0;
            EndOfInput = false;
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

            CurrentPosition = toReturn.Position;

            return toReturn;
        }

        public Token TakeOfType(params TokenType[] types)
        {
            var upcoming = Peek();
            if (!types.Contains(upcoming.TokenType))
                throw new InvalidTokenException(upcoming, $"excepted any of types {string.Join(", ", types)}");

            return Take();
        }

        public Token TakeAnyOtherThan(params TokenType[] types)
        {
            var upcoming = Peek();
            if (types.Contains(upcoming.TokenType))
                throw new InvalidTokenException(upcoming, $"excepted any other than types {string.Join(", ", types)}");

            return Take();
        }

        // these methods aren't lazily evaluated because of the Take() side-effect
        public IEnumerable<Token> TakeSequentialOfType(params TokenType[] types)
        {
            var takenTokens = new List<Token>();

            while (types.Contains(Peek().TokenType))
                takenTokens.Add(Take());

            return takenTokens.AsEnumerable();
        }
        
        public IEnumerable<Token> TakeSequentialOfAnyOtherThan(params TokenType[] types)
        {
            var takenTokens = new List<Token>();

            while (!types.Contains(Peek().TokenType))
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

            var character = (char)textReader.Read();
            var (type, value) = GetTokenTypeAndValue(character);
            tokens.Add(new Token(type, value, ReadPosition));

            readColumn += value.Length;

            // ReSharper disable once InvertIf
            if (type == TokenType.Newline)
            {
                readLine += 1;
                readColumn = 0;
            }

            return true;
        }

        private (TokenType, string) GetTokenTypeAndValue(char character)
        {
            foreach (var matcher in matchers)
            {
                var result = matcher(character);
                if (result != null)
                    return result(character, textReader);
            }

            throw new ArgumentException($"no matcher found for '{character}'", nameof(character));
        }
    }
}
