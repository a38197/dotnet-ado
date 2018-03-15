using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.Shared
{
    public abstract class BidException : Exception
    {
        public BidException() : base() { }
        public BidException(string message) : base(message) { }
        public BidException(string message, Exception cause) : base(message, cause) { }
    }

    public class ConnectException : BidException
    {
        public ConnectException() : base() { }
        public ConnectException(string message) : base(message) { }
        public ConnectException(string message, Exception cause) : base(message, cause) { }
    }

    public class DisconnectException : BidException
    {
        public DisconnectException() : base() { }
        public DisconnectException(string message) : base(message) { }
        public DisconnectException(string message, Exception cause) : base(message, cause) { }
    }

    public class Utils
    {
        public static Exception whatexception(Exception e)
        {
            Exception ebase = e;
            while (ebase.InnerException != null)
            {
                ebase = ebase.InnerException;
            }
            return ebase;
        }
    }
}
