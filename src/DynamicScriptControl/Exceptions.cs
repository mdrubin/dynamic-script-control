using System;
using System.Runtime.Serialization;

namespace DynamicScriptControl
{
    public class UnkownLanguageExtensionException : Exception
    {
        public UnkownLanguageExtensionException()
        {
        }

        public UnkownLanguageExtensionException(string message) : base(message)
        {
        }

        public UnkownLanguageExtensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnkownLanguageExtensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}