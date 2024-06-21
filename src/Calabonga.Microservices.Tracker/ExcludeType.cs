namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// // Calabonga: Summary required (ExcludedItem 2024-06-21 08:50)
    /// </summary>
    internal enum ExcludeType
    {
        Path,
        Scheme,
        Host
    }

    public enum CheckExcludeType
    {
        Contains,
        Equality,
        StartWith
    }
}