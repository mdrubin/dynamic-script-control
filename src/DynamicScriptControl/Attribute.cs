#region Usings

using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

namespace DynamicScriptControl
{
    public class Attribute : INotifyPropertyChanged
    {
        private string _formatString;
        private string _key;
        private string _value;
        private AttributeType _attributeType = AttributeType.Default;

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>The type of the attribute.</value>
        public AttributeType AttributeType
        {
            get { return _attributeType; }
            set
            {
                if (_attributeType == value) return;
                _attributeType = value;
                OnPropertyChanged("AttributeType");
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString
        {
            get
            {
                if (_formatString.IsEmpty())
                    _formatString = "{0}";
                return _formatString;
            }
            set
            {
                if (_formatString == value) return;
                _formatString = value;
                OnPropertyChanged("FormatString");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Returns this attributes code representation.
        /// </summary>
        /// <param name="codeTemplate">The code template.</param>
        /// <returns></returns>
        public string ToCode(string codeTemplate)
        {
            return string.Format(codeTemplate, Key, string.Format(FormatString, Value));
        }

        /// <summary>
        /// Fires the event for the property when it changes.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AttributeCollection : ObservableCollection<Attribute>
    {
    }
}