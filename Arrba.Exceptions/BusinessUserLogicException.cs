using System;
using System.Runtime.Serialization;

namespace Arrba.Exceptions
{
    public class BusinessUserLogicException : BusinessLogicException, ISerializable
    {
        public BusinessUserLogicException()
            :base()
        {

        }

        public BusinessUserLogicException(string message)
            : base(message)
        {

        }

        public BusinessUserLogicException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected BusinessUserLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}