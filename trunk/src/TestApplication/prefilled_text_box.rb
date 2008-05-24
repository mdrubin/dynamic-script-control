require "PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"

include System::Windows::Controls

#class Object
#  def self.dsc_new_with_attributes(hash = {})
#    raise ArgumentError('I need a  hash to intialize my properties from') unless hash.is_a? Hash
#    new.initialize_from_hash(hash)     
#  end
#  
#  def initialize_from_hash(hash)
#    hash.each do |k, v|
#            instance_variable_set("@#{k.to_s}", v)
#        end    
#        self
#    end    
#end
#
#def self.dsc_initialize
#  h = eval(attrs)
#  o = PrefilledTextBox.dsc_new_with_attributes h
#  o
#      end

class PrefilledTextBox < TextBox
  
  def initialize
    self.text = "I'm prefilled"
  end
  
end