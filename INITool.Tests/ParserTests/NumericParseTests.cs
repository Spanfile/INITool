using System.IO;
using INITool.Parser.Parselets;
using INITool.Parser.Units;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class NumericParseTests
    {
        [Fact]
        public void TestParseInteger()
        {
            using (var reader = new StringReader("10"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("10", unit.SourceTokenString());
                Assert.Equal(10L, unit.Value);
            }
        }

        [Fact]
        public void TestParseTooLargeInteger()
        {
            using (var reader = new StringReader("9223372036854775808"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Fact]
        public void TestParseFloatingPoint()
        {
            using (var reader = new StringReader("1.1"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("1.1", unit.SourceTokenString());
                Assert.Equal(1.1d, unit.Value);
            }
        }

        [Fact]
        public void TestParseIntegerNegative()
        {
            using (var reader = new StringReader("-10"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("-10", unit.SourceTokenString());
                Assert.Equal(-10L, unit.Value);
            }
        }

        [Fact]
        public void TestParseFloatingPointNegative()
        {
            using (var reader = new StringReader("-1.1"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("-1.1", unit.SourceTokenString());
                Assert.Equal(-1.1d, unit.Value);
            }
        }

        [Fact]
        public void TestParseHexLiteralType1()
        {
            using (var reader = new StringReader("0xdeadbeef"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("0xdeadbeef", unit.SourceTokenString());
                Assert.Equal(3735928559UL, unit.Value);
            }
        }

        [Fact]
        public void TestParseTooLargeHexLiteral()
        {
            using (var reader = new StringReader("0xFFFFFFFFFFFFFFFFF"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Fact]
        public void TestParseHexLiteralType2()
        {
            using (var reader = new StringReader("&Hdeadbeef"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("&Hdeadbeef", unit.SourceTokenString());
                Assert.Equal(3735928559UL, unit.Value);
            }
        }

        [Fact]
        public void TestParseInvalidHexLiteral()
        {
            using (var reader = new StringReader("0xg"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Fact]
        public void TestParseBinaryLiteral()
        {
            using (var reader = new StringReader("0b0101100101011001"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("0b0101100101011001", unit.SourceTokenString());
                Assert.Equal(22873UL, unit.Value);
            }
        }

        [Fact]
        public void TestParseInvalidBinaryLiteral()
        {
            using (var reader = new StringReader("0b2"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }
    }
}
