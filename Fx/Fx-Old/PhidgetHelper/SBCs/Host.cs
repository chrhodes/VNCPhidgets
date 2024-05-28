using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidgets;

namespace PhidgetHelper.SBCs
{
    public class Host
    {
        // Fields...
        private bool _Enabled;
        private string _Port;
        private string _IPAddress;
        private string _Name;


        /// <summary>
        /// Initializes a new instance of the Host class.
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="port"></param>
        /// <param name="iPAddress"></param>
        /// <param name="name"></param>
        public Host(string name, string iPAddress, string port, bool enabled )
        {
            _Name = name;
            _IPAddress = iPAddress;
            _Port = port;
            _Enabled = enabled;

        }

        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
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
        
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
            }
        }


        public string Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
            }
        }
        
    }
}
