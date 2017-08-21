using System.IO;
using INITool.Parser;
using INITool.Parser.Units;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class AssignmentParseTests
    {
        [Fact]
        public void TestParseIntegerAssignment()
        {
            using (var reader = new StringReader("value=10"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.Equal("value=10", unit.SourceTokenString());

                Assert.Equal("value", unit.Name);
                Assert.Equal(10L, unit.Value);
            }
        }

        [Fact]
        public void TestParseFloatingPointAssignment()
        {
            using (var reader = new StringReader("value=1.1"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.Equal("value=1.1", unit.SourceTokenString());

                Assert.Equal("value", unit.Name);
                Assert.Equal(1.1d, unit.Value);
            }
        }

        [Fact]
        public void TestParseBooleanAssignment()
        {
            using (var reader = new StringReader("value=true"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.Equal("value=true", unit.SourceTokenString());

                Assert.Equal("value", unit.Name);
                Assert.Equal(true, unit.Value);
            }
        }

        [Fact]
        public void TestParseStringAssignment()
        {
            using (var reader = new StringReader("value='Hello, World!'"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.Equal("value='Hello, World!'", unit.SourceTokenString());

                Assert.Equal("value", unit.Name);
                Assert.Equal("Hello, World!", unit.Value);
            }
        }

        [Fact]
        public void TestNewlineInAssignment()
        {
            using (var reader = new StringReader("value=\n10"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnit());
            }
        }

        [Fact]
        public void TestIncompleteAssignment()
        {
            using (var reader = new StringReader("value="))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<EndOfDocumentException>(() => parser.ParseUnit());
            }
        }

        [Fact]
        public void TestInvalidLeftAssignment()
        {
            using (var reader = new StringReader("10=10"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidUnitException>(() => parser.ParseUnit());
            }
        }


        [Fact]
        public void TestInvalidAssignment()
        {
            using (var reader = new StringReader("value=value"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidUnitException>(() => parser.ParseUnitOfType<AssignmentUnit>());
            }
        }
    }
}
