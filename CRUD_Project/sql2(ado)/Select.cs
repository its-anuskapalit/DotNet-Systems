using Microsoft.Data.SqlClient;
public interface IGetEmployees
{
    void GetAll(SqlConnection con);
}
public class GetEmployees : IGetEmployees
{
    public void GetAll(SqlConnection con)
    {
        string sql = @"SELECT EmployeeId, FullName, Department, Salary FROM dbo.Employees ORDER BY EmployeeId";
        using var cmd = new SqlCommand(sql, con);
        using var reader = cmd.ExecuteReader();
        Console.WriteLine("Employee List:\n");
        while (reader.Read())
        {
            Console.WriteLine($"{reader["EmployeeId"]} | {reader["FullName"]} | {reader["Department"]} | {reader["Salary"]}");
        }
    }
}
