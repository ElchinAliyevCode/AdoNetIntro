using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNetIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=PizzaMizza;Trusted_Connection=true;TrustServerCertificate=true");
            connection.Open();
        start:
            Console.WriteLine("PizzaMizza");
            Console.WriteLine("1) Pizzalara bax");
            Console.WriteLine("2) Pizza elave et");
            Console.WriteLine("3) Pizza sil");
            Console.Write("Secim daxil edin: ");
            var choice = Console.ReadLine();
            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    ShowPizzasInfo(connection);
                    Console.WriteLine();
                    break;

                case "2":
                nameInput:
                    Console.Write("Enter pizza name: ");
                    var pizzaName = Console.ReadLine();
                    if (pizzaName == null)
                    {
                        Console.WriteLine("Please enter correct name!");
                        goto nameInput;
                    }
                priceInput:
                    Console.Write("Enter price: ");
                    var pizzaPrice = Console.ReadLine();
                    var isParsed = decimal.TryParse(pizzaPrice, out decimal price);
                    if (!isParsed)
                    {
                        Console.WriteLine("Please enter correct price!");
                        goto priceInput;
                    }
                countInput:
                    Console.Write("Enter ingridient count: ");
                    var pizzaIngridientCount = Console.ReadLine();
                    var isParsedInt = int.TryParse(pizzaIngridientCount, out int count);
                    if (!isParsedInt)
                    {
                        Console.WriteLine("Please enter correct count!");
                        goto countInput;
                    }

                    SqlCommand addPizza = new SqlCommand($"INSERT INTO Pizzas VALUES('{pizzaName}',{pizzaPrice},{pizzaIngridientCount})", connection);

                    var insertResult = addPizza.ExecuteNonQuery();

                    if (insertResult > 0)
                    {
                        Console.WriteLine("Pizza added successfully");
                        Console.WriteLine();
                    }

                    break;

                case "3":
                    ShowPizzasInfo(connection);
                idInput:
                    Console.Write("Enter id for deletion: ");
                    var idForDelete = Console.ReadLine();
                    var idParsed = int.TryParse(idForDelete, out int id);
                    if (!idParsed)
                    {
                        Console.WriteLine("Enter valid Id!");
                        goto idInput;
                    }

                    SqlCommand deletePizza = new SqlCommand($"DELETE FROM Pizzas WHERE Id={id}", connection);

                    var deletedResult = deletePizza.ExecuteNonQuery();

                    if (deletedResult == 0)
                    {
                        Console.WriteLine("Id not found!");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Pizza deleted successfully");
                        Console.WriteLine();
                    }
                    break;
                default:
                    break;
            }
            goto start;
        }

        private static void ShowPizzasInfo(SqlConnection connection)
        {
            SqlCommand getAllPizzas = new SqlCommand("SELECT * FROM Pizzas", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(getAllPizzas);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Console.WriteLine($"{row["Id"]}. - {row["Name"]} - {row["Price"]} - {row["IngridientCount"]}");
            }
        }
    }
}
