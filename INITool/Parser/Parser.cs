using System;
using System.Collections.Generic;
using System.IO;
using INITool.Parser.Parselets;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser
{
    internal class Parser : IDisposable
    {
        public static Parser FromFile(string filename, IniOptions options)
            => new Parser(new StreamReader(File.OpenRead(filename)), options);

        public bool EndOfDocument => tokeniser.EndOfInput;

        private readonly IniOptions options;
        
        private readonly Tokeniser.Tokeniser tokeniser;
        private readonly Dictionary<TokenType, Type> prefixParselets;
        private readonly Dictionary<TokenType, Type> infixParselets;
        private readonly Dictionary<Type, Type> singleUnitParselets;
        private Type invariantParselet;
        private readonly Dictionary<Type, Parselet> parseletCache;

        public Parser(TextReader reader, IniOptions options)
        {
            this.options = options;
            tokeniser = new Tokeniser.Tokeniser(reader);

            parseletCache = new Dictionary<Type, Parselet>();

            prefixParselets = new Dictionary<TokenType, Type>();
            infixParselets = new Dictionary<TokenType, Type>();
            singleUnitParselets = new Dictionary<Type, Type>();

            RegisterPrefixParselet<NameParselet>(TokenType.Word);
            RegisterPrefixParselet<SectionParselet>(TokenType.LeftSquareBracket);
            RegisterPrefixParselet<NumericParselet>(TokenType.Number, TokenType.Dash, TokenType.Ampersand);
            RegisterPrefixParselet<StringParselet>(TokenType.SingleQuote, TokenType.DoubleQuote, TokenType.At);
            RegisterPrefixParselet<CommentParselet>(TokenType.Semicolon, TokenType.Hash);

            RegisterInfixParselet<AssignmentParselet>(TokenType.Equals);
            
            RegisterInvariantParselet<InvariantParselet>();

            RegisterSingleUnitTransform<NameUnit, AssignmentParselet>();
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

        private void RegisterInvariantParselet<T>() where T : Parselet, IPrefixParselet
        {
            invariantParselet = typeof(T);
        }

        private void RegisterSingleUnitTransform<TUnit, TParselet>() where TUnit : Unit where TParselet : Parselet, ISingleUnitParselet
        {
            singleUnitParselets.Add(typeof(TUnit), typeof(TParselet));
        }

        public Unit ParseUnit(bool skipNewline = true, bool skipWhitespace = true, bool allowSingleUnitTransform = true)
        {
            if (EndOfDocument)
                throw new EndOfDocumentException();

            var tokensToSkip = new List<TokenType>(2);

            if (skipNewline)
                tokensToSkip.Add(TokenType.Newline);
            if (skipWhitespace)
                tokensToSkip.Add(TokenType.Whitespace);

            tokeniser.TakeSequentialOfType(tokensToSkip.ToArray());

            var token = tokeniser.Take();

            if (!prefixParselets.TryGetValue(token.TokenType, out var prefixType))
            {
                if (token.TokenType == TokenType.Empty)
                    throw new EndOfDocumentException();
                
                if (options.PropertyParsing == PropertyParsing.Strong)
                    throw new InvalidTokenException(token);

                prefixType = invariantParselet;
            }

            var prefix = (IPrefixParselet) GetParselet(prefixType);
            var left = prefix.Parse(token);

            if (skipWhitespace)
                tokeniser.TakeSequentialOfType(TokenType.Whitespace);

            var upcoming = tokeniser.Peek();
            if (!infixParselets.TryGetValue(upcoming.TokenType, out var infixType))
            {
                // check if this unit can be transformed into another
                if (!singleUnitParselets.TryGetValue(left.GetType(), out var singleUnitType))
                    return left;

                if (!allowSingleUnitTransform)
                    return left;

                if (GetParselet(singleUnitType) is ISingleUnitParselet singleUnitParselet)
                    return singleUnitParselet.TransformUnit(left);
            }

            // consume the token peeked for infix parselets
            tokeniser.Take();
            var infix = (IInfixParselet) GetParselet(infixType);
            return infix.Parse(left, upcoming);
        }

        public T ParseUnitOfType<T>(bool skipNewline = true, bool skipWhitespace = true, bool allowSingleUnitTransform = true) where T : Unit
        {
            var parsed = ParseUnit(skipNewline, skipWhitespace, allowSingleUnitTransform);
            if (!(parsed is T))
                throw new InvalidUnitException(parsed, typeof(T));
            return parsed as T;
        }

        private Parselet GetParselet(Type type)
        {
            if (parseletCache.TryGetValue(type, out var parselet))
                return parselet;

            var instance = (Parselet)Activator.CreateInstance(type, this, tokeniser, options);
            parseletCache.Add(type, instance);
            return instance;
        }
    }
}
