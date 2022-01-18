using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.General
{
    public static class ExtensionsMethods
    {
        public static string GetDescription(this object value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
