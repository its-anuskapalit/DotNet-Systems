using Microsoft.Data.SqlClient;
public interface IUpdateEmployee
{
    void UpdateSalary(SqlConnection con, int id, decimal newSalary);
}
public class UpdateEmployee : IUpdateEmployee
{
    public void UpdateSalary(SqlConnection con, int id, decimal newSalary)
    {
        string sql = "UPDATE dbo.Employees SET Salary = @salary WHERE EmployeeId = @id";
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
        var salaryParam = cmd.Parameters.Add("@salary", System.Data.SqlDbType.Decimal);
        salaryParam.Precision = 10;
        salaryParam.Scale = 2;
        salaryParam.Value = newSalary;
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Salary Updated\n" : "Employee Not Found\n");
    }
}
