using System;

namespace Arrba.Exceptions
{
    [Serializable]
    public class BusinessCriticalLogicException : BusinessLogicException
    {
        public BusinessCriticalLogicException() { }
        public BusinessCriticalLogicException(string message) : base(message) { }
        public BusinessCriticalLogicException(string message, Exception inner) : base(message, inner) { }
        protected BusinessCriticalLogicException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}