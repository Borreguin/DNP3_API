namespace DAL
{
    using System.Data.SQLite;
    using Newtonsoft.Json.Linq;
    using UTILS;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DTO;
    using System;

    public class _DNP3_client_DB
    {
        private readonly SqliteConnection conn = new SqliteConnection();
        private readonly string db_table = "devices";
        private static readonly string KEY_1_ID_MEDIDOR = "Id";
        private static readonly string KEY_2_ID_NAME = "Id_name";
        private static readonly string KEY_3_NAME = "Name";
        private static readonly string KEY_4_DNP3_CONFIG = "DNP3_config";
        private static readonly string KEY_5_NETWORK = "Network_config";
        private static readonly string KEY_6_SERIAL = "Serial_config";
        private static readonly string KEY_7_JSON_INFO = "JSON_info";

        public bool create_table()
        {
            conn.create_db_if_not_exist();

            using (var sqlite = conn.new_connection())
            {
                sqlite.Open();
                string sql = $"create table {db_table} (" +
                    $"{KEY_1_ID_MEDIDOR} INTEGER PRIMARY KEY, " +
                    $"{KEY_2_ID_NAME} TEXT NOT NULL UNIQUE, " +
                    $"{KEY_3_NAME} TEXT NOT NULL UNIQUE, " +
                    $"{KEY_4_DNP3_CONFIG} TEXT, " +
                    $"{KEY_5_NETWORK} TEXT, " +
                    $"{KEY_6_SERIAL} TEXT, " +
                    $"{KEY_7_JSON_INFO} TEXT" +
                    $")";
                using (SQLiteCommand command = new SQLiteCommand(sql, sqlite))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ReturnInfo insert(API_DEVICE_MODEL device_model) {
            try
            {
                GEN_DEVICE device = new GEN_DEVICE()
                {
                    device_name = device_model.device_name,
                    active = device_model.active,
                    dnp3_client_config = device_model.dnp3_client_config,
                    gen_com_network = device_model.gen_com_network,
                    gen_com_serial = device_model.gen_com_serial,
                    device_serie = device_model.device_serie,
                    trace_level = device_model.trace_level,
                    group = device_model.group,
                };
                device.setDeviceCode = device_model.device_code;
                return insert(device);
            }
            catch (Exception ex) {
                return new ReturnInfo()
                {
                    succesful = false,
                    message = $"Device {device_model.device_name} was not inserted",
                    inner_exception = ex
                };
            }
        }

        public ReturnInfo insert(GEN_DEVICE device) {

            try
            {
                using (var sqlite = conn.new_connection())
                {

                    string[] attr_device_config = new string[] { "device_serie", "active", "group", "trace_level"};
                    JObject device_config = CollectionHelper.Get_json_from(device, attr_device_config);

                    string[] attr_dnp3_config = new string[] { "remote_address", "server_address", "integrity_polling_seconds",
                    "class1_polling_seconds", "class2_polling_seconds", "class3_polling_seconds", "comm_media"};
                    JObject dnp3_config = CollectionHelper.Get_json_from(device.dnp3_client_config, attr_dnp3_config);

                    string[] attr_comm_net = new string[] { "ip_address", "port", "protocol" };
                    string[] attr_comm_ser = new string[] { "baud_rate", "data_bit", "parity", "stop_bits", "timeout" };
                    JObject comm_net = CollectionHelper.Get_json_from(device.gen_com_network, attr_comm_net);
                    JObject comm_ser = CollectionHelper.Get_json_from(device.gen_com_serial, attr_comm_ser);

                    JObject JSON_object = new JObject();
                    JSON_object.Add("device_config", device_config);

                    sqlite.Open();
                    string sql = $"INSERT INTO {db_table} " +
                    $"({KEY_2_ID_NAME}, " +
                    $"({KEY_3_NAME}, " +
                    $"{KEY_4_DNP3_CONFIG}, " +
                    $"{KEY_5_NETWORK}, " +
                    $"{KEY_6_SERIAL}, " +
                    $"{KEY_7_JSON_INFO}) " +
                    $"VALUES(" +
                    $" '{device.device_code}'," +
                    $" '{device.device_name}'," +
                    $" '{dnp3_config.ToString()}', " +
                    $" '{((comm_net == null) ? "" : comm_net.ToString())}' , " +
                    $" '{((comm_ser == null) ? "" : comm_ser.ToString())}' , " +
                    $" '{JSON_object.ToString()}' ) ";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                return new ReturnInfo
                {
                    succesful = true,
                    message = $"Device {device.device_name} ha sido ingresado en base de datos",
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
                        message = $"El dispositivo ({device.device_name}, {device.device_code}) ya existe en base de datos",
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

        public void read_by_device_name(string device_name) {
            GEN_DEVICE Device = new GEN_DEVICE();
            DataTable dt = new DataTable();
            try
            {
                using (var sqlite = conn.new_connection())
                {
                    sqlite.Open();
                    string sql = $"SELECT * FROM {db_table} WHERE {KEY_3_NAME} = '{device_name}'";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite))
                    {
                        SQLiteDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        reader.Close();
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            GEN_DEVICE device = CollectionHelper.CreateItem<GEN_DEVICE>(row);
                            Console.WriteLine(row);
                            /*lbl_name = row["name"].ToString();
                            lbl_gender = row["gender"].ToString();
                            lbl_contact = row["contactno"].ToString();*/
                        }
                        //lstDevice = CollectionHelper.ConvertTo<GEN_DEVICE>(dt).ToList();
                        Console.WriteLine(dt);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
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
                    string sql = $"SELECT * FROM {db_table} ORDER BY {KEY_3_NAME}";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite))
                    {
                        SQLiteDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        reader.Close();
                        lstDevice = CollectionHelper.ConvertTo<GEN_DEVICE>(dt).ToList();
                    }
                }
            }
            catch {


            }
            return lstDevice;
        }


    }

}
