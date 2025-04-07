using Beans.Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;

namespace Beans.Services
{
    public class BeanRepository : IBeanRepository
    {
        private readonly string _connectionString;

        //Constructor to inject the connection string
        public BeanRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Get All Beans
        public async Task<IEnumerable<Bean>> GetAllBeans()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Beans";

                return await connection.QueryAsync<Bean>(query);
            }
        }

        //Get a single bean by ID
        public Bean GetBeanById(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Beans WHERE _id = @id";
                return connection.QuerySingleOrDefault<Bean>(query, new { id });
            }
        }

        //Add a new bean
        public Bean AddBean(Bean bean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Beans (_id, `index`, isBOTD, Cost, Image, colour, `Name`, `Description`, Country)
                          VALUES (@_id, @Index, @IsBOTD, @Cost, @Image, @Colour, @Name, @Description, @Country)";
                connection.Execute(query, bean);
                return bean;
            }
        }

        //Update an existing bean
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
                return bean; 
            }
        }

        //Delete a bean by ID
        public bool DeleteBean(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Beans WHERE _id = @id";
                var result = connection.Execute(query, new { id });
                return result > 0; // Returns true if rows are deleted
            }
        }

        //Bean of the day logic:

        //Set the winning bean as bean of the day
        public void UpdateBeanAsBOTD(string id, bool isBOTD, DateTime previousWinnerDate)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"UPDATE Beans 
                  SET isBOTD = @isBOTD, 
                      PreviousWinnerDate = @PreviousWinnerDate 
                  WHERE _id = @Id";

            connection.Execute(query, new { Id = id, isBOTD, PreviousWinnerDate = previousWinnerDate });
        }

        //Retrieve BOTD
        public Bean GetPreviousBOTD()
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "SELECT * FROM Beans WHERE isBOTD = TRUE LIMIT 1";
            return connection.QuerySingleOrDefault<Bean>(query);
        }
        //Reset to ensure only one bean set as BOTD
        public void ResetBOTD()
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "UPDATE Beans SET isBOTD = FALSE";
            connection.Execute(query);
        }
    }
}
