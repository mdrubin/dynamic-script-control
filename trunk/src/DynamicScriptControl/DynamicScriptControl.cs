#region Usings

using System;
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

        public static readonly DependencyProperty AttributesProperty =
            DependencyProperty.Register(
                "Attributes",
                typeof (AttributeCollection),
                typeof (DynamicScriptControl),
                new UIPropertyMetadata(new AttributeCollection())
                );

        public static readonly DependencyProperty ClassNameProperty =
            DependencyProperty.Register("ClassName", typeof (string), typeof (DynamicScriptControl),
                                        new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ScriptFileProperty =
            DependencyProperty.Register("ScriptFile", typeof (string), typeof (DynamicScriptControl),
                                        new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ScriptLanguageProperty =
            DependencyProperty.Register("ScriptLanguage", typeof (string), typeof (DynamicScriptControl),
                                        new UIPropertyMetadata(string.Empty));

        #endregion

        public DynamicScriptControl()
        {
            Attributes = new AttributeCollection();
        }

        #region Instance properties

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName
        {
            get { return (string) GetValue(ClassNameProperty); }
            set { SetValue(ClassNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the script language.
        /// </summary>
        /// <value>The script language.</value>
        public string ScriptLanguage
        {
            get { return (string) GetValue(ScriptLanguageProperty); }
            set { SetValue(ScriptLanguageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the script file.
        /// </summary>
        /// <value>The script file.</value>
        public string ScriptFile
        {
            get { return (string) GetValue(ScriptFileProperty); }
            set { SetValue(ScriptFileProperty, value); }
        }


        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public AttributeCollection Attributes
        {
            get { return (AttributeCollection) GetValue(AttributesProperty); }
            set { SetValue(AttributesProperty, value); }
        }

        #endregion
    }
}