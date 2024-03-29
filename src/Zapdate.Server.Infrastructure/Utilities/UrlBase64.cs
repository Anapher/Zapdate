using System;
using System.Collections.Generic;

namespace Zapdate.Server.Infrastructure.Utilities
{
    //https://github.com/neosmart/UrlBase64/blob/master/UrlBase64/UrlBase64.cs
    public static class UrlBase64
    {
        private static readonly char[] TwoPads = {'=', '='};

        public static string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static byte[] Decode(string encoded)
        {
            var chars = new List<char>(encoded.ToCharArray());

            for (var i = 0; i < chars.Count; ++i)
                if (chars[i] == '_')
                    chars[i] = '/';
                else if (chars[i] == '-')
                    chars[i] = '+';

            switch (encoded.Length % 4)
            {
                case 2:
                    chars.AddRange(TwoPads);
                    break;
                case 3:
                    chars.Add('=');
                    break;
            }

            var array = chars.ToArray();

            return Convert.FromBase64CharArray(array, 0, array.Length);
        }
    }
}