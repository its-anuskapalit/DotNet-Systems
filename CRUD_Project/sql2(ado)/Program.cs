using Microsoft.Data.SqlClient;
using System;
using DotNetEnv;
class Program
{
    static void Main()
    {
        Env.Load(".env");
        string? cs = Environment.GetEnvironmentVariable("DB_CONNECTION");
        //string cs = "Server=POLLY\\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        using SqlConnection con = new SqlConnection(cs);
        con.Open();
        IInsertEmployee insertService = new InsertEmployee();
        IUpdateEmployee updateService = new UpdateEmployee();
        ICountEmployees countService = new CountEmployees();
        IGetEmployees getService = new GetEmployees();
        IDeleteEmployee deleteService = new DeleteEmployee();

        bool service = true;

        while (service)
        {
            Console.WriteLine("\n====== EMPLOYEE MENU ======");
            Console.WriteLine("1. Insert Employee");
            Console.WriteLine("2. Update Salary");
            Console.WriteLine("3. Count Employees");
            Console.WriteLine("4. View Employees");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("0. Exit");
            Console.Write("Select Option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Enter Department: ");
                    string dept = Console.ReadLine() ?? "";

                    Console.Write("Enter Salary: ");
                    decimal salary;
                    while (!decimal.TryParse(Console.ReadLine(), out salary))
                    {
                        Console.Write("Invalid salary. Enter again: ");
                    }
                    insertService.Insert(con, name, dept, salary);
                    break;
                case "2":
                    Console.Write("Enter EmployeeId: ");
                    int updateId;
                    while (!int.TryParse(Console.ReadLine(), out updateId))
                    {
                        Console.Write("Invalid ID. Enter again: ");
                    }
                    Console.Write("Enter New Salary: ");
                    decimal newSalary;
                    while (!decimal.TryParse(Console.ReadLine(), out newSalary))
                    {
                        Console.Write("Invalid salary. Enter again: ");
                    }
                    updateService.UpdateSalary(con, updateId, newSalary);
                    break;
                case "3":
                    int total = countService.Count(con);
                    Console.WriteLine($"Total Employees: {total}");
                    break;
                case "4":
                    getService.GetAll(con);
                    break;
                case "5":
                    Console.Write("Enter EmployeeId to Delete: ");
                    int deleteId;
                    while (!int.TryParse(Console.ReadLine(), out deleteId))
                    {
                        Console.Write("Invalid ID. Enter again: ");
                    }
                    deleteService.Delete(con, deleteId);
                    break;
                case "0":
                    service = false;
                    Console.WriteLine("Exiting Application...");
                    break;
                default:
                    Console.WriteLine("Invalid Option. Try Again.");
                    break;
            }
        }
    }
}




