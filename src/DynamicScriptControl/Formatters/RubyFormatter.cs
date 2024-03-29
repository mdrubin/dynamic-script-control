#region Usings

using System.Collections.Generic;

#endregion

namespace DynamicScriptControl.Formatters
{
    public class RubyFormatter : FormatterBase
    {
        #region Ruby code

        private const string RUBY_CODE =
            @"class ##CLASS##

    alias_method :old_initialize, :initialize
    def initialize
        old_initialize
        ##ATTRIBUTES##       
    end    
end

##CLASS##.new

";

        #endregion

        private static readonly Dictionary<AttributeType, string> _attributeTypeMapping = new Dictionary<AttributeType, string>
                                                                                              {
                                                                                                  {AttributeType.Default, "self.{0} = {1}"},
                                                                                                  {AttributeType.Text, "self.{0} = '{1}'"},
#if IRA
                                                                                             { AttributeType.Resource, "self.{0} = DynamicScriptControl::Workarounds.get_app_resource '{1}'" },
#endif
                                                                                                  {AttributeType.Number, "self.{0} = {1}"},
                                                                                                  {AttributeType.Date, "self.{0} = Time.parse '{1}'"}
                                                                                              };

        public RubyFormatter(string className, AttributeCollection attributes)
            : base(className, attributes)
        {
        }

        public override string Format()
        {
            var attrs = _attributes.ToCode(_attributeTypeMapping);

            return RUBY_CODE.Replace("##CLASS##", _className).Replace("##ATTRIBUTES##", attrs);
        }
    }
}