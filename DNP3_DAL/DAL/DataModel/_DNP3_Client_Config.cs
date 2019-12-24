namespace DAL.DataModel
{
    using DAL.Connection;
    using System.Data.SQLite;
    using DTO;
    using Newtonsoft.Json.Linq;
    using UTILS;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    class _DNP3_Client_Config
    {
        private SqliteConnection conn = new SqliteConnection();
        private string db_table = "devices";
        private static string KEY_1_ID_MEDIDOR = "Id";
        private static string KEY_2_NAME = "Name";
        private static string KEY_3_DNP3_CONFIG = "DNP3_config";
        private static string KEY_4_NETWORK = "Network_config";
        private static string KEY_5_SERIAL = "Serial_config";
        private static string KEY_6_JSON_info = "JSON_info";

        public bool create_table()
        {
            conn.create_db_if_not_exist();

            using (var sqlite = conn.new_connection())
            {
                sqlite.Open();
                string sql = $"create table {db_table} (" +
                    $"{KEY_1_ID_MEDIDOR} INTEGER PRIMARY KEY, " +
                    $"{KEY_2_NAME} TEXT NOT NULL UNIQUE, " +
                    $"{KEY_3_DNP3_CONFIG} TEXT, " +
                    $"{KEY_4_NETWORK} TEXT, " +
                    $"{KEY_5_SERIAL} TEXT, " +
                    $"{KEY_6_JSON_info} TEXT" +
                    $")";
                SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                command.ExecuteNonQuery();
            }
            return true;
        }

        public ReturnInfo insert(GEN_DEVICE device) {

            try
            {
                using (var sqlite = conn.new_connection())
                {
                    string[] attr_dnp3_config = new string[] { "Remote_Address", "Server_Address", "Integrity_PollingSeconds",
                    "Class1_PollingSeconds", "Class2_PollingSeconds", "Class3_PollingSeconds", "Autentication",
                    "Update_KeyHex", "Media", "User_Number"};
                    JObject dnp3_config = CollectionHelper.Get_json_from(device.DNP3_CLIENT_CONFIG, attr_dnp3_config);

                    string[] attr_comm_net = new string[] {"IP", "_Port", "_Protocol" };
                    string[] attr_comm_ser = new string[] {"BaudRate", "DataBit", "Parity", "StopBits", "Timeout_Value"};
                    JObject comm_net = CollectionHelper.Get_json_from(device.GEN_COM_NETWORK, attr_comm_net);
                    JObject comm_ser = CollectionHelper.Get_json_from(device.GEN_COM_SERIAL, attr_comm_ser);

                    JObject JSON_object = new JObject();

                    sqlite.Open();
                    string sql = $"INSERT INTO {db_table} " +
                    $"({KEY_2_NAME}, " +
                    $"{KEY_3_DNP3_CONFIG}, " +
                    $"{KEY_4_NETWORK}, " +
                    $"{KEY_5_SERIAL}, " +
                    $"{KEY_6_JSON_info}) " +
                    $"VALUES(" +
                    $" '{device.DeviceName}'," +
                    $" '{dnp3_config.ToString()}', " +
                    $" '{((comm_net == null) ? "" : comm_net.ToString())}' , " +
                    $" '{((comm_ser == null) ? "" : comm_ser.ToString())}' , " +
                    $" '{JSON_object.ToString()}' ) ";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
                return new ReturnInfo
                {
                    succesful = true,
                    message = "El dispositivo ha sido ingresado en base de datos",
                    inner_exception = null
                };
            }
            catch (SQLiteException e)
            {
                // Si el registro ya existe, este se vuelve a actualizar
                if (e.Message.ToUpper().Contains("UNIQUE CONSTRAINT"))
                {
                    return new ReturnInfo
                    {
                        succesful = false,
                        message = "El dispositivo ya ha sido ingresado en base de datos",
                        inner_exception = null
                    };
                }
                // Si la tabla no existe, esta se crea en el momento:
                if (e.Message.ToUpper().Contains("NO SUCH TABLE: " + db_table.ToUpper()))
                {
                    create_table();
                    return insert(device);
                }
                return new ReturnInfo
                {
                    succesful = false,
                    message = "No es posible ingresar el evento. " + e.Message,
                    inner_exception = e
                };
            }
        }


        public IEnumerable<GEN_DEVICE> read_all() {
            List<GEN_DEVICE> lstDevice = new List<GEN_DEVICE>();
            DataTable dt = new DataTable();
            try
            {
                using (var sqlite = conn.new_connection())
                {
                    sqlite.Open();
                    string sql = $"SELECT * FROM {db_table} ORDER BY {KEY_2_NAME}";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    SQLiteDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    lstDevice = CollectionHelper.ConvertTo<GEN_DEVICE>(dt).ToList();
                }

            }
            catch {


            }
            return lstDevice;
        }


    }

}
