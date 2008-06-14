namespace DynamicScriptControl
{
    public enum AttributeType
    {
        Default,
#if IRA
        Resource,
#endif
        Number,
        Date,
        Text
    }
}