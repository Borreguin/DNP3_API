
namespace DTO
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;

    public class DTO_GEN_COM_NETWORK
    {
        private ProtocolValues _Protocol;
        private string _IpAddress;
        private int _Port;

        public enum ProtocolValues
        {
            TCP = 0,
            UDP = 1
        }

        public void Init()
        {
            _Protocol = ProtocolValues.TCP;
            _IpAddress = "127.0.0.1";
            _Port = 20000;
        }

        [DisplayName("IPv4"), Category("1. Configurations"), Description("IP Address")]
        public string ip_address
        {
            get
            {
                return _IpAddress;
            }
            set
            {
                if (ValidateIPv4(value))
                {
                    _IpAddress = IPAddress.Parse(value).ToString();
                }
                else
                {
                    throw new ArgumentException("IP no valida. Formato correcto: xxx.xxx.xxx.xxxx");
                }
            }
        }

        [DisplayName("Protocol"), Category("1. Configurations"), Description("Protocol type")]
        public ProtocolValues protocol
        {
            get
            {
                return _Protocol;
            }
            set
            {
                _Protocol = value;
            }
        }

        [DisplayName("Port"), Category("1. Configurations"), Description("Port")]
        public int port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }


        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

    }
}
