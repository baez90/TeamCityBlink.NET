using System;
using System.Runtime.Serialization;

namespace TCBlink.NET.Common.Exceptions
{
    public class TCConectionFailException : Exception
    {
        public TCConectionFailException()
        {
        }

        public TCConectionFailException(string message) : base(message)
        {
        }

        public TCConectionFailException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TCConectionFailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}