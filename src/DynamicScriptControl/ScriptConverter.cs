#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

#endregion

namespace DynamicScriptControl
{
    public class ScriptConverter : IMultiValueConverter
    {

        #region Ruby code

        private const string RUBY_CODE = @"class Object
    def self.dsc_new_with_attributes(hash = {})
        raise ArgumentError('I need a  hash to intialize my properties from') unless hash.is_a? Hash
        new.initialize_from_hash(hash)     
    end

    def initialize_from_hash(hash)
        hash.each do |k, v|
            instance_variable_set(" + "\"@#{k.to_s}\", v)" + @"
        end    
        self
    end    
end

#def self.dsc_initialize
  h = ##ATTRIBUTES##
  o = ##CLASS##.dsc_new_with_attributes h
  o
#end
";

        #endregion

        #region Implementation of IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var scriptFile = values[0].ToString();
            var className = values[1].ToString();
            var attributes = values[2].ToString();
            var scriptLanguage = values[3].ToString();

            if (scriptLanguage.IsEmpty()) scriptLanguage = Path.GetExtension(scriptFile);

            var engine = DynamicScriptControl.ScriptRuntime.GetEngine(scriptLanguage);
            var scriptScope = engine.CreateScope();
            var scriptSource = engine.CreateScriptSourceFromFile(scriptFile);
            scriptSource.Execute();

            var attrs = attributes.IsEmpty() ? "{}" : attributes;

            var initializationScript = RUBY_CODE.Replace("##CLASS##", className).Replace("##ATTRIBUTES##", attrs);
            return scriptScope.Execute<UIElement>(initializationScript);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("We can't convert back to script at this point");
        }

        #endregion


    }
}