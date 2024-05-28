using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Fx.Lights;

namespace Fx
{
    public class Set
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        private int _DisplayDelay = 5000;
        /// <summary>
        /// DisplayDelay: The delay in seconds between Displays in the set.
        /// </summary>
        public int DisplayDelay
        {
            get
            {
                return _DisplayDelay;
            }
            set
            {
                _DisplayDelay = value;
            }
        }

        private Collection<DisplayLight> _DisplayLights;
        public Collection<DisplayLight> DisplayLights
        {
            get
            {
                return _DisplayLights;
            }
            set
            {
                _DisplayLights = value;
            }
        }

        private Collection<Display> _Displays = new Collection<Display>();
        public Collection<Display> Displays
        {
            get { return _Displays; }
            set
            {
                _Displays = value;
            }
        }

        private string _HostIP;
        public string HostIP
        {
            get
            {
                return _HostIP;
            }
            set
            {
                _HostIP = value;
            }
        }

        private string _HostPort;
        public string HostPort
        {
            get
            {
                return _HostPort;
            }
            set
            {
                _HostPort = value;
            }
        }

        private XElement _SetXml;
        public XElement SetXml
        {
            get
            {
                return _SetXml;
            }
            set
            {
                _SetXml = value;
            }
        }

        private void LoadDisplays()
        {
            foreach (XElement displayInfo in _SetXml.Elements("Display"))
            {
                try
                {
                    Display display = new Display();

                    display.DisplayXml = displayInfo;
                    display.DisplayLights = DisplayLights;

                    Displays.Add(display);

                    display.Prepare();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public void Prepare()
        {
            Name = _SetXml.Attribute("Name").Value;
            Description = _SetXml.Attribute("Description").Value;
            DisplayDelay = int.Parse(_SetXml.Attribute("DisplayDelay").Value);

            LoadDisplays();
        }

        public void Present()
        {
            foreach (Display display in Displays)
            {
                display.Present();
                DebugWindow.Common.WriteToDebugWindow(string.Format("Delaying between displays for {0} seconds", DisplayDelay));
                System.Threading.Thread.Sleep(DisplayDelay);        
            }
        }
    }
}
