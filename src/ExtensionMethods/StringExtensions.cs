using System;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value) => value is null || value == string.Empty;

        public static byte[] ToBytes(this string value)
        {
            if (value.IsNullOrEmpty()) 
                throw new ArgumentNullException(nameof(value));

            return Encoding.UTF8.GetBytes(value);
        }
    }
}