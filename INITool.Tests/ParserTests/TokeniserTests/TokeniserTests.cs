using System;
using System.IO;
using System.Linq;
using INITool.Parser;
using INITool.Parser.Tokeniser;
using NUnit.Framework;

namespace INITool.Tests.ParserTests.TokeniserTests
{
    [TestFixture]
    internal class TokeniserTests
    {
        [Test]
        public void TestTokenToString()
        {
            using (var reader = new StringReader("a"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual("<ln 0 col 0, Word: 'a'>", tokeniser.Take().ToString());
            }
        }

        [Test]
        public void TestTakeOfTypeValid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.DoesNotThrow(() => tokeniser.TakeOfType(TokenType.Semicolon));
            }
        }

        [Test]
        public void TestTakeOfTypeInvalid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => tokeniser.TakeOfType(TokenType.Semicolon));
            }
        }

        [Test]
        public void TestTakeAnyOtherThanValid()
        {
            using (var reader = new StringReader("#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.DoesNotThrow(() => tokeniser.TakeAnyOtherThan(TokenType.Semicolon));
            }
        }

        [Test]
        public void TestTakeAnyOtherThanInvalid()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => tokeniser.TakeAnyOtherThan(TokenType.Semicolon));
            }
        }

        [Test]
        public void TestRepeatedPeek()
        {
            using (var reader = new StringReader(";"))
            using (var tokeniser = new Tokeniser(reader))
            {
                for (var i = 0; i < 5; i++)
                    Assert.AreEqual(TokenType.Semicolon, tokeniser.Peek().TokenType);
            }
        }

        [Test]
        public void TestTakeTokensOfType()
        {
            using (var reader = new StringReader(";;;;;"))
            using (var tokeniser = new Tokeniser(reader))
            {
                var tokens = tokeniser.TakeSequentialOfType(TokenType.Semicolon);
                Assert.AreEqual(5, tokens.Count());
                foreach (var token in tokens)
                    Assert.AreEqual(TokenType.Semicolon, token.TokenType);
            }
        }
        
        [Test]
        public void TestTakeTokensOfAnyOtherThan()
        {
            using (var reader = new StringReader(";;;;;#"))
            using (var tokeniser = new Tokeniser(reader))
            {
                var tokens = tokeniser.TakeSequentialOfAnyOtherThan(TokenType.Hash);
                Assert.AreEqual(5, tokens.Count());
                foreach (var token in tokens)
                    Assert.AreEqual(TokenType.Semicolon, token.TokenType);
            }
        }

        [Test]
        public void TestTakeEmptyToken()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(TokenType.Empty, tokeniser.Take().TokenType);
            }
        }

        [Test]
        public void TestEndOfInput()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                tokeniser.Take();
                Assert.AreEqual(true, tokeniser.EndOfInput);
            }
        }

        [Test]
        public void TestTakeEmptyTokens()
        {
            using (var reader = new StringReader(""))
            using (var tokeniser = new Tokeniser(reader))
            {
                for (var i = 0; i < 3; i++)
                    Assert.AreEqual(TokenType.Empty, tokeniser.Take().TokenType);
            }
        }

        [Test]
        public void TestTokenPositions()
        {
            using (var reader = new StringReader("a\na\r\na"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(new Position(0, 0), tokeniser.Take().Position);
                Assert.AreEqual(new Position(0, 1), tokeniser.Take().Position);
                Assert.AreEqual(new Position(1, 0), tokeniser.Take().Position);
                Assert.AreEqual(new Position(1, 1), tokeniser.Take().Position);
                Assert.AreEqual(new Position(2, 0), tokeniser.Take().Position);

                Assert.AreEqual(new Position(2, 0), tokeniser.CurrentPosition);
            }
        }

        [Test]
        public void TestWhitespace()
        {
            using (var reader = new StringReader(" \t\f\va"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(" \t\f\v", tokeniser.TakeOfType(TokenType.Whitespace).Value);
            }
        }

        [Test]
        public void TestNewline()
        {
            using (var reader = new StringReader("\n\r\n"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual("\n", tokeniser.TakeOfType(TokenType.Newline).Value);
                Assert.AreEqual("\r\n", tokeniser.TakeOfType(TokenType.Newline).Value);
            }
        }

        [Test]
        public void TestIncompleteNewline()
        {
            using (var reader = new StringReader("\r"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.Throws<ArgumentException>(() => tokeniser.TakeOfType(TokenType.Newline));
            }
        }

        [Test, Sequential]
        public void TestSingleCharacters(
            [Values(
            TokenType.Semicolon,
            TokenType.Hash,
            TokenType.LeftSquareBracket,
            TokenType.RightSquareBracket,
            TokenType.Equals,
            TokenType.SingleQuote,
            TokenType.DoubleQuote,
            TokenType.Period,
            TokenType.Dash,
            TokenType.Ampersand,
            TokenType.At)] TokenType type,
            [Values(";", "#", "[", "]", "=", "'", "\"", ".", "-", "&", "@")] string input)
        {
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(input, tokeniser.TakeOfType(type).Value);
            }
        }

        [Test]
        public void TestWord(
            [Values("a", "aA")] string input)
        {
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(input, tokeniser.TakeOfType(TokenType.Word).Value);
            }
        }

        [Test]
        public void TestNumber(
            [Values("1", "11")] string input)
        {
            using (var reader = new StringReader(input))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual(input, tokeniser.TakeOfType(TokenType.Number).Value);
            }
        }

        [Test]
        public void TestEscapeSequence()
        {
            using (var reader = new StringReader("\\a"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual("\\a", tokeniser.TakeOfType(TokenType.EscapeSequence).Value);
            }
        }

        [Test]
        public void TestSingleUnknown()
        {
            using (var reader = new StringReader("_"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
            }
        }

        [Test]
        public void TestMultipleUnknowns()
        {
            using (var reader = new StringReader("__"))
            using (var tokeniser = new Tokeniser(reader))
            {
                Assert.AreEqual("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
                Assert.AreEqual("_", tokeniser.TakeOfType(TokenType.Unknown).Value);
            }
        }
    }
}
