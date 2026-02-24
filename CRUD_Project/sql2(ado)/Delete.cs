using Microsoft.Data.SqlClient;

public interface IDeleteEmployee
{
    void Delete(SqlConnection con, int id);
}

public class DeleteEmployee : IDeleteEmployee
{
    public void Delete(SqlConnection con, int id)
    {
        string sql = "DELETE FROM dbo.Employees WHERE EmployeeId = @id";
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id; //passes value to sql
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Employee Deleted" : "Employee Not Found");
    }
}
