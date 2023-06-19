namespace Microsoft.CAP.DistributedCache
{
    using Newtonsoft.Json;

    /// <summary>
    /// Cache Application
    /// </summary>
    public class CacheApplication
    {
        /// <summary>
        /// Application Name
        /// </summary>
        [JsonRequired]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Match keys pattern
        /// </summary>
        [JsonRequired]
        public string MatchKeys { get; set; }
    }
}
