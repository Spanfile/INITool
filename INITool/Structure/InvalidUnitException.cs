using System;
using System.Runtime.Serialization;
using INITool.Parser.Units;

namespace INITool.Structure
{
    [Serializable]
    internal class InvalidUnitException : StructureException
    {
        public InvalidUnitException(Unit unit) : base($"invalid unit: {unit}")
        {
        }

        public InvalidUnitException(Unit unit, Exception inner) : base($"invalid unit: {unit}", inner)
        {
        }

        protected InvalidUnitException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}