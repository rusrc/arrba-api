using System;
using System.Runtime.Serialization;

namespace Arrba.Extensions.Exceptions
{
    [Serializable]
    public class ExtentionsException : Exception
    {
        public ExtentionsException()
        {
        }

        public ExtentionsException(string message) : base(message)
        {
        }

        public ExtentionsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExtentionsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}