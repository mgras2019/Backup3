namespace Rightspot.Integrations.Infrastructure.SQLServerDB.SQLCommands.Student;

public class UpdateStudentSQLCommand
{
    private readonly string _sqlCommand = @"usp_update_student";

    public UpdateStudentSQLCommand() { }

    public override string ToString()
    {
        return _sqlCommand;
    }
}

