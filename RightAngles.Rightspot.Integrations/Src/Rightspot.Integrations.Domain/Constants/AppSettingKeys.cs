namespace Rightspot.Integrations.Domain.Constants;

/// <summary>
/// 
/// </summary>
public partial class RightspotIntegrationDomainConstants
{
    /// <summary>
    /// Class to hold constants for Application Settings (local.settings.json)
    /// </summary>
    public static class AppSettingKeys
    {
        /// <summary>
        /// constant for the Logging Section
        /// </summary>
        public const string LOGGING_SECTION = "Logging";
        /// <summary>
        /// constant for the ConnectionString Section
        /// </summary>
        public const string RIGHTSPOT_SQL_DB_CONNECTION_STRING_SECTION = "RightspotDBConnectionStrings";
        /// <summary>
        /// constant for the HttpClientPolicies Section
        /// </summary>
        public const string HTTP_CLIENT_POLICIES_SECTION = "HttpClientPolicies";
        /// <summary>
        /// constant for the System Parameters Section
        /// </summary>
        public const string SYSTEM_PARAMETER_SECTION = "SystemParameters";
    }
}
