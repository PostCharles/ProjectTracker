using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Exceptions
{
    public class SecretNotFoundException : Exception
    {
        public SecretNotFoundException()
        {
        }

        public SecretNotFoundException(string? message) : base(message)
        {
        }

        public SecretNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SecretNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
