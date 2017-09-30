using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using INITool.Parser.Units;

namespace INITool
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an error that occurs when reading an unexpected or otherwise invalid unit
    /// </summary>
    [Serializable]
    public class InvalidUnitException : Exception
    {
        internal InvalidUnitException(Unit unit, params Type[] expectedUnits)
            : base($"invalid unit {unit}, expected one from {string.Join(",", expectedUnits.Select(t => t.Name))}")
        {
        }

        internal InvalidUnitException(Unit unit, IEnumerable<Type> expectedUnits, Exception inner)
            : base($"invalid unit {unit}, expected one from {string.Join(",", expectedUnits.Select(t => t.Name))}", inner)
        {
        }

        protected InvalidUnitException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}