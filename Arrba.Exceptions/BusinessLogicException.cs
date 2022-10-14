using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;

namespace Arrba.Exceptions
{
    public class BusinessLogicException : Exception, ISerializable
    {
        public BusinessLogicException()
            :base("General")
        {

        }

        public BusinessLogicException(string message)
            : base(message)
        {

        }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}