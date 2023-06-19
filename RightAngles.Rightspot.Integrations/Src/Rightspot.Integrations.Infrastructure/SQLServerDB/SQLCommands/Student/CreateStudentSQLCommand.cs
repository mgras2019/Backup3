namespace Rightspot.Integrations.Infrastructure.SQLServerDB.SQLCommands.Student;

public class CreateStudentSQLCommand
{
    private readonly string _sqlCommand = @"usp_insert_student";

    public CreateStudentSQLCommand() { }

    public override string ToString()
    {
        return _sqlCommand;
    }
}

 