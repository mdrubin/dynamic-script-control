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

        public string ToCode(string formatString)
        {
            return string.Format(formatString, Key, string.Format(FormatString, Value));
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