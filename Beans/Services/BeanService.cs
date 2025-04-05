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

        public void AddBean(Bean bean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(@"
            INSERT INTO Beans (_id, `index`, isBOTD, Cost, Image, colour, `Name`, `Description`, Country)
            VALUES (@_id, @index, @isBOTD, @Cost, @Image, @colour, @Name, @Description, @Country)", connection))
                {
                    command.Parameters.AddWithValue("@_id", bean._id);
                    command.Parameters.AddWithValue("@index", bean.Index);
                    command.Parameters.AddWithValue("@isBOTD", bean.IsBOTD);
                    command.Parameters.AddWithValue("@Cost", bean.Cost);
                    command.Parameters.AddWithValue("@Image", bean.Image ?? string.Empty);
                    command.Parameters.AddWithValue("@colour", bean.Colour ?? string.Empty);
                    command.Parameters.AddWithValue("@Name", bean.Name ?? string.Empty);
                    command.Parameters.AddWithValue("@Description", bean.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@Country", bean.Country ?? string.Empty);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
