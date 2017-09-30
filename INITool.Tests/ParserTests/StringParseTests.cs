using System.IO;
using INITool.Parser;
using INITool.Parser.Units;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.ParserTests
{
    [TestFixture]
    public class StringParseTests
    {
        [Test]
        public void TestParseSingleQuotedString()
        {
            var options = new IniOptions();

            using (var reader = new StringReader("'Hello, World!'"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("'Hello, World!'", unit.SourceTokenString());
                Assert.AreEqual("Hello, World!", unit.Value);
            }
        }

        [Test]
        public void TestParseDoubleQuotedString()
        {
            using (var reader = new StringReader("\"Hello, World!\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("\"Hello, World!\"", unit.SourceTokenString());
                Assert.AreEqual("Hello, World!", unit.Value);
            }
        }

        [Test]
        public void TestIncompleteString()
        {
            using (var reader = new StringReader("\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<EndOfDocumentException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Test]
        public void TestNewlineInString()
        {
            using (var reader = new StringReader("\"Hello,\nWorld!\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Test]
        public void TestVerbatimString()
        {
            using (var reader = new StringReader("@\"Hello,\nWorld!\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("@\"Hello,\nWorld!\"", unit.SourceTokenString());
                Assert.AreEqual("Hello,\nWorld!", unit.Value);
            }
        }

        [Test, Sequential]
        public void TestEscapeSequences(
            [Values(@"\b", @"\n", @"\r", @"\t", @"\v", @"\\", @"\'", @"\""")] string input,
            [Values("\b", "\n", "\r", "\t", "\v", "\\", "'", "\"")] string escape)
        {
            using (var reader = new StringReader($"\"{input}\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual($"\"{input}\"", unit.SourceTokenString());
                Assert.AreEqual(escape, unit.Value);
            }
        }

        [Test]
        public void TestVerbatimEscapeSequenceString()
        {
            using (var reader = new StringReader(@"@""Hello,\nWorld!"""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual(@"@""Hello,\nWorld!""", unit.SourceTokenString());
                Assert.AreEqual(@"Hello,\nWorld!", unit.Value);
            }
        }
    }
}
