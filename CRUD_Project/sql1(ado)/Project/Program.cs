using Microsoft.Data.SqlClient;
using DotNetEnv;
using Project;
class Program
{
    static void Main()
    {
        Env.Load(".env");
        string? cs = Environment.GetEnvironmentVariable("DB_CONNECTION");
        using SqlConnection con = new SqlConnection(cs);
        con.Open();
        var db = new DbManager(con);
        var tableHelper = new TableHelper(con);
        while (true)
        {
            Console.WriteLine("==== MENU ====");
            Console.WriteLine("1 Insert (auto create table)");
            Console.WriteLine("2 Select");
            Console.WriteLine("3 Delete");
            Console.WriteLine("4 Count");
            Console.WriteLine("0 Exit");
            string ch = Console.ReadLine();
            if (ch == "0") break;
            Console.Write("Enter table name: ");
            string table = Console.ReadLine();

            if (!tableHelper.TableExists(table))
            {
                Console.WriteLine("Table does not exist!");
                Console.Write("Create table? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                    tableHelper.CreateTableDynamic(table);
                else
                    continue;
            }

            switch (ch)
            {
                case "1":
                    var data = tableHelper.GetRowData(table);
                    db.Insert(table, data);
                    break;
                case "2":
                    db.SelectAll(table);
                    break;
                case "3":
                    Console.Write("Enter Id: ");
                    int id = int.Parse(Console.ReadLine());
                    db.Delete(table, "Id", id);
                    break;
                case "4":
                    Console.WriteLine($"Count = {db.Count(table)}");
                    break;
            }
        }
    }
}