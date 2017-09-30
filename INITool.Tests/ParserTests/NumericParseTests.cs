using System.IO;
using INITool.Parser.Parselets;
using INITool.Parser.Units;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.ParserTests
{
    [TestFixture]
    public class NumericParseTests
    {
        [Test]
        public void TestParseInteger()
        {
            using (var reader = new StringReader("10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("10", unit.SourceTokenString());
                Assert.AreEqual(10L, unit.Value);
            }
        }

        [Test]
        public void TestParseTooLargeInteger()
        {
            using (var reader = new StringReader("9223372036854775808"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Test]
        public void TestParseFloatingPoint()
        {
            using (var reader = new StringReader("1.1"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("1.1", unit.SourceTokenString());
                Assert.AreEqual(1.1d, unit.Value);
            }
        }

        [Test]
        public void TestParseIntegerNegative()
        {
            using (var reader = new StringReader("-10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("-10", unit.SourceTokenString());
                Assert.AreEqual(-10L, unit.Value);
            }
        }

        [Test]
        public void TestParseFloatingPointNegative()
        {
            using (var reader = new StringReader("-1.1"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("-1.1", unit.SourceTokenString());
                Assert.AreEqual(-1.1d, unit.Value);
            }
        }

        [Test]
        public void TestParseHexLiteralType1()
        {
            using (var reader = new StringReader("0xdeadbeef"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("0xdeadbeef", unit.SourceTokenString());
                Assert.AreEqual(3735928559UL, unit.Value);
            }
        }

        [Test]
        public void TestParseTooLargeHexLiteral()
        {
            using (var reader = new StringReader("0xFFFFFFFFFFFFFFFFF"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Test]
        public void TestParseHexLiteralType2()
        {
            using (var reader = new StringReader("&Hdeadbeef"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("&Hdeadbeef", unit.SourceTokenString());
                Assert.AreEqual(3735928559UL, unit.Value);
            }
        }

        [Test]
        public void TestParseInvalidHexLiteral()
        {
            using (var reader = new StringReader("0xg"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }

        [Test]
        public void TestParseBinaryLiteral()
        {
            using (var reader = new StringReader("0b0101100101011001"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("0b0101100101011001", unit.SourceTokenString());
                Assert.AreEqual(22873UL, unit.Value);
            }
        }

        [Test]
        public void TestParseInvalidBinaryLiteral()
        {
            using (var reader = new StringReader("0b2"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidLiteralException>(() => parser.ParseUnitOfType<ValueUnit>());
            }
        }
    }
}
