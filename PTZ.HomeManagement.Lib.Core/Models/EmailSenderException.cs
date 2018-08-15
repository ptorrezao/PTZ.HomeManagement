using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PTZ.HomeManagement.Models
{
    [Serializable]
    public class EmailSenderException : Exception
    {
        public EmailSenderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public EmailSenderException(string message) : base(message)
        {
        }
    }
}
