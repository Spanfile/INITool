using System;
using System.Runtime.Serialization;

namespace INITool.Structure.Sections
{
    [Serializable]
    public class SectionNotFoundException : StructureException
    {
        public SectionNotFoundException(string sectionName) : base($"section not found: {sectionName}")
        {
        }

        public SectionNotFoundException(string sectionName, Exception inner) : base($"section not found: {sectionName}", inner)
        {
        }

        protected SectionNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
