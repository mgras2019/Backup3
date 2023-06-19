namespace Rightspot.Integrations.Domain.Configurations;

/// <summary>
/// 
/// </summary>
public class SystemParameterConfiguration
{
    public int SQLCommandTimeoutInSeconds { get; set; }
    public int SqlConnectionRetryCount { get; set; }
    public int SqlConnectionRetryIntervalInSeconds { get; set; }
    public int BulkCopyTimeOutInSeconds { get; set; }
    public int BulkCopyBatchSize { get; set; }
    public int TransScopeTimeOutInMinutes { get; set; }
}
