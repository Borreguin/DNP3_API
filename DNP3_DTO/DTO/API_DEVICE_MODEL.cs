﻿namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    public class API_DEVICE_MODEL
    {
        private string _IdDeviceName;
        private string _Serie;
        private LogsType _traceLevel;
        private string _Name;
        private int _GroupScan;
        private bool _Active;
        

        public DNP3_CLIENT_CONFIG dnp3_client_config = new DNP3_CLIENT_CONFIG();
        public DTO_GEN_COM_NETWORK gen_com_network = new DTO_GEN_COM_NETWORK();
        public DTO_GEN_COM_SERIAL gen_com_serial = new DTO_GEN_COM_SERIAL();
        // public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> analogs = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        // public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> counters = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        // public List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>> digitals = new List<KeyValuePair<int, DTO_DNP3_POINT_OBJECT>>();
        // private DNP3_CLIENT_CONFIG _configuration;
        public enum LogsType
        {
            OFF=1,
            INFO=2,
            ERROR=3,
            WARNING=4,
            VERBOSE=5
        }

 
        [Browsable(true), DisplayName("Device Code"), Category("1. Identification"), Description("Valor único identificativo para la creación de tag points")]
        public string device_code
        {
            get
            {
                return _IdDeviceName;
            }
        }

        [Browsable(true), DisplayName("Device _Serie"), Category("1. Identification"), Description("Serie del dispositivo identificativo")]
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

        [DisplayName("Device Name"), Category("1. Identification"), Description("Nombre del dispositivo a configurar. Cambie este valor para identificación del dispositivo.")]
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
        /*
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("3. Device Configuration"), DisplayName("Device Configuration"), Description("Parameters of Device Configuration")]
        public DNP3_CLIENT_CONFIG Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }*/

        [Category("4. Configuration Log"), DisplayName("LogType"), Description("Type Log to save")]
        public string trace_level
        {
            get
            {
                return nameof(_traceLevel);
            }
            set
            {
                _traceLevel = (LogsType)Enum.Parse(typeof(LogsType), value);
            }
        }

        public void Init()
        {
            DateTime ldate = DateTime.Now;

            _Active = true;
            _GroupScan = 100;
            _Name = "New Device";

            _IdDeviceName = getCode(_Name);
            //_configuration = DNP3_CLIENT_CONFIG;
            _traceLevel = LogsType.OFF;
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