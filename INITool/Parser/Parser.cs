using System;
using System.Collections.Generic;
using System.IO;
using INITool.Parser.Parselets;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;
using INITool.Structure;

namespace INITool.Parser
{
    internal class Parser : IDisposable
    {
        public static Parser FromFile(string filename) => new Parser(new StreamReader(File.OpenRead(filename)));

        public bool EndOfDocument => tokeniser.EndOfInput;

        private readonly Tokeniser.Tokeniser tokeniser;
        private readonly Dictionary<TokenType, Type> prefixParselets;
        private readonly Dictionary<TokenType, Type> infixParselets;
        private readonly Dictionary<Type, Parselet> parseletCache;

        public Parser(TextReader reader)
        {
            tokeniser = new Tokeniser.Tokeniser(reader);

            parseletCache = new Dictionary<Type, Parselet>();

            prefixParselets = new Dictionary<TokenType, Type>();
            infixParselets = new Dictionary<TokenType, Type>();

            RegisterPrefixParselet<NameParselet>(TokenType.Word);
            RegisterPrefixParselet<SectionParselet>(TokenType.LeftSquareBracket);
            RegisterPrefixParselet<NumericParselet>(TokenType.Number, TokenType.Dash, TokenType.Ampersand);
            RegisterPrefixParselet<StringParselet>(TokenType.SingleQuote, TokenType.DoubleQuote, TokenType.At);
            RegisterPrefixParselet<CommentParselet>(TokenType.Semicolon, TokenType.Hash);

            RegisterInfixParselet<AssignmentParselet>(TokenType.Equals);
        }

        public void Dispose()
        {
            tokeniser.Dispose();
        }

        private void RegisterPrefixParselet<T>(params TokenType[] tokensTypes) where T : Parselet, IPrefixParselet
        {
            foreach (var tokenType in tokensTypes)
                prefixParselets.Add(tokenType, typeof(T));
        }

        private void RegisterInfixParselet<T>(params TokenType[] tokensTypes) where T : Parselet, IInfixParselet
        {
            foreach (var tokenType in tokensTypes)
                infixParselets.Add(tokenType, typeof(T));
        }

        public Unit ParseUnit(bool skipNewline = true)
        {
            if (EndOfDocument)
                throw new EndOfDocumentException();

            // skip whitespace, possibly newlines too
            if (skipNewline)
                tokeniser.TakeSequentialOfType(TokenType.Whitespace, TokenType.Newline);
            else
                tokeniser.TakeSequentialOfType(TokenType.Whitespace);

            var token = tokeniser.Take();

            if (!prefixParselets.TryGetValue(token.TokenType, out Type prefixType))
            {
                if (token.TokenType != TokenType.Empty)
                    throw new InvalidTokenException(token);

                throw new EndOfDocumentException();
            }

            var prefix = (IPrefixParselet) GetParselet(prefixType);
            var left = prefix.Parse(token);

            // skip whitespace for infix parselets
            tokeniser.TakeSequentialOfType(TokenType.Whitespace);

            var upcoming = tokeniser.Peek();
            if (!infixParselets.TryGetValue(upcoming.TokenType, out Type infixType))
                return left;

            // consume the token peeked for infix parselets
            tokeniser.Take();
            var infix = (IInfixParselet) GetParselet(infixType);
            return infix.Parse(left, upcoming);
        }

        public T ParseUnitOfType<T>(bool skipNewline = true, bool throwOnEmptyPrefix = false) where T : Unit
        {
            var parsed = ParseUnit(skipNewline);
            if (!(parsed is T))
                throw new InvalidUnitException(parsed, typeof(T));
            return parsed as T;
        }

        private Parselet GetParselet(Type type)
        {
            if (parseletCache.TryGetValue(type, out Parselet parselet))
                return parselet;

            return (Parselet) Activator.CreateInstance(type, this, tokeniser);
        }
    }
}
