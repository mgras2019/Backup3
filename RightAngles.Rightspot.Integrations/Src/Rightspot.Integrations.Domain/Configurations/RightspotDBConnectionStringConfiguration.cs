namespace Rightspot.Integrations.Domain.Configurations;

/// <summary>
/// 
/// </summary>
public class RightspotDBConnectionStringConfiguration
{
    public RightspotDBConnectionStringConfiguration(string value) => Value = value;

    public string Value { get; set; }
}
