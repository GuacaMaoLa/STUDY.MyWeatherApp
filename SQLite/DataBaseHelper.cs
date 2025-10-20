using System.Data.SQLite;
using System.IO;

namespace LEARN_MVVM.SQLite
{
    /// <summary>
    /// Helper class to create a databank for the weather app
    /// </summary>
    public static class DataBaseHelper
    {
        private readonly static string connetionString = @"Data Source =..\..\..\SQLite\WeatherAppManagementSystem.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(@"..\..\..\SQLite\WeatherAppManagementSystem.db"))
            {
                SQLiteConnection.CreateFile(@"..\..\..\SQLite\WeatherAppManagementSystem.db");

                using (var connection = new SQLiteConnection(connetionString))
                {
                    connection.Open();

                    // Create table for Weather App Data
                    string createTemperatureTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Temperatures (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            City TEXT NOT NULL,
                            Temp DECIMAL(2,2),
                            Date DATETIME
                        );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createTemperatureTableQuery;
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
        }
    }
}
