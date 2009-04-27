require "PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"

include System::Windows::Controls

class PrefilledTextBox < TextBox
  def initialize
    self.text = "I'm prefilled in Ruby"
  end
end
