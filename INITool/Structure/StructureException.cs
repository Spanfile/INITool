using System;
using System.Runtime.Serialization;

namespace INITool.Structure
{
    [Serializable]
    public class StructureException : Exception
    {
        internal StructureException()
        {
        }

        internal StructureException(string message) : base(message)
        {
        }

        internal StructureException(string message, Exception inner) : base(message, inner)
        {
        }

        protected StructureException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
