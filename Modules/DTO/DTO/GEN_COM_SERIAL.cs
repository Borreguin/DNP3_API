
namespace DTO
{
    using System;
    using System.ComponentModel;

    public class DTO_GEN_COM_SERIAL
    {

        private int _dataBit { get; set; }
        private int _parity { get; set; }
        private int _stopBits { get; set; }
        private int _timeout { get; set; }
        private BaudRateValues _baudRate;


        public enum BaudRateValues
        {
            b_300 = 300,
            b_600 = 600,
            b_1200 = 1200,
            b_2400 = 2400,
            b_4800 = 4800,
            b_9600 = 9600,
            b_19200 = 19200
        }


        public enum ParityValues
        {
            None = 0,
            Even = 1,
            Odd = 2
        }

        public enum StopBitsValues
        {
            None = 0,
            One = 1,
            OnePointFive = 2,
            Two = 3
        }

        public void Init()
        {
            _baudRate = BaudRateValues.b_2400;
            _parity = (int)ParityValues.None;
            _dataBit = 8;
            _stopBits = (int)StopBitsValues.One;
            _timeout = 3000;
        }

        [Category("Configuración del puerto"), DescriptionAttribute("Bits de datos: Se refiere a la cantidad de bits en la transmisión.")]
        public int data_bit { get { return _dataBit; } set { _dataBit = value; } }

        [Category("Configuración del puerto"), DescriptionAttribute("Paridad: Es una forma sencilla de verificar si hay errores " +
            "en la transmisión serial. Existen cuatro tipos de paridad: par, impar, marcada y espaciada. ")]
        public ParityValues parity
        {
            get
            {
                return (ParityValues)_parity;
                //return Enum.Parse(typeof(ParityValues), Parity);
            }
            set { _parity = (int)value; }
        }

        [Category("Configuración del puerto"), DescriptionAttribute("Bits de parada: Usado para indicar el fin de la comunicación " +
            "de un solo paquete. Los valores típicos son 1, 1.5 o 2 bits. ")]
        public StopBitsValues stop_bits
        {
            get { return (StopBitsValues)_stopBits; }
            set { _stopBits = (int)value; }
        }

        [Category("Configuración del puerto"), DescriptionAttribute("Tiempo de espera en milisegundos. Una vez que la comunicación " +
            "no fue establecida con el dispositivo remoto, la conexión es fallida.")]
        public int timeout { get { return _timeout; } set { _timeout = value; } }

        [Category("Configuración del puerto"), DescriptionAttribute("Velocidad de transmisión (baud rate): Indica el número de bits " +
            "por segundo que se transfieren, y se mide en baudios (bauds)")]
        public int baud_rate
        {
            get
            {
                return (int)_baudRate;
            }
            set
            {
                _baudRate = (BaudRateValues)Enum.Parse(typeof(BaudRateValues), "" + value);
            }
        }

    }
}
