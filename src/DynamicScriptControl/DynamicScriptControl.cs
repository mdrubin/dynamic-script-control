#region Usings

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting.Hosting;

#endregion

namespace DynamicScriptControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynamicScriptContentControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynamicScriptContentControl;assembly=DynamicScriptContentControl"
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
    public class DynamicScriptControl : Control
    {

        #region Private static fields

        private static ScriptRuntime _scriptRuntime;

        public static ScriptRuntime ScriptRuntime
        {
            get
            {
                if (_scriptRuntime == null)
                    _scriptRuntime = ScriptRuntime.Create();
                return _scriptRuntime;
            }
            set { _scriptRuntime = value; }
        }

        #endregion

        #region Type initializer

        static DynamicScriptControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DynamicScriptControl),
                                                     new FrameworkPropertyMetadata(typeof (DynamicScriptControl)));
        }

        #endregion

        #region Dependency properties

        // Using a DependencyProperty as the backing store for Attributes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttributesProperty =
            DependencyProperty.Register("Attributes", typeof (string), typeof (DynamicScriptControl), new UIPropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for ScriptFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScriptFileProperty =
            DependencyProperty.Register("ScriptFile", typeof(string), typeof(DynamicScriptControl), new UIPropertyMetadata(string.Empty));
        // Using a DependencyProperty as the backing store for ScriptLanguage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScriptLanguageProperty =
            DependencyProperty.Register("ScriptLanguage", typeof(string), typeof(DynamicScriptControl), new UIPropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for ClassName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClassNameProperty =
            DependencyProperty.Register("ClassName", typeof(string), typeof(DynamicScriptControl), new UIPropertyMetadata(string.Empty));

        #endregion

        #region Instance properties

        public string ClassName
        {
            get { return (string)GetValue(ClassNameProperty); }
            set { SetValue(ClassNameProperty, value); }
        }

        public string ScriptLanguage
        {
            get { return (string)GetValue(ScriptLanguageProperty); }
            set { SetValue(ScriptLanguageProperty, value); }
        }

        public string ScriptFile
        {
            get { return (string)GetValue(ScriptFileProperty); }
            set { SetValue(ScriptFileProperty, value); }
        }

        
        public string Attributes
        {
            get { return (string) GetValue(AttributesProperty); }
            set { SetValue(AttributesProperty, value); }
        }

        #endregion
        
    }
}