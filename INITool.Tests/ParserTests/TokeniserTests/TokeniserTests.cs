using System.IO;
using System.Linq;
using INITool.Parser;
using INITool.Parser.Tokeniser;
using Xunit;
using System;

namespace INITool.Tests.ParserTests.TokeniserTests
{
    public class TokeniserTests
    {
        [Fact]
        public void TestTokenToString()
        {
            using (var reader = new StringReader("a"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal("<ln 0 col 0, Word: 'a'>", tokeniser.Take().ToString());
            }
        }

        [Fact]
        public void TestTakeOfTypeValid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                tokeniser.TakeOfType(TokenType.Semicolon);
            }
        }

        [Fact]
        public void TestTakeOfTypeInvalid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => tokeniser.TakeOfType(TokenType.Semicolon));
            }
        }

        [Fact]
        public void TestTakeAnyOtherThanValid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                tokeniser.TakeAnyOtherThan(TokenType.Semicolon);
            }
        }

        [Fact]
        public void TestTakeAnyOtherThanInvalid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => tokeniser.TakeAnyOtherThan(TokenType.Semicolon));
            }
        }

        [Fact]
        public void TestRepeatedPeek()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                for (var i = 0; i < 5; i++)
                    Assert.Equal(TokenType.Semicolon, tokeniser.Peek().TokenType);
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
                    Assert.Equal(TokenType.Semicolon, token.TokenType);
            }
        }

        [Fact]
        public void TestTakeEmptyToken()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(TokenType.Empty, tokeniser.Take().TokenType);
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
                    Assert.Equal(TokenType.Empty, tokeniser.Take().TokenType);
            }
        }

        [Fact]
        public void TestTokenPositions()
        {
            using (var reader = new StringReader("a\na\r\na"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(new Position(0, 0), tokeniser.Take().Position);
                Assert.Equal(new Position(0, 1), tokeniser.Take().Position);
                Assert.Equal(new Position(1, 0), tokeniser.Take().Position);
                Assert.Equal(new Position(1, 1), tokeniser.Take().Position);
                Assert.Equal(new Position(2, 0), tokeniser.Take().Position);

                Assert.Equal(new Position(2, 0), tokeniser.CurrentPosition);
            }
        }

        [Fact]
        public void TestWhitespace()
        {
            using (var reader = new StringReader(" \t\f\va"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(" \t\f\v", tokeniser.TakeOfType(TokenType.Whitespace).Value);
            }
        }

        [Fact]
        public void TestNewline()
        {
            using (var reader = new StringReader("\n\r\n"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal("\n", tokeniser.TakeOfType(TokenType.Newline).Value);
                Assert.Equal("\r\n", tokeniser.TakeOfType(TokenType.Newline).Value);
            }
        }

        [Fact]
        public void TestIncompleteNewline()
        {
            using (var reader = new StringReader("\r"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<ArgumentException>(() => tokeniser.TakeOfType(TokenType.Newline));
            }
        }

        [Theory]
        [InlineData("Semicolon", ";")]
        [InlineData("Hash", "#")]
        [InlineData("LeftSquareBracket", "[")]
        [InlineData("RightSquareBracket", "]")]
        [InlineData("Equals", "=")]
        [InlineData("SingleQuote", "'")]
        [InlineData("DoubleQuote", "\"")]
        [InlineData("Period", ".")]
        [InlineData("Dash", "-")]
        [InlineData("Ampersand", "&")]
        [InlineData("At", "@")]
        public void TestSingleCharacters(string typeStr, string input)
        {
            var type = Enum.Parse<TokenType>(typeStr);
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(input, tokeniser.TakeOfType(type).Value);
            }
        }

        [Theory]
        [InlineData("a")]
        [InlineData("aA")]
        public void TestWord(string input)
        {
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(input, tokeniser.TakeOfType(TokenType.Word).Value);
            }
        }

        [Theory]
        [InlineData("1")]
        [InlineData("11")]
        public void TestNumber(string input)
        {
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal(input, tokeniser.TakeOfType(TokenType.Number).Value);
            }
        }

        [Fact]
        public void TestEscapeSequence()
        {
            using (var reader = new StringReader("\\a"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal("\\a", tokeniser.TakeOfType(TokenType.EscapeSequence).Value);
            }
        }

        [Fact]
        public void TestSingleUnknown()
        {
            using (var reader = new StringReader("_"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
            }
        }

        [Fact]
        public void TestMultipleUnknowns()
        {
            using (var reader = new StringReader("__"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Equal("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
                Assert.Equal("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
            }
        }
    }
}
