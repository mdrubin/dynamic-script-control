#region Usings

using System.Collections.ObjectModel;
using System.Text;
using DynamicScriptControl.Formatters.ExtensionsForRuby;

#endregion

namespace DynamicScriptControl.Formatters
{
    public class RubyFormatter : FormatterBase
    {
        #region Ruby code

        private const string RUBY_CODE = @"class ##CLASS##

    alias_method :old_initialize, :initialize
    def initialize
        old_initialize
        ##ATTRIBUTES##       
    end    
end

##CLASS##.new

";

        #endregion

        public RubyFormatter(string className, AttributeCollection attributes)
            : base(className, attributes)
        {
        }

        public override string Format()
        {
            var attrs = _attributes.ToCode();

            return RUBY_CODE.Replace("##CLASS##", _className).Replace("##ATTRIBUTES##", attrs);
        }
    }

    namespace ExtensionsForRuby
    {
        public static class ExtensionMethods
        {
            public static string ToCode(this ObservableCollection<Attribute> dict)
            {
                var sb = new StringBuilder();
                foreach (var attribute in dict)
                {
                    sb.AppendLine(attribute.ToCode("self.{0} = {1}"));
                }
                return sb.ToString();
            }
        }
    }
}