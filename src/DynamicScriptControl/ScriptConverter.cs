#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using DynamicScriptControl.DLR;
using DynamicScriptControl.Formatters;
using Microsoft.Scripting.Hosting;

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
            var attributes = (AttributeCollection) values[2];
            var scriptLanguage = values[3].ToString();

            if (scriptLanguage.IsEmpty()) scriptLanguage = Path.GetExtension(scriptFile);

            var engine = DynamicScriptControl.ScriptRuntime.GetEngineByFileExtension(scriptLanguage);

            var scriptScope = engine.CreateScope();
            var scriptSource = engine.CreateScriptSourceFromFile(scriptFile);
            scriptSource.Execute();

            var script = scriptScope.CreateFormatter(className, attributes).Format();

            return engine.Execute<UIElement>(script, scriptScope);
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
            public static IFormatter CreateFormatter(this ScriptScope scriptScope, string className,
                                                     AttributeCollection attributes)
            {
                if (scriptScope.Engine.Setup.Names.Contains("Python")) return new PythonFormatter(className, attributes);
                
                return new RubyFormatter(className, attributes);
            }
        }
    }
}