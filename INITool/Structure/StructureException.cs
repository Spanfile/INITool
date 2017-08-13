using System;
using System.Runtime.Serialization;

namespace INITool.Structure
{
    [Serializable]
    public class StructureException : Exception
    {
        public StructureException()
        {
        }

        public StructureException(string message) : base(message)
        {
        }

        public StructureException(string message, Exception inner) : base(message, inner)
        {
        }

        protected StructureException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
