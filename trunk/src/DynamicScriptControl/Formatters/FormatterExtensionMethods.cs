using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DynamicScriptControl.Formatters
{
    public static class FormatterExtensionMethods
    {
        public static string ToCode(this ObservableCollection<Attribute> dict, Dictionary<AttributeType, string> mapping)
        {
            var sb = new StringBuilder();
            foreach (var attribute in dict)
            {
                sb.AppendLine(attribute.ToCode(mapping[attribute.AttributeType]));
            }
            return sb.ToString();
        }
    }
}