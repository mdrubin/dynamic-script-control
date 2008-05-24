using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Utils;

namespace DynamicScriptControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynamicScriptControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynamicScriptControl;assembly=DynamicScriptControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DynamicScriptControl/>
    ///
    /// </summary>
    public class DynamicScriptControl : Canvas
    {
        private readonly static ScriptRuntime _scriptRuntime;
        private ScriptEngine _scriptEngine;
        private UIElement _content;
        private ScriptScope _scriptScope;
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

def self.dsc_initialize
  h = ##ATTRIBUTES##
  o = ##CLASS##.dsc_new_with_attributes h
  o
end
";

        #region Dependency properties

        /// <summary>
        /// A dependency property that points to Attributes
        /// </summary>
        public static readonly DependencyProperty AttributesProperty;

        /// <summary>
        /// A dependency property that points to the ClassName
        /// </summary>
        public static readonly DependencyProperty ClassNameProperty;

        /// <summary>
        /// A dependency property that points to the ScriptFile property
        /// </summary>
        public static readonly DependencyProperty ScriptFileProperty;

        /// <summary>
        /// A dependency property that points to the ScriptLanguage property
        /// </summary>
        public static readonly DependencyProperty ScriptLanguageProperty;

        /// <summary>
        /// Use this property to tell the dynamic script control which file you want to load.
        /// The class name will tell the control which class to instantiate from that file.
        /// </summary>
        /// <value>The script file.</value>
        public string ScriptFile
        {
            get { return SafeGetValue(ScriptFileProperty, string.Empty); }
            set { SetValue(ScriptFileProperty, value); }
        }

        /// <summary>
        /// By default the <see cref="DynamicScriptControl"/> will try to figure out which 
        /// language it needs to execute the ScriptFile but in case you're using a
        /// non-standard extension you can specify the language here.
        /// </summary>
        /// <value>The script language.</value>
        public string ScriptLanguage
        {
            get { return SafeGetValue(ScriptLanguageProperty, string.Empty); }
            set { SetValue(ScriptLanguageProperty, value); }
        }

        /// <summary>
        /// At this point you still have to specify the class name that we need to 
        /// instantiate. In the future we could use the default naming convention
        /// for a certain language where a file is named after the class we want to use.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName
        {
            get { return SafeGetValue(ClassNameProperty, string.Empty); }
            set { SetValue(ClassNameProperty, value); }
        }

        /// <summary>
        /// This property takes or returns a string which is the hash/dictionary representation.
        /// of the properties you want to set on the wpf control.
        /// </summary>
        /// <value>The attributes.</value>
        public string Attributes
        {
            get
            {
                return SafeGetValue(AttributesProperty, string.Empty);
            }
            set { SetValue(AttributesProperty, value); }
        }

        #endregion

        public T SafeGetValue<T>(DependencyProperty property, T defaultValue)
        {
            var result = GetValue(property);
            return result.IsNull() ? defaultValue : (T) result;
        }

        static DynamicScriptControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DynamicScriptControl),
                                                     new FrameworkPropertyMetadata(typeof (DynamicScriptControl)));
            ScriptFileProperty = DependencyProperty.Register("ScriptFile", typeof (string),
                                                             typeof (DynamicScriptControl));
            ScriptLanguageProperty = DependencyProperty.Register("ScriptLanguage", typeof (string),
                                                                 typeof (DynamicScriptControl));
            ClassNameProperty = DependencyProperty.Register("ClassName", typeof (string), typeof (DynamicScriptControl));
            AttributesProperty = DependencyProperty.Register("Attributes", typeof (string),
                                                             typeof (DynamicScriptControl));

            _scriptRuntime = ScriptRuntime.Create();
        }


        public override void  EndInit()
        {
            ValidateProperties();
            InitializeLanguageEngine();
            EvaluateScript();

            base.EndInit();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Children.Add(_content);
        }

        private void EvaluateScript()
        {
            _scriptEngine = _scriptRuntime.GetEngine(ScriptLanguage);
            _scriptScope = _scriptEngine.CreateScope();
            ScriptLanguage = _scriptEngine.LanguageDisplayName;
            var scriptSource = _scriptEngine.CreateScriptSourceFromFile(ScriptFile);
            scriptSource.Execute(_scriptScope);

            var attrs = Attributes.IsEmpty() ? "{}" : Attributes;
            var initializationScript = RUBY_CODE.Replace("##CLASS##", ClassName).Replace("##ATTRIBUTES##", attrs);
            _scriptScope.Execute(initializationScript);

            _content = _scriptScope.GetVariable<Function<UIElement>>("dsc_initialize")();
        }

        private void InitializeLanguageEngine()
        {
            if(ScriptLanguage.IsEmpty())
            {
                ScriptLanguage = Path.GetExtension(ScriptFile);
            }
           
        }

        private void ValidateProperties()
        {
            if(ScriptFile.IsEmpty())
                throw new MissingMemberException("DynamicScriptControl", "ScriptFile");
            if(ClassName.IsEmpty())
                throw new MissingMemberException("DynamicScriptControl", "ClassName");
        }
    }
}