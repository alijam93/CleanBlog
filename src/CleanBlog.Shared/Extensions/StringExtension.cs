using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanBlog.Shared.Extensions
{
    public static class StringExtension
    {
        public static string FriendlyUrl(this string text)
        {
            if (String.IsNullOrEmpty(text)) return "";
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"\-{2,}", "-");

            text = text.ToLower();
            text = Regex.Replace(text, @"&\w+;", "");
            text = Regex.Replace(text, @"[^a-z0-9\-\s]", "");
            text = text.Replace(' ', '-');
            text = Regex.Replace(text, @"-{2,}", "-");
            text = text.TrimStart(new[] { '-' });
            if (text.Length > 80)
                text = text.Substring(0, 79);
            text = text.TrimEnd(new[] { '-' });
            return text;
        }

    }
}
