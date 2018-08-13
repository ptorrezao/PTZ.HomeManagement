using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PTZ.HomeManagement.Models
{
    public class EmailSenderException : Exception
    {
        public EmailSenderException(string message) : base(message)
        {
        }
    }
}
