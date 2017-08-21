using System;
using INITool.Structure.Sections;
using Xunit;
// ReSharper disable ArgumentsStyleLiteral

namespace INITool.Tests
{
    public class ReaderTests
    {
        [Fact]
        public void TestReadCaseSensitive()
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            using (var reader = new IniReader("basic.ini", new IniOptions(caseSensitive: true)))
            {
                Assert.Throws<ArgumentException>(() => reader.GetInt64("Section.uppercase"));
            }
        }

        [Fact]
        public void TestReadCaseInsensitive()
        {
            using (var reader = new IniReader("basic.ini", new IniOptions(caseSensitive: false)))
            {
                Assert.Equal(10L, reader.GetInt64("Section.uppercase"));
            }
        }

        [Fact]
        public void TestReadInt64()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal(10L, reader.GetInt64("Section.int"));
            }
        }

        [Fact]
        public void TestReadString()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal("Hello, World!", reader.GetString("Section.string"));
            }
        }

        [Fact]
        public void TestReadDouble()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal(1.1d, reader.GetDouble("Section.float"));
            }
        }

        [Fact]
        public void TestReadBool()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal(true, reader.GetBool("Section.bool"));
            }
        }

        [Fact]
        public void TestImplicitReadFromDefaultSection()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal(10L, reader.GetInt64("nosection"));
            }
        }

        [Fact]
        public void TestExplicitReadFromDefaultSection()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal(10L, reader.GetInt64($"{Section.DefaultSectionName}.nosection"));
            }
        }

        [Fact]
        public void TestGetCommentForSection()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal("section", reader.GetComment("Section"));
            }
        }

        [Fact]
        public void TestGetCommentForProperty()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal("property", reader.GetComment("Section.string"));
            }
        }

        [Fact]
        public void TestImplicitGetCommentForDefaultSection()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal("comment\ncomment", reader.GetComment("nosection"));
            }
        }

        [Fact]
        public void TestExplicitGetCommentForDefaultSection()
        {
            using (var reader = new IniReader("basic.ini", IniOptions.Default))
            {
                Assert.Equal("comment\ncomment", reader.GetComment($"{Section.DefaultSectionName}.nosection"));
            }
        }
    }
}
