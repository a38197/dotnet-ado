using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.ConnectedEnvironment.Tables
{
    public class ConvertUtils
    {
        public static string GetByteString(byte[] source)
        {
            return Enumerable.Select(source
                , b => GetByteString(b)
                ).Aggregate((s1, s2) => s1 + s2);
        }

        public static string GetByteString(byte source)
        {
            char[] chars = new char[] {
                (source & 1) > 0 ? '1' : '0',
                (source & 2) > 0 ? '1' : '0',
                (source & 4) > 0 ? '1' : '0',
                (source & 8) > 0 ? '1' : '0',
                (source & 16) > 0 ? '1' : '0',
                (source & 32) > 0 ? '1' : '0',
                (source & 64) > 0 ? '1' : '0',
                (source & 128) > 0 ? '1' : '0'
            };

            return new string(chars);
        }
    }
}
