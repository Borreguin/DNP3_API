
namespace DTO
{
    using System;
    using System.ComponentModel;
    public class DNP3_CLIENT_CONFIG
    {

        private int _RemoteAddress;
        private int _ServerAddress;
        private CommMediaVal _CommMedia;
        private int _Class1PollingSeconds;
        private int _Class2PollingSeconds;
        private int _Class3PollingSeconds;
        private int _IntegrityPollingSeconds;
        //private bool Authentication;
        //private int UserNumber;
        //private string UpdateKeyHex;


        public enum CommMediaVal
        {
            Network = 0,
            Serial = 1,
        }

        public void Init()
        {
            _RemoteAddress = 100;
            _ServerAddress = 2;
            _Class1PollingSeconds = 0;
            _Class2PollingSeconds = 0;
            _Class3PollingSeconds = 0;
            _IntegrityPollingSeconds = 4;
            _CommMedia = CommMediaVal.Network;
            /*
                Authentication = false;
                UserNumber = 0;
                UpdateKeyHex = "";
             */
        }

        [DisplayName("Remote Address"), Category("3. Slave - Master"), Description("Dirección del dispostivo remoto")]
        public int remote_address
        {
            get
            {
                return _RemoteAddress;
            }
            set
            {
                validate_positive_value(value);
                _RemoteAddress = value;
            }
        }

        [DisplayName("Server Address"), Category("3. Slave - Master"), Description("Dirección del servidor master")]
        public int server_address
        {
            get
            {
                return _ServerAddress;
            }
            set
            {
                validate_positive_value(value);
                _ServerAddress = value;
            }

        }

        [DisplayName("Media "), Category("4. Media"), Description("Asignar medio de comunicación")]
        public CommMediaVal comm_media
        { 
            get { return _CommMedia; }
            set { _CommMedia = value; }
        }

        [DisplayName("Class1 Polling Seconds"), Category("5. Poleo"), Description("Tiempo asignado para realizar el poleo en la clase. Si el valor es cero, la clase no es escaneada")]
        public int class1_polling_seconds
        {
            get
            {
                return _Class1PollingSeconds;
            }
            set
            {
                validate_positive_value(value);
                _Class1PollingSeconds = value;
            }
        }

        [DisplayName("Class2 Polling Seconds"), Category("5. Poleo"), Description("Tiempo asignado para realizar el poleo en la clase. Si el valor es cero, la clase no es escaneada")]
        public int class2_polling_seconds
        {
            get
            {
                return _Class2PollingSeconds;
            }
            set
            {
                validate_positive_value(value);
                _Class2PollingSeconds = value;
            }
        }

        [DisplayName("Class3 Polling Seconds"), Category("5. Poleo"), Description("Tiempo asignado para realizar el poleo en la clase. Si el valor es cero, la clase no es escaneada")]
        public int class3_polling_seconds
        {
            get
            {
                return _Class3PollingSeconds;
            }
            set
            {
                validate_positive_value(value);
                _Class3PollingSeconds = value;
            }
        }

        [DisplayName("Integrity Polling Seconds"), Category("5. Poleo"), Description("Tiempo asignado para realizar el poleo general")]
        public int integrity_polling_seconds
        {
            get
            {
                return _IntegrityPollingSeconds;
            }
            set
            {
                validate_positive_value(value);
                _IntegrityPollingSeconds = value;
            }
        }

        public void validate_positive_value(int to_validate)
        {
            if (to_validate < 0) throw new ArgumentException("El valor debe ser mayor igual a cero");
        }

    }

}
