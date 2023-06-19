namespace Rightspot.Integrations.Infrastructure.SQLServerDB;

/// <summary>
/// 
/// </summary>
public class RightspotSqlDBContext
{
	private readonly IConfiguration config;
	private readonly string connString;

	public RightspotSqlDBContext(IConfiguration config)
	{
		this.config = config;
		connString = this.config.GetConnectionString(RightspotIntegrationDomainConstants.AppSettingKeys.RIGHTSPOT_SQL_DB_CONNECTION_STRING_SECTION);
	}

	public IDbConnection CreateConnection() => new SqlConnection(connString);
}
