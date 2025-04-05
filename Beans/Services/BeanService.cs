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

        public void AddBean(CreateBean createBean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(@"
            INSERT INTO Beans (_id, `index`, isBOTD, Cost, Image, colour, `Name`, `Description`, Country)
            VALUES (@_id, @index, @isBOTD, @Cost, @Image, @colour, @Name, @Description, @Country)", connection))
                {
                    command.Parameters.AddWithValue("@_id", createBean._id);
                    command.Parameters.AddWithValue("@index", createBean.Index);
                    command.Parameters.AddWithValue("@isBOTD", createBean.IsBOTD);
                    command.Parameters.AddWithValue("@Cost", createBean.Cost);
                    command.Parameters.AddWithValue("@Image", createBean.Image ?? string.Empty);
                    command.Parameters.AddWithValue("@colour", createBean.Colour ?? string.Empty);
                    command.Parameters.AddWithValue("@Name", createBean.Name);
                    command.Parameters.AddWithValue("@Description", createBean.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@Country", createBean.Country ?? string.Empty);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UpdateBean(string id, UpdateBean updateBean)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var fieldsToUpdate = new List<string>();
                var command = new MySqlCommand();
                command.Connection = connection;

                if (updateBean.Index.HasValue)
                {
                    fieldsToUpdate.Add("`index` = @index");
                    command.Parameters.AddWithValue("@index", updateBean.Index.Value);
                }

                if (updateBean.IsBOTD.HasValue)
                {
                    fieldsToUpdate.Add("isBOTD = @isBOTD");
                    command.Parameters.AddWithValue("@isBOTD", updateBean.IsBOTD.Value);
                }

                if (updateBean.Cost.HasValue)
                {
                    fieldsToUpdate.Add("Cost = @Cost");
                    command.Parameters.AddWithValue("@Cost", updateBean.Cost.Value);
                }

                if (!string.IsNullOrEmpty(updateBean.Image))
                {
                    fieldsToUpdate.Add("Image = @Image");
                    command.Parameters.AddWithValue("@Image", updateBean.Image);
                }

                if (!string.IsNullOrEmpty(updateBean.Colour))
                {
                    fieldsToUpdate.Add("colour = @colour");
                    command.Parameters.AddWithValue("@colour", updateBean.Colour);
                }

                if (!string.IsNullOrEmpty(updateBean.Name))
                {
                    fieldsToUpdate.Add("`Name` = @Name");
                    command.Parameters.AddWithValue("@Name", updateBean.Name);
                }

                if (!string.IsNullOrEmpty(updateBean.Description))
                {
                    fieldsToUpdate.Add("`Description` = @Description");
                    command.Parameters.AddWithValue("@Description", updateBean.Description);
                }

                if (!string.IsNullOrEmpty(updateBean.Country))
                {
                    fieldsToUpdate.Add("Country = @Country");
                    command.Parameters.AddWithValue("@Country", updateBean.Country);
                }

                if (fieldsToUpdate.Count == 0)
                {
                    return false;
                }

                command.Parameters.AddWithValue("@_id", id);
                var updateClause = string.Join(", ", fieldsToUpdate);
                command.CommandText = $"UPDATE Beans SET {updateClause} WHERE _id = @_id";

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DeleteBean(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("DELETE FROM Beans WHERE _id = @_id", connection);
                command.Parameters.AddWithValue("@_id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
