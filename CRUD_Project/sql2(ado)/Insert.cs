using Microsoft.Data.SqlClient;
public interface IInsertEmployee
{
    void Insert(SqlConnection con, string name, string dept, decimal salary);
}
public class InsertEmployee : IInsertEmployee
{
    public void Insert(SqlConnection con, string name, string dept, decimal salary)
    {
        string sql = @"INSERT INTO dbo.Employees (FullName, Department, Salary) VALUES (@name, @dept, @salary)";
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 100).Value = name;
        cmd.Parameters.Add("@dept", System.Data.SqlDbType.NVarChar, 50).Value = dept;
        var salaryParam = cmd.Parameters.Add("@salary", System.Data.SqlDbType.Decimal);
        salaryParam.Precision = 10;
        salaryParam.Scale = 2;
        salaryParam.Value = salary;
        cmd.ExecuteNonQuery();
        Console.WriteLine("Employee Inserted Successfully\n");
    }
}
