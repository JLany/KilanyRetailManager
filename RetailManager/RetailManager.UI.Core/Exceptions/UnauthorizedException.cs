using System;
using System.Collections.Generic;
using System.Text;

namespace RetailManager.UI.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() 
        { 

        }

        public UnauthorizedException(string message) : base(message) 
        {

        }

        public UnauthorizedException(string message,  Exception inner) : base(message, inner)
        {

        }
    }
}
