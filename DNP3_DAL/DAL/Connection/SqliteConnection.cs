using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace DAL.Connection
{
    public class SqliteConnection
    {
        private string DB_name = "devices.db";

        public bool create_db_if_not_exist() {
            string event_db_path = Path.Combine(Directory.GetCurrentDirectory(), "Database", DB_name);

            if (!System.IO.File.Exists(event_db_path))
            {
                SQLiteConnection.CreateFile(event_db_path);
                return true;
            }
            return false;
        }

        public SQLiteConnection new_connection() {
            return new SQLiteConnection("Data Source=" + DB_name);
        }
        
    }   
}
