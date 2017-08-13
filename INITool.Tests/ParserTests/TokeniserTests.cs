using System.IO;
using System.Linq;
using INITool.Parser.Tokeniser;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class TokeniserTests
    {
        private static void AssertToken(Token token, TokenType type, string value = null)
        {
            Assert.Equal(type, token.TokenType);
            if (value != null)
                Assert.Equal(value, token.Value);
        }

        [Fact]
        public void TestWhitespceTokens()
        {
            using (var reader = new StringReader(" \t\f\v"))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Whitespace, " ");
                AssertToken(tokeniser.Take(), TokenType.Whitespace, "\t");
                AssertToken(tokeniser.Take(), TokenType.Whitespace, "\f");
                AssertToken(tokeniser.Take(), TokenType.Whitespace, "\v");
            }
        }

        [Fact]
        public void TestNewlineToken()
        {
            using (var reader = new StringReader("\n"))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Newline, "\n");
            }
        }

        [Fact]
        public void TestSingleCharacterTokens()
        {
            using (var reader = new StringReader(";#[]='\".-&@1aA"))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Semicolon, ";");
                AssertToken(tokeniser.Take(), TokenType.Hash, "#");
                AssertToken(tokeniser.Take(), TokenType.LeftSquareBracket, "[");
                AssertToken(tokeniser.Take(), TokenType.RightSquareBracket, "]");
                AssertToken(tokeniser.Take(), TokenType.Equals, "=");
                AssertToken(tokeniser.Take(), TokenType.SingleQuote, "'");
                AssertToken(tokeniser.Take(), TokenType.DoubleQuote, "\"");
                AssertToken(tokeniser.Take(), TokenType.Period, ".");
                AssertToken(tokeniser.Take(), TokenType.Dash, "-");
                AssertToken(tokeniser.Take(), TokenType.Ampersand, "&");
                AssertToken(tokeniser.Take(), TokenType.At, "@");
                AssertToken(tokeniser.Take(), TokenType.Digit, "1");
                AssertToken(tokeniser.Take(), TokenType.Letter, "a");
                AssertToken(tokeniser.Take(), TokenType.Letter, "A");
            }
        }

        [Fact]
        public void TestTakeOfTypeValid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Semicolon);
            }
        }

        [Fact]
        public void TestTakeOfTypeInvalid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<UnexpectedTokenException>(() => tokeniser.TakeOfType(TokenType.Semicolon));
            }
        }

        [Fact]
        public void TestTakeAnyOtherThanValid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Hash);
            }
        }

        [Fact]
        public void TestTakeAnyOtherThanInvalid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<UnexpectedTokenException>(() => tokeniser.TakeAnyOtherThan(TokenType.Semicolon));
            }
        }

        [Fact]
        public void TestRepeatedPeek()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                for (var i = 0; i < 5; i++)
                    AssertToken(tokeniser.Peek(), TokenType.Semicolon);
            }
        }

        [Fact]
        public void TestTakeTokensOfType()
        {
            using (var reader = new StringReader(";;;;;"))
            using (var tokeniser = new Tokeniser(reader))
            {
                var tokens = tokeniser.TakeSequentialOfType(TokenType.Semicolon).ToArray();
                Assert.Equal(5, tokens.Length);
                foreach (var token in tokens)
                    AssertToken(token, TokenType.Semicolon);
            }
        }

        [Fact]
        public void TestTakeEmptyToken()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                AssertToken(tokeniser.Take(), TokenType.Empty);
            }
        }

        [Fact]
        public void TestEndOfInput()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                tokeniser.Take();
                Assert.Equal(true, tokeniser.EndOfInput);
            }
        }

        [Fact]
        public void TestTakeEmptyTokens()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                for (var i = 0; i < 3; i++)
                    AssertToken(tokeniser.Take(), TokenType.Empty);
            }
        }
    }
}
