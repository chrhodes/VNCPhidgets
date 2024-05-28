using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidgets;

namespace PhidgetHelper.Motors
{
    public class AdvancedServo : Phidgets.AdvancedServo
    {
        // Fields...
        private int _FxSerialNumber;

        private bool _Enable;

        private int _Port;
        private string _IPAddress;

        /// <summary>
        /// Initializes a new instance of the AdvancedServo class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public AdvancedServo(string ipAddress, int port, int fXSerialNumber, bool enable)
        {
            _IPAddress = ipAddress;
            _Port = port;
            _FxSerialNumber = fXSerialNumber;
            _Enable = enable;
        }

        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
            }
        }

        public int FxSerialNumber
        {
            get { return _FxSerialNumber; }
            set
            {
                _FxSerialNumber = value;
            }
        }

        public string IPAddress
        {
            get { return _IPAddress; }
            set
            {
                _IPAddress = value;
            }
        }

        public int Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
            }
        }

    }
}
