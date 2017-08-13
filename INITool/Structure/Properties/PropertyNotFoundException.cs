using System;
using System.Runtime.Serialization;

namespace INITool.Structure.Properties
{
    [Serializable]
    public class PropertyNotFoundException : StructureException
    {
        public PropertyNotFoundException(string property, string section) : base($"property {property} not found in {section}")
        {
        }

        public PropertyNotFoundException(string property, string section, Exception inner) : base($"property {property} not found in {section}", inner)
        {
        }

        protected PropertyNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
