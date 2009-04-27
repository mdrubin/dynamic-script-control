import clr

clr.AddReferenceByPartialName("PresentationCore")
clr.AddReferenceByPartialName("PresentationFramework")
clr.AddReferenceByPartialName("WindowsBase")
clr.AddReferenceByPartialName("IronPython")
clr.AddReferenceByPartialName("Microsoft.Scripting")


from System.Windows.Controls import TextBox

class PrefilledTextBox(TextBox):
  def __init__(self):
    self.Text = "I'm prefilled in Python";