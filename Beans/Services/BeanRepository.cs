using Beans.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace Beans.Services
{
    public class BeanRepository : IBeanRepository
    {
        private readonly string _connectionString;

        // Constructor to inject the connection string
        public BeanRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get a single bean by ID
        public Bean GetBeanById(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Beans WHERE _id = @id";
                return connection.QuerySingleOrDefault<Bean>(query, new { id });
            }
        }

        // Add a new bean
        public Bean AddBean(Bean bean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Beans (_id, `index`, isBOTD, Cost, Image, colour, `Name`, `Description`, Country)
                          VALUES (@_id, @Index, @IsBOTD, @Cost, @Image, @Colour, @Name, @Description, @Country)";
                connection.Execute(query, bean);
                return bean; // Return the inserted bean (with the ID, if it's auto-generated)
            }
        }

        // Update an existing bean
        public Bean UpdateBean(Bean bean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE Beans SET 
                          `Name` = @Name, 
                          Cost = @Cost, 
                          Image = @Image, 
                          colour = @Colour, 
                          `Description` = @Description, 
                          Country = @Country
                          WHERE _id = @Id";
                connection.Execute(query, bean);
                return bean; // Return the updated bean
            }
        }

        // Delete a bean by ID
        public bool DeleteBean(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Beans WHERE _id = @id";
                var result = connection.Execute(query, new { id });
                return result > 0; // Returns true if rows are affected (deleted)
            }
        }
    }
}
