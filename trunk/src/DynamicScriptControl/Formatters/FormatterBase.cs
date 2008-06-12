namespace DynamicScriptControl.Formatters
{
    public abstract class FormatterBase : IFormatter
    {
        protected readonly AttributeCollection _attributes;
        protected readonly string _className;

        protected FormatterBase(string className, AttributeCollection attributes)
        {
            _className = className;
            _attributes = attributes;
        }

        #region IFormatter Members

        public abstract string Format();

        #endregion
    }
}