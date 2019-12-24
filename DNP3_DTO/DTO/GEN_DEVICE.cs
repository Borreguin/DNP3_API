namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    public class GEN_DEVICE
    {
        private int IdDevice;
        private string IdDeviceName;
        private string Serie;
        private int traceLevel;
        private string Name;
        private int GroupScan;
        private bool Active;
        private DNP3_CLIENT_CONFIG configuration;

        public DNP3_CLIENT_CONFIG DNP3_CLIENT_CONFIG = new DNP3_CLIENT_CONFIG();
        public DTO_GEN_COM_NETWORK GEN_COM_NETWORK = new DTO_GEN_COM_NETWORK();
        public DTO_GEN_COM_SERIAL GEN_COM_SERIAL = new DTO_GEN_COM_SERIAL();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> analogs = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> counters = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> digitals = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        
        //public enum LogsType
        //{
        //    OFF=1,
        //    INFO=2,
        //    ERROR=3,
        //    WARNING=4,
        //    VERBOSE=5
        //}
        public string setDeviceCode {
            set {
                IdDeviceName = value;
            }
        }
 
        [Browsable(false), DisplayName("Device ID"), Category("1. Identification"), Description("Id del dispositivo a configurar. Cambie este valor para identificación del dispositivo.")]
        public int _IdDevice
        {
            get
            {
                return IdDevice;
            }
            set
            {
                IdDevice = value;
            }
        }

        [Browsable(true), DisplayName("Device Code"), Category("1. Identification"), Description("Valor único identificativo para la creación de tag points")]
        public string DeviceCode
        {
            get
            {
                return IdDeviceName;
            }
        }

        [Browsable(true), DisplayName("Device _Serie"), Category("1. Identification"), Description("_Serie del dispositivo identificativo")]
        public string DeviceSerie
        {
            get
            {
                return Serie;
            }
            set
            {
                Serie = value;
            }
        }

        [DisplayName("Device _Name"), Category("1. Identification"), Description("Nombre del dispositivo a configurar. Cambie este valor para identificación del dispositivo.")]
        public string DeviceName
        {
            get
            {
                return Name;
            }
            set
            {
                if (value.Length > 4)
                {
                    Name = value;
                }
                else
                {
                    throw new ArgumentException("El nombre del dispositivo debe tener al menos 4 caracteres");
                }
                
            }
        }

        [DisplayName("_Active"), Category("2. Scanning"), Description("Activa/desactiva un dispositivo para el escaneo.")]
        public bool _Active
        {
            get
            {
                return Active;
            }
            set
            {
                Active = value;
            }

        }

        [Category("2. Scanning"), Description("Grupo de escaneo al que pertenece.")]
        public int Group
        {
            get
            {
                return GroupScan;
            }
            set
            {
                GroupScan = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("3. Device Configuration"), DisplayName("Device Configuration"), Description("Parameters of Device Configuration")]
        public DNP3_CLIENT_CONFIG Configuration
        {
            get
            {
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }

        [Category("4. Configuration Log"), DisplayName("LogType"), Description("Type Log to save")]
        public TraceLevel Trace_Level
        {
            get
            {
                return (TraceLevel)traceLevel;
            }
            set
            {
                traceLevel = (int)value;
            }
        }

        public void Init()
        {
            DateTime ldate = DateTime.Now;

            Active = true;
            GroupScan = 100;
            Name = "New Device";

            IdDeviceName = getCode(Name);
            configuration = DNP3_CLIENT_CONFIG;
            traceLevel = (int)TraceLevel.Off;
        }

        public string getCode(string Name) {
            Name = Name.Replace(" ", "*").ToUpper();
            DateTime ldate = DateTime.Now;
            int n = Math.Min(10, Name.Length);
            if (n < 10) {
                int r = 10 - n;
                for (int i = 0; i <= r; i++) {
                    Name += "_";
                }
            }
            string n_str = Name.Substring(0, 5) +
                Name.Substring((int) (n/2), 5) +
                Name.Substring(Name.Length-5);
            n_str = "DNP-" + n_str.ToUpper() + "-" + (int)(ldate.Year + ldate.Month*1000 + ldate.Day*1000 + ldate.Hour*100 + ldate.Minute*10 + ldate.Second + ldate.Millisecond/100);
            return n_str;
        }

    }
}
