namespace DynamicScriptControl
{
    public static class ExtensionMethods
    {
        public static bool IsEmpty(this string self)
        {
            return string.IsNullOrEmpty(self) || self.Trim().Length == 0;
        }
    }
}