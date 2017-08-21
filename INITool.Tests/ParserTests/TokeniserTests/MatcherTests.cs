using INITool.Parser.Tokeniser.Matchers;
using Xunit;

namespace INITool.Tests.ParserTests.TokeniserTests
{
    public class MatcherTests
    {
        [Theory]
        [InlineData(' ', true)]
        [InlineData('\t', true)]
        [InlineData('\f', true)]
        [InlineData('\v', true)]
        [InlineData('\n', false)]
        [InlineData('\r', false)]
        public void TestWhitespace(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, WhitespaceMatcher.Match(input) != null);
        }

        [Theory]
        [InlineData('\n', true)]
        [InlineData('\r', true)]
        [InlineData(' ', false)]
        public void TestNewline(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, NewlineMatcher.Match(input) != null);
        }

        [Theory]
        [InlineData(';', true)]
        [InlineData('#', true)]
        [InlineData('[', true)]
        [InlineData(']', true)]
        [InlineData('=', true)]
        [InlineData('\'', true)]
        [InlineData('"', true)]
        [InlineData('.', true)]
        [InlineData('-', true)]
        [InlineData('&', true)]
        [InlineData('@', true)]
        [InlineData('a', false)]
        public void TestSingleCharacter(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, SingleCharacterMatcher.Match(input) != null);
        }

        [Theory]
        [InlineData('a', true)]
        [InlineData('A', true)]
        [InlineData('1', false)]
        public void TestWord(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, WordMatcher.Match(input) != null);
        }

        [Theory]
        [InlineData('1', true)]
        [InlineData('a', false)]
        public void TestNumber(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, NumberMatcher.Match(input) != null);
        }

        [Fact]
        public void TestEscapeSequence()
        {
            Assert.NotNull(EscapeSequenceMatcher.Match('\\'));
        }

        [Theory]
        [InlineData('_', true)]
        [InlineData('-', true)]
        public void TestUnknown(char input, bool shouldMatch)
        {
            Assert.Equal(shouldMatch, UnknownMatcher.Match(input) != null);
        }
    }
}
