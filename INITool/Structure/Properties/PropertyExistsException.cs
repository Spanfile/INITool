using System;
using System.Runtime.Serialization;

namespace INITool.Structure.Properties
{
    [Serializable]
    public class PropertyExistsException : StructureException
    {
        public PropertyExistsException(string propertyName, string sectionName) : base($"property {propertyName} already exists in {sectionName}")
        {
        }

        public PropertyExistsException(string propertyName, string sectionName, Exception inner) : base($"property {propertyName} already exists in {sectionName}", inner)
        {
        }

        protected PropertyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
