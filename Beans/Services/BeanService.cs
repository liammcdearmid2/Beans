using Beans.Models;
using MySql.Data.MySqlClient;

namespace Beans.Services
{
    public class BeanService
    {
        private readonly string _connectionString;

        public BeanService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Bean> GetBeans()
        {
            var beans = new List<Bean>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Beans", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        beans.Add(new Bean
                        {
                            _id = reader.GetString("_id"),
                            Index = reader.GetInt32("index"),
                            IsBOTD = reader.GetBoolean("isBOTD"),
                            Cost = reader.GetDecimal("Cost"),
                            Image = reader.GetString("Image"),
                            Colour = reader.GetString("colour"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Country = reader.GetString("Country")
                        });
                    }
                }
            }

            return beans;
        }
    }
}
