﻿using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;

namespace NetCoreServer.Utils
{
    public class MySQLTool
    {
        const string connectionString = "localhost";
        protected MySqlConnection client;

        public void Connect()
        {
            client = new MySqlConnection(connectionString);
        }
        public void Insert() { }
        public void Delete() { }
        public void Update() { }
        public void Query() { }

        public static async Task Main()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Database = "turtle",
                UserID = "root",
                Password = "",
                SslMode = MySqlSslMode.None,
            };

            Debug.Print($"builder.{builder.Server}");

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Debug.Print("Opening connection");
                await conn.OpenAsync();

                //Debug.Print("using command");
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "DROP TABLE IF EXISTS inventory;";
                    await command.ExecuteNonQueryAsync();
                    Debug.Print("Finished dropping table (if existed)");

                    command.CommandText = "CREATE TABLE inventory (id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER);";
                    await command.ExecuteNonQueryAsync();
                    Debug.Print("Finished creating table");

                    command.CommandText = @"INSERT INTO inventory (name, quantity) VALUES (@name1, @quantity1),
                        (@name2, @quantity2), (@name3, @quantity3);";
                    command.Parameters.AddWithValue("@name1", "banana");
                    command.Parameters.AddWithValue("@quantity1", 150);
                    command.Parameters.AddWithValue("@name2", "orange");
                    command.Parameters.AddWithValue("@quantity2", 154);
                    command.Parameters.AddWithValue("@name3", "apple");
                    command.Parameters.AddWithValue("@quantity3", 100);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Debug.Print(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Debug.Print("Closing connection");
            }
        }
        public static async Task TestQuery()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Database = "turtle",
                UserID = "root",
                Password = "",
                SslMode = MySqlSslMode.None,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Debug.Print("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM inventory;";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Debug.Print(string.Format(
                                "Reading from table=({0}, {1}, {2})",
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2)));
                        }
                    }
                }

                Debug.Print("Closing connection");
            }

            Debug.Print("Press RETURN to exit");
        }
    }
}