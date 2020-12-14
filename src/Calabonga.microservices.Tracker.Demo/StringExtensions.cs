using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Calabonga.microservices.Tracker.Demo
{
    /// <summary>
    /// Condition class extensions
    /// </summary>
    public static class StringExtensions
    {

        private static readonly Regex WebUrlExpression = new Regex("((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+(:[0-9]+)?|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex EmailExpression = new Regex("^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        [DebuggerStepThrough]
        public static Guid ToGuid(this string value)
        {
            Guid.TryParse(value, out Guid result);
            return result;
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                return WebUrlExpression.IsMatch(target);
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                return EmailExpression.IsMatch(target);
            }

            return false;
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            if (target == null)
            {
                return string.Empty;
            }

            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHtmlExpression.Replace(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T result = defaultValue;
            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    result = (T)Enum.Parse(typeof(T), target.Trim(), ignoreCase: true);
                    return result;
                }
                catch (ArgumentException)
                {
                    return result;
                }
            }

            return result;

        }
    }
}
