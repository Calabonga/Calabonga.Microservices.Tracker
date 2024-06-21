using System;
using System.Collections.Generic;
using System.Linq;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
    /// </summary>
    public class ExcludeOptions
    {
        public ExcludeOptions()
        {
            Excludes = new List<ExcludedItem>();
        }

        /// <summary>
        /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
        /// </summary>
        internal List<ExcludedItem> Excludes { get; }

        /// <summary>
        /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkType"></param>
        public ExcludeOptions AddPathExcludes(string value, CheckExcludeType checkType)
        {
            if (Excludes.FirstOrDefault(x => x.ExcludeType == ExcludeType.Path && x.Value == value) is null)
            {
                Excludes.Add(new ExcludedItem(ExcludeType.Path, value, checkType));
            }

            return this;
        }

        /// <summary>
        /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkType"></param>
        public ExcludeOptions AddSchemeExcludes(string value, CheckExcludeType checkType)
        {
            if (Excludes.FirstOrDefault(x => x.ExcludeType == ExcludeType.Scheme && x.Value == value) is null)
            {
                Excludes.Add(new ExcludedItem(ExcludeType.Scheme, value, checkType));
            }

            return this;
        }

        /// <summary>
        /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkType"></param>
        public ExcludeOptions AddHostExcludes(string value, CheckExcludeType checkType)
        {
            if (Excludes.FirstOrDefault(x => x.ExcludeType == ExcludeType.Host && x.Value == value) is null)
            {
                Excludes.Add(new ExcludedItem(ExcludeType.Host, value, checkType));
            }

            return this;
        }

        /// <summary>
        /// Checks value from exclude options contains in Host
        /// </summary>
        /// <param name="value"></param>
        internal bool CheckHostContainsValue(string value)
        {
            return IsContains(ExcludeType.Host, value);
        }

        /// <summary>
        /// Checks value from exclude options contains in Scheme
        /// </summary>
        /// <param name="value"></param>
        internal bool CheckSchemeContainsValue(string value)
        {
            return IsContains(ExcludeType.Scheme, value);
        }

        /// <summary>
        /// Checks value from exclude options contains in Path
        /// </summary>
        /// <param name="value"></param>
        internal bool CheckPathContainsValue(string value)
        {
            return IsContains(ExcludeType.Path, value);
        }

        /// <summary>
        /// Checks value from exclude options contains
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private bool IsContains(ExcludeType type, string value)
        {
            foreach (var item in Excludes)
            {
                if (item.ExcludeType != type)
                {
                    continue;
                }

                switch (item.CheckType)
                {
                    case CheckExcludeType.Contains:
                        return value.Contains(item.Value, StringComparison.InvariantCultureIgnoreCase);

                    case CheckExcludeType.Equality:
                        return value.Equals(item.Value, StringComparison.InvariantCultureIgnoreCase);

                    case CheckExcludeType.StartWith:
                        return value.StartsWith(item.Value, StringComparison.InvariantCultureIgnoreCase);

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            return false;
        }
    }
}