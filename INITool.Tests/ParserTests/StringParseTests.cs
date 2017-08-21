using System.IO;
using INITool.Parser;
using INITool.Parser.Units;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class StringParseTests
    {
        [Fact]
        public void TestParseSingleQuotedString()
        {
            using (var reader = new StringReader("'Hello, World!'"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("'Hello, World!'", unit.SourceTokenString());
                Assert.Equal("Hello, World!", unit.Value);
            }
        }

        [Fact]
        public void TestParseDoubleQuotedString()
        {
            using (var reader = new StringReader("\"Hello, World!\""))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("\"Hello, World!\"", unit.SourceTokenString());
                Assert.Equal("Hello, World!", unit.Value);
            }
        }

        [Fact]
        public void TestNewlineInString()
        {
            using (var reader = new StringReader("\"Hello,\nWorld!\""))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Fact]
        public void TestVerbatimString()
        {
            using (var reader = new StringReader("@\"Hello,\nWorld!\""))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("@\"Hello,\nWorld!\"", unit.SourceTokenString());
                Assert.Equal("Hello,\nWorld!", unit.Value);
            }
        }

        [Theory]
        [InlineData(@"\b", "\b")]
        [InlineData(@"\n", "\n")]
        [InlineData(@"\r", "\r")]
        [InlineData(@"\t", "\t")]
        [InlineData(@"\v", "\v")]
        [InlineData(@"\\", "\\")]
        [InlineData(@"\'", "'")]
        [InlineData(@"\""", "\"")]
        public void TestEscapeSequences(string input, string escape)
        {
            using (var reader = new StringReader($"\"{input}\""))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal($"\"{input}\"", unit.SourceTokenString());
                Assert.Equal(escape, unit.Value);
            }
        }

        [Fact]
        public void TestVerbatimEscapeSequenceString()
        {
            using (var reader = new StringReader(@"@""Hello,\nWorld!"""))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal(@"@""Hello,\nWorld!""", unit.SourceTokenString());
                Assert.Equal(@"Hello,\nWorld!", unit.Value);
            }
        }
    }
}
