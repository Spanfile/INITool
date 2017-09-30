using System;
using System.IO;
using NUnit.Framework;

namespace INITool.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void TestOptionTemplateFlexible()
        {
            using (var reader = new IniReader("flexible.ini", IniOptions.Flexible))
            {
                Assert.AreEqual("flexible value", reader.GetString("flexible"));
                Assert.AreEqual(10L, reader.GetInt64("int_in_string"));
                Assert.Null(reader.GetString("nonexistant"));
                Assert.True(reader.GetBool("loose_property"));
            }
        }

        [Test]
        public void TestOptionTemplateWindows()
        {
            using (var reader = new IniReader("case-sensitivity.ini", IniOptions.Windows))
            {
                Assert.AreEqual(10L, reader.GetInt64("uppercase"));
            }

            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Windows))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("[section]\r\n", read);
            }
        }

        [Test]
        public void TestOptionTemplateUnix()
        {
            using (var reader = new IniReader("case-sensitivity.ini", IniOptions.Unix))
            {
                Assert.Throws<ArgumentException>(() => reader.GetInt64("uppercase"));
            }

            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Unix))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("[section]\n", read);
            }
        }
    }
}
