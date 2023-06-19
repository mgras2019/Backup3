namespace Rightspot.Integrations.Infrastructure.SQLServerDB.Repositories;

public class StudentRepository : IStudentRepository
{
    #region " Members ... "

    private readonly RightspotSqlDBContext context;
    protected readonly SystemParameterConfiguration systemParamConfig;

    #endregion

    #region " ctor ... "

    public StudentRepository(RightspotSqlDBContext context, IOptions<SystemParameterConfiguration> options)
    {
        this.context = context;
        this.systemParamConfig = options.Value;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rollNumber"></param>
    /// <returns></returns>
    public async Task<StudentEntity> GetStudentByRollNumberAsync(string rollNumber)
    {
        using var conn = context.CreateConnection();
        var results = await conn.QueryAsync<StudentEntity>("uspGetStudentByRollNumber",
                              commandType: CommandType.Text,
                              commandTimeout: systemParamConfig.SQLCommandTimeoutInSeconds,
                              param: new { @p_RollNumber = rollNumber });
                
        return results.FirstOrDefault()!; 
    }

    public Task<IEnumerable<StudentEntity>> GetStudentsBySchoolAsync(string schoolcode)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEntity> SaveAsync(StudentEntity student)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEntity> DeleteAsync(StudentEntity student)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEntity> DeleteByRollNumberAsync(string rollNumber)
    {
        throw new NotImplementedException();
    }
}