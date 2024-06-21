namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:46)
    /// </summary>
    internal class ExcludedItem
    {
        public ExcludedItem(ExcludeType excludeType, string value, CheckExcludeType checkType)
        {
            ExcludeType = excludeType;
            Value = value;
            CheckType = checkType;
        }

        /// <summary>
        /// Checking type
        /// </summary>
        public CheckExcludeType CheckType { get; set; }

        /// <summary>
        /// Term to search for skip
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Type of exclude item
        /// </summary>
        public ExcludeType ExcludeType { get; }
    }
}
