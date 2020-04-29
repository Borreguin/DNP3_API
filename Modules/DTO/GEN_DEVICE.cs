namespace DTO
{   /*
        Esta clase realiza las operaciones CRUD relativas a API_DEVICE_MODEL
     */
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    public class GEN_DEVICE
    {
        private string _IdDeviceName;
        private string _Serie;
        private int _traceLevel;
        private string _Name;
        private int _GroupScan;
        private bool _Active;
        private DNP3_CLIENT_CONFIG _configuration;

        public DNP3_CLIENT_CONFIG dnp3_client_config = new DNP3_CLIENT_CONFIG();
        public DTO_GEN_COM_NETWORK gen_com_network = new DTO_GEN_COM_NETWORK();
        public DTO_GEN_COM_SERIAL gen_com_serial = new DTO_GEN_COM_SERIAL();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> analogs = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> counters = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> digitals = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        
        public enum LogsType
        {
            OFF=1,
            INFO=2,
            ERROR=3,
            WARNING=4,
            VERBOSE=5
        }

        public string setDeviceCode {
            set {
                _IdDeviceName = value;
            }
        }
 

        [Browsable(true), DisplayName("Device Code"), Category("1. Identification"), Description("Valor único identificativo para la creación de tag points")]
        public string device_code
        {
            get
            {
                return _IdDeviceName;
            }
        }

        [Browsable(true), DisplayName("Device _Serie"), Category("1. Identification"), Description("_Serie del dispositivo identificativo")]
        public string device_serie
        {
            get
            {
                return _Serie;
            }
            set
            {
                _Serie = value;
            }
        }

        [DisplayName("Device _Name"), Category("1. Identification"), Description("Nombre del dispositivo a configurar. Cambie este valor para identificación del dispositivo.")]
        public string device_name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value.Length > 4)
                {
                    _Name = value;
                }
                else
                {
                    throw new ArgumentException("El nombre del dispositivo debe tener al menos 4 caracteres");
                }
                
            }
        }

        [DisplayName("Active"), Category("2. Scanning"), Description("Activa/desactiva un dispositivo para el escaneo.")]
        public bool active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
            }

        }

        [Category("2. Scanning"), Description("Grupo de escaneo al que pertenece.")]
        public int group
        {
            get
            {
                return _GroupScan;
            }
            set
            {
                _GroupScan = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("3. Device Configuration"), DisplayName("Device Configuration"), Description("Parameters of Device Configuration")]
        public DNP3_CLIENT_CONFIG configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        [Category("4. Configuration Log"), DisplayName("LogType"), Description("Type Log to save")]
        public TraceLevel trace_level
        {
            get
            {
                return (TraceLevel)_traceLevel;
            }
            set
            {
                _traceLevel = (int)value;
            }
        }

        public void Init()
        {
            DateTime ldate = DateTime.Now;

            _Active = true;
            _GroupScan = 100;
            _Name = "New Device";

            _IdDeviceName = getCode();
            _configuration = new DNP3_CLIENT_CONFIG();
            _traceLevel = (int)TraceLevel.Off;
        }

        public string getCode() {
            string id = _Name.ToUpper() + DateTime.Now.ToString();
            string hashedData = ComputeMD5Hash(id);
            return hashedData;
        }

        static string ComputeMD5Hash(string rawData)
        {
            // Create a MD5 HASH   
            using (MD5 MD5Hash = MD5.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
