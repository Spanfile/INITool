using System.IO;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace INITool.Tests
{
    [TestFixture]
    public class WriterTests
    {
        [Test]
        public void TestBasicWrite()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.AddProperty("value", 10L);
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("value=10\n", read);
            }
        }

        [Test]
        public void TestSectionWrite()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.StartSection("section");
                writer.AddProperty("value", 10L);
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("[section]\nvalue=10\n", read);
            }
        }

        [Test]
        public void TestPropertyComment()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.AddProperty("value", 10L, "property");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("#property\nvalue=10\n", read);
            }
        }

        [Test]
        public void TestSectionComment()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.StartSection("section", "section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("#section\n[section]\n", read);
            }
        }

        [Test]
        public void TestLineEndingLF()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.LF)))
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

        [Test]
        public void TestLineEndingCR()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.CR)))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.AreEqual("[section]\r", read);
            }
        }

        [Test]
        public void TestLineEndingCRLF()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.CRLF)))
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
    }
}
