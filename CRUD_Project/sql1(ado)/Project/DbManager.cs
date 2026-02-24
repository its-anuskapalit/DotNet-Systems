using Microsoft.Data.SqlClient;
using System.Data;
namespace Project{
public class DbManager
{
    private readonly SqlConnection _con;
    public DbManager(SqlConnection con)
    {
        _con = con;
    }
    public void Insert(string tableName, Dictionary<string, object> data)
    {
        string columns = string.Join(",", data.Keys);
        string parameters = string.Join(",", data.Keys.Select(k => "@" + k));
        string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
        using var cmd = new SqlCommand(sql, _con);
        foreach (var item in data)
            cmd.Parameters.AddWithValue("@" + item.Key, item.Value ?? DBNull.Value);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Inserted successfully");
    }
    public void Update(string tableName, string idColumn, object idValue, Dictionary<string, object> data)
    {
        string setClause = string.Join(",", data.Keys.Select(k => $"{k}=@{k}"));
        string sql = $"UPDATE {tableName} SET {setClause} WHERE {idColumn}=@id";
        using var cmd = new SqlCommand(sql, _con);
        foreach (var item in data)
            cmd.Parameters.AddWithValue("@" + item.Key, item.Value ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", idValue);
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Updated" : "Not Found");
    }
    public void Delete(string tableName, string idColumn, object idValue)
    {
        string sql = $"DELETE FROM {tableName} WHERE {idColumn}=@id";
        using var cmd = new SqlCommand(sql, _con);
        cmd.Parameters.AddWithValue("@id", idValue);
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Deleted" : "Not Found");
    }
    public void SelectAll(string tableName)
    {
        string sql = $"SELECT * FROM {tableName}";
        using var cmd = new SqlCommand(sql, _con);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($"{reader[i]} | ");

            Console.WriteLine();
        }
    }
    public int Count(string tableName)
    {
        string sql = $"SELECT COUNT(*) FROM {tableName}";
        using var cmd = new SqlCommand(sql, _con);
        return (int)cmd.ExecuteScalar();
    }
}}