using System.Collections.Generic;

namespace DynamicScriptControl.Formatters
{
    public class PythonFormatter : FormatterBase
    {
        #region Python code

        private const string PYTHON_CODE = @"class ##CLASS##2(##CLASS##):
   def __init__(self):
      ##CLASS##.__init__(self);
      ##ATTRIBUTES##      

dynamicControl = ##CLASS##2();";

        #endregion

        #region Attribute Mapping

        private static readonly Dictionary<AttributeType, string> _attributeTypeMapping = new Dictionary<AttributeType, string>
                                                                                         {
                                                                                             { AttributeType.Default, "self.{0} = {1}" },
                                                                                             { AttributeType.Text, "self.{0} = '{1}'" },
#if IRA
                                                                                             { AttributeType.Resource, "self.{0} = DynamicScriptControl::Workarounds.get_app_resource '{1}'" },
#endif
                                                                                             { AttributeType.Number, "self.{0} = {1}" },
                                                                                             { AttributeType.Date, "self.{0} = Time.parse '{1}'" }
                                                                                         };


        #endregion

        public PythonFormatter(string className, AttributeCollection attributes)
            : base(className, attributes)
        {
        }

        public override string Format()
        {
            var attrs = _attributes.ToCode(_attributeTypeMapping);

            return PYTHON_CODE.Replace("##CLASS##", _className).Replace("##ATTRIBUTES##", attrs);
        }
    }
}