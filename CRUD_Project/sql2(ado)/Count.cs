using Microsoft.Data.SqlClient;
public interface ICountEmployees
{
    int Count(SqlConnection con);
}
public class CountEmployees : ICountEmployees
{
    public int Count(SqlConnection con)
    {
        string sql = "SELECT COUNT(*) FROM dbo.Employees";
        using var cmd = new SqlCommand(sql, con);
        return (int)cmd.ExecuteScalar();
    }
}
