using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCaraTest.util
{
    public static class Extension
    {
        public static void AppendLine(this StringBuilder builder, string str, bool app)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine(str);
            }
            else
            {
                builder.Append(str);
            }
        }
        public static bool IsEmpty(this string str)
        {
            if (str == null || str.Length == 0)
                return true;
            else
                return false;
        }

        public static byte[] Slice(this byte[] arr, int length)
        {
            byte[] result = new byte[length];
            Buffer.BlockCopy(arr, 0, result, 0, length);
            return result;
        }
        public static string Clear(this string str)
        {
            if (str != null)
                str = str.Remove(str.Length - 1);
            else
                str = "";
            return str;
        }
    }
}
