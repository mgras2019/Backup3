namespace Rightspot.Integrations.Infrastructure.SQLServerDB.Contracts;

/// <summary>
/// 
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Fetch student by roll number
    /// </summary>
    /// <param name="rollNumber"></param>
    /// <returns>Returns student entity</returns>
    Task<StudentEntity> GetStudentByRollNumberAsync(string rollNumber);
    /// <summary>
    /// Fetch students By school
    /// </summary>
    /// <returns>Returns students collection</returns>
    Task<IEnumerable<StudentEntity>> GetStudentsBySchoolAsync(string schoolcode);
    /// <summary>
    /// Save Student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task<StudentEntity> SaveAsync(StudentEntity student);
    /// <summary>
    /// Delete Student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task<StudentEntity> DeleteAsync(StudentEntity student);
    /// <summary>
    /// Delete Student by roll number.
    /// </summary>
    /// <param name="rollnumber"></param>
    /// <returns></returns>
    Task<StudentEntity> DeleteByRollNumberAsync(string rollNumber);
}

