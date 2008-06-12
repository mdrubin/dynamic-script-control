#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using DynamicScriptControl.Formatters;
using Microsoft.Scripting.Hosting;
using DynamicScriptControl.DLR;

#endregion

namespace DynamicScriptControl
{
    public class ScriptConverter : IMultiValueConverter
    {



        #region Implementation of IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var scriptFile = values[0].ToString();
            var className = values[1].ToString();
            var attributes = (AttributeCollection)values[2];
            var scriptLanguage = values[3].ToString();

            if (scriptLanguage.IsEmpty()) scriptLanguage = Path.GetExtension(scriptFile);

            var engine = DynamicScriptControl.ScriptRuntime.GetEngine(scriptLanguage);
            
            var scriptScope = engine.CreateScope();
            var scriptSource = engine.CreateScriptSourceFromFile(scriptFile);
            scriptSource.Execute();
            
            var script = scriptScope.CreateFormatter(className, attributes).Format();
            
            return scriptScope.Execute<UIElement>(script);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("We can't convert back to script at this point");
        }

        #endregion
    }

    namespace DLR
    {
        public static class Extensions
        {
            public static IFormatter CreateFormatter(this ScriptScope scriptScope, string className, AttributeCollection attributes)
            {
                switch (scriptScope.Engine.LanguageDisplayName.ToLowerInvariant())
                {
                    case "IronPython":
                        throw new NotImplementedException("IronPython support hasn't been implemented yet.");
                    default:
                        return new RubyFormatter(className, attributes);
                }
            }
        }
    }
}
