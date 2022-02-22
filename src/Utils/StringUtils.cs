using System;
using System.Collections.Generic;
using System.Text;

namespace GitFolks.Utils
{
    public static class StringUtils
    {
        public static string RemoveLineBreak(this string value)
        {
            return value.Replace("\n", string.Empty).Replace("\r", string.Empty);
        }
    }
}
