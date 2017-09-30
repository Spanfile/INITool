using System;
using INITool.Structure.Sections;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable RedundantArgumentDefaultValue

namespace INITool.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void TestInvalidPropertyIdentifier()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Multiple(() =>
                {
                    Assert.Throws<InvalidPropertyIdentifierException>(() => reader.GetString("too.many.arguments"));
                    Assert.Throws<InvalidPropertyIdentifierException>(() => reader.GetString(""));
                });
            }
        }

        [Test]
        public void TestInvalidCommentableIdentifier()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Multiple(() =>
                {
                    Assert.Throws<InvalidPropertyIdentifierException>(() => reader.GetComment("too.many.arguments"));
                    Assert.Throws<InvalidPropertyIdentifierException>(() => reader.GetComment(""));
                });
            }
        }

        [Test]
        public void TestReadCaseSensitive(
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: true,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.Throws<ArgumentException>(() => reader.GetInt64("Section.uppercase"));
            }
        }

        [Test]
        public void TestReadCaseInsensitive(
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: false,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(10L, reader.GetInt64("Section.uppercase"));
            }
        }

        [Test]
        public void TestReadInvalidType(
            [Values] bool caseSensitive)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: false);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.Throws<InvalidCastException>(() => reader.GetDouble("Section.int"));
            }
        }

        [Test]
        public void TestReadImplicitCastToStringNotAllowed(
            [Values] bool caseSensitive)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: false);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.Throws<InvalidCastException>(() => reader.GetString("Section.int"));
            }
        }

        [Test]
        public void TestReadImplicitCastToStringAllowed(
            [Values] bool caseSensitive)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: true);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("10", reader.GetString("Section.int"));
            }
        }

        [Test]
        public void TestReadParseFromStringNotAllowed(
            [Values] bool caseSensitive)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: false);

            using (var reader = new IniReader("special.ini", options))
            {
                Assert.Throws<InvalidCastException>(() => reader.GetInt64("int_in_string"));
            }
        }

        [Test]
        public void TestReadParseFromStringAllowed(
            [Values] bool caseSensitive)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: true);

            using (var reader = new IniReader("special.ini", options))
            {
                Assert.AreEqual(10L, reader.GetInt64("int_in_string"));
            }
        }

        [Test]
        public void TestReadInt64(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(10L, reader.GetInt64("Section.int"));
            }
        }

        [Test]
        public void TestReadString(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("Hello, World!", reader.GetString("Section.string"));
            }
        }

        [Test]
        public void TestReadDouble(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(1.1d, reader.GetDouble("Section.float"));
            }
        }

        [Test]
        public void TestReadBool(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(true, reader.GetBool("Section.bool"));
            }
        }

        [Test]
        public void TestImplicitReadFromDefaultSection(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(10L, reader.GetInt64("nosection"));
            }
        }

        [Test]
        public void TestExplicitReadFromDefaultSection(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual(10L, reader.GetInt64($"{Section.DefaultSectionName}.nosection"));
            }
        }

        [Test]
        public void TestGetCommentForSection(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("section", reader.GetComment("Section"));
            }
        }

        [Test]
        public void TestGetCommentForProperty(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("property", reader.GetComment("Section.string"));
            }
        }

        [Test]
        public void TestImplicitGetCommentForDefaultSection(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("comment\ncomment", reader.GetComment("nosection"));
            }
        }

        [Test]
        public void TestExplicitGetCommentForDefaultSection(
            [Values] bool caseSensitive,
            [Values] bool allowValueConversion,
            [Values] bool allowLooseProperties)
        {
            var options = new IniOptions(
                caseSensitive: caseSensitive,
                allowValueConversion: allowValueConversion,
                allowLooseProperties: allowLooseProperties);

            using (var reader = new IniReader("basic.ini", options))
            {
                Assert.AreEqual("comment\ncomment", reader.GetComment($"{Section.DefaultSectionName}.nosection"));
            }
        }
    }
}
