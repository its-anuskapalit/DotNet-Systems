using Microsoft.Data.SqlClient;
namespace Project{
public class TableCreator
{
    public void CreateEmployeeTable(SqlConnection con)
    {
        string sql = @"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
        CREATE TABLE Employees
        (
            EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
            FullName NVARCHAR(100),
            Department NVARCHAR(50),
            Salary DECIMAL(10,2)
        )";

        using var cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Table ensured");
    }
}
}