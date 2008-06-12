using System.Collections.Generic;

namespace DynamicScriptControl.Formatters
{
    public abstract class FormatterBase : IFormatter
    {
        protected readonly string _className;
        protected readonly AttributeCollection _attributes;

        protected FormatterBase(string className, AttributeCollection attributes)
        {
            _className = className;
            _attributes = attributes;
        }

        public abstract string Format();
    }
}