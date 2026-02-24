using Microsoft.Data.SqlClient;
namespace Project{
public class TableHelper
{
    private readonly SqlConnection _con;
    public TableHelper(SqlConnection con)
    {
        _con = con;
    }
    public bool TableExists(string tableName)
    {
        string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table";
        using var cmd = new SqlCommand(sql, _con);
        cmd.Parameters.AddWithValue("@table", tableName);
        return (int)cmd.ExecuteScalar() > 0;
    }
    public void CreateTableDynamic(string tableName)
    {
        Console.Write("How many columns (excluding Id)? ");
        int n = int.Parse(Console.ReadLine());
        List<string> columns = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Column {i + 1} name: ");
            string colName = Console.ReadLine();
            Console.Write($"Data type for {colName} (int/nvarchar/decimal): ");
            string type = Console.ReadLine();
            if (type == "nvarchar")
                columns.Add($"{colName} NVARCHAR(100)");
            else if (type == "decimal")
                columns.Add($"{colName} DECIMAL(10,2)");
            else
                columns.Add($"{colName} INT");
        }

        string sql = $@"CREATE TABLE {tableName}
        (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            {string.Join(",", columns)}
        )";
        using var cmd = new SqlCommand(sql, _con);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Table created successfully!");
    }
    public Dictionary<string, object> GetRowData(string tableName)
    {
        string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @table
                       AND COLUMN_NAME <> 'Id'";

        using var cmd = new SqlCommand(sql, _con);
        cmd.Parameters.AddWithValue("@table", tableName);
        using var reader = cmd.ExecuteReader();
        var data = new Dictionary<string, object>();
        while (reader.Read())
        {
            string col = reader.GetString(0);
            Console.Write($"Enter {col}: ");
            data[col] = Console.ReadLine();
        }
        return data;
    }
}}