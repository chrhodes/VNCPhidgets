using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Fx.Lights;

namespace Fx
{
    public class Display
    {

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

        private XElement _DisplayXml;
        public XElement DisplayXml
        {
            get
            {
                return _DisplayXml;
            }
            set
            {
                _DisplayXml = value;
            }
        }

        private Collection<Light> _Lights = new Collection<Light>();
        public Collection<Light> Lights
        {
            get { return _Lights; }
            set
            {
                _Lights = value;
            }
        }

        private int _LoopDelay = 5000;
        /// <summary>
        /// DisplayDelay: The delay in seconds between Loops in the Display.
        /// </summary>
        public int LoopDelay
        {
            get
            {
                return _LoopDelay;
            }
            set
            {
                _LoopDelay = value;
            }
        }

        private int _Loops = 1;
        public int Loops
        {
            get { return _Loops; }
            set
            {
                _Loops = value;
            }
        }
        
        private void LoadLights()
        {
            foreach (XElement lightInfo in _DisplayXml.Elements("Light"))
            {
                try
                {
                    Light light = new Light();

                    light.DisplayLightId = lightInfo.Attribute("Id").Value;
                    light.IsOn = bool.Parse(lightInfo.Attribute("On").Value);

                    Lights.Add(light);

                }
                catch (Exception ex)
                {
                    
                }
                //Display display = new Display();

                //display.DisplayXml = displayInfo;
                //display.DisplayLights = DisplayLights;

                //Displays.Add(display);
            }
        }

        public void Prepare()
        {
            Loops = int.Parse(_DisplayXml.Attribute("Loops").Value);
            LoopDelay = int.Parse(_DisplayXml.Attribute("LoopDelay").Value);

            LoadLights();
        }

        private void PresentDisplay()
        {
            foreach (Light light in Lights)
            {
                foreach (DisplayLight displayLight in DisplayLights)
                {
                    if (displayLight.Id == light.DisplayLightId)
                    {
                        if (light.IsOn)
                        {
                            displayLight.On();
                        }
                        else
                        {
                            displayLight.Off();
                        }
                    }
                }
            }
        }

        public void Present()
        {
            for (int i = 0; i < Loops; i++)
            {
                PresentDisplay();
                DebugWindow.Common.WriteToDebugWindow(string.Format("Delaying between Loops for {0} seconds", LoopDelay));
                System.Threading.Thread.Sleep(LoopDelay);               
            }
        }
        
    }
}
