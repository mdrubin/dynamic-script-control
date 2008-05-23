using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

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
    public class DynamicScriptControl : ContentControl
    {
        private readonly static ScriptRuntime _scriptRuntime;
        private ScriptEngine _scriptEngine;
        private ScriptSource _scriptSource;

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
            get { return GetValue(ScriptFileProperty).ToString(); }
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
            get { return GetValue(ScriptLanguageProperty).ToString(); }
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
            get { return GetValue(ClassNameProperty).ToString(); }
            set { SetValue(ClassNameProperty, value); }
        }

        /// <summary>
        /// This property takes or returns a string which is the hash/dictionary representation.
        /// of the properties you want to set on the wpf control.
        /// </summary>
        /// <value>The attributes.</value>
        public string Attributes
        {
            get { return GetValue(AttributesProperty).ToString(); }
            set { SetValue(AttributesProperty, value); }
        }

        #endregion


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


        protected override void OnInitialized(EventArgs e)
        {
            ValidateProperties();

            InitializeLanguageFromExtension();
            base.OnInitialized(e);

            _scriptEngine = _scriptRuntime.GetEngine("rb");
            ScriptLanguage = _scriptEngine.LanguageDisplayName;
            _scriptSource = _scriptEngine.CreateScriptSourceFromFile(ScriptFile);
            Content = _scriptSource.Execute();
        }

        private void InitializeLanguageFromExtension()
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