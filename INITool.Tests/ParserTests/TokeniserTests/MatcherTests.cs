using INITool.Parser.Tokeniser.Matchers;
using NUnit.Framework;

namespace INITool.Tests.ParserTests.TokeniserTests
{
    [TestFixture]
    public class MatcherTests
    {
        [Test]
        public void TestValidWhitespace(
            [Values(' ', '\t', '\f', '\v')] char input)
        {
            Assert.NotNull(WhitespaceMatcher.Match(input));
        }

        [Test]
        public void TestInvalidWhitespace(
            [Values('\n', '\r')] char input)
        {
            Assert.Null(WhitespaceMatcher.Match(input));
        }

        [Test]
        public void TestValidNewline(
            [Values('\n', '\r')] char input)
        {
            Assert.NotNull(NewlineMatcher.Match(input));
        }

        [Test]
        public void TestInvalidNewline(
            [Values(' ')] char input)
        {
            Assert.Null(NewlineMatcher.Match(input));
        }

        [Test]
        public void TestValidSingleCharacter(
            [Values(';', '#', '[', ']', '=', '\'', '"', '.', '-', '&', '@')] char input)
        {
            Assert.NotNull(SingleCharacterMatcher.Match(input));
        }

        [Test]
        public void TestInvalidSingleCharacter(
            [Values('a')] char input)
        {
            Assert.Null(SingleCharacterMatcher.Match(input));
        }

        [Test]
        public void TestValidWord(
            [Values('a', 'A')] char input)
        {
            Assert.NotNull(WordMatcher.Match(input));
        }

        [Test]
        public void TestInvalidWord(
            [Values('1')] char input)
        {
            Assert.Null(WordMatcher.Match(input));
        }

        [Test]
        public void TestValidNumber(
            [Values('1')] char input)
        {
            Assert.NotNull(NumberMatcher.Match(input));
        }

        [Test]
        public void TestInvalidNumber(
            [Values('a')] char input)
        {
            Assert.Null(NumberMatcher.Match(input));
        }

        [Test]
        public void TestEscapeSequence()
        {
            Assert.NotNull(EscapeSequenceMatcher.Match('\\'));
        }

        [Test]
        public void TestUnknown(
            [Values('_', '-')] char input)
        {
            Assert.NotNull(UnknownMatcher.Match(input));
        }
    }
}
