using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

using Fx.Lights;

using Phidgets;
//Needed for the event handling classes 
using Phidgets.Events;


namespace Fx
{
    public class FxShow
    {
        private static int CLASS_BASE_ERRORNUMBER = ErrorNumbers.FXSHOW;
        private const string LOG_APPNAME = Common.APP_NAME;

        const string CONTROL_NAME = "FxShow";

        #region Initialization

        public FxShow()
        {

        }

        public FxShow(XElement showXml)
        {
            ShowXml = showXml;
        }

        #endregion

        #region Properties

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

        private Collection<DisplayLight> _DisplayLights = new Collection<DisplayLight>();
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

        private string _Duration;
        public string Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }
        
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

        private int _SetDelay = 5000;
        /// <summary>
        /// SetDelay: The delay in seconds between sets in the show.
        /// </summary>
        public int SetDelay
        {
            get
            {
                return _SetDelay;
            }
            set
            {
                _SetDelay = value;
            }
        }

        private Collection<Set> _Sets = new Collection<Set>();
        public Collection<Set> Sets
        {
            get
            {
                return _Sets;
            }
            set
            {
                _Sets = value;
            }
        }

        private XElement _ShowXml;
        public XElement ShowXml
        {
            get
            {
                return _ShowXml;
            }
            set
            {
                _ShowXml = value;
            }
        }

        private static bool _UseDebugLightGrid;
        public static bool UseDebugLightGrid
        {
            get
            {
                return _UseDebugLightGrid;
            }
            set
            {
                _UseDebugLightGrid = value;
            }
        }
        
        InterfaceKit InterfaceKit_136645 = new InterfaceKit();
        InterfaceKit InterfaceKit_136647 = new InterfaceKit();

        #endregion

        #region Main Function Routines

        private void InitializeLights()
        {
            DebugWindow.Common.WriteToDebugWindow(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (XElement light in _ShowXml.Element("Initialization").Element("DisplayLights").Elements("DisplayLight"))
            {
                DisplayLight displayLight = new DisplayLight();
                displayLight.Id = light.Attribute("Id").Value;

                // Need to get new InterfaceKit each time value changes
                // Need to determine when to init Interface Kits.
                //displayLight.IOInterFaceKit = light.Attribute("InterfaceKit").Value;

                switch (light.Attribute("InterfaceKit").Value)
                {
                    case "136645":
                        displayLight.IOInterFaceKit = InterfaceKit_136645;
                        break;

                    case "136647":
                        displayLight.IOInterFaceKit = InterfaceKit_136647;
                        break;

                    default:
                        break;
                        //
                }

                displayLight.PortNumber = int.Parse(light.Attribute("PortNumber").Value);
                displayLight.Color = light.Attribute("Color").Value;

                // Add the light to the collection
                DisplayLights.Add(displayLight);

                //<DisplayLight Id="0"  InterfaceKit="136645" PortNumber="0" />
                //System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2}", 
                //    light.Attribute("Id").Value,
                //    light.Attribute("InterfaceKit").Value,
                //    light.Attribute("PortNumber").Value));
            }
        }

        private void InitializePhidgets()
        {
            try
            {
                //Initialize the InterfaceKit object
                //ifKitA = new InterfaceKit();
                //ifKitB = new InterfaceKit();
                //ifKitC = new InterfaceKit();

                //Hook the basic event handlers
                InterfaceKit_136645.Attach += ifKit_Attach;
                InterfaceKit_136647.Attach += ifKit_Attach;

                InterfaceKit_136645.Detach += ifKit_Detach;
                InterfaceKit_136647.Detach += ifKit_Detach;

                InterfaceKit_136645.Error += ifKit_Error;
                InterfaceKit_136647.Error += ifKit_Error;
                
                //ifKitA.Attach += new AttachEventHandler(ifKit_Attach);
                //ifKitA.Detach += new DetachEventHandler(ifKit_Detach);
                //ifKitA.Error += new ErrorEventHandler(ifKit_Error);

                //Hook the phidget spcific event handlers
                //ifKit.InputChange += new InputChangeEventHandler(ifKit_InputChange);
                //ifKit.OutputChange += new OutputChangeEventHandler(ifKit_OutputChange);
                //ifKit.SensorChange += new SensorChangeEventHandler(ifKit_SensorChange);

                //Open the object for device connections
                //ifKit.open();

                //Open the Phidget using the command line arguments

                InterfaceKit_136645.open(136645, "10.0.200.2", 5001);
                InterfaceKit_136647.open(136647, "10.0.200.2", 5001);

                //ifKitA.open(Int32.Parse(txtInterfaceKitA.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));
                //ifKitB.open(Int32.Parse(txtInterfaceKitB.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));
                //ifKitC.open(Int32.Parse(txtInterfaceKitC.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));

                //openCmdLine(ifKit);

                //Wait for an InterfaceKit phidget to be attached
                Trace.WriteLine("Waiting for InterfaceKit to be attached...");

                InterfaceKit_136645.waitForAttachment();
                InterfaceKit_136647.waitForAttachment();
                //ifKitC.waitForAttachment();

                ////Wait for user input so that we can wait and watch for some event data 
                ////from the phidget
                //Console.WriteLine("Press any key to end...");
                //Console.Read();

                ////User input was rad so we'll terminate the program, so close the object
                //ifKit.close();

                ////set the object to null to get it out of memory
                //ifKit = null;

                ////If no expcetions where thrown at this point it is safe to terminate 
                ////the program
                //Console.WriteLine("ok");
            }
            catch (PhidgetException ex)
            {
                Trace.WriteLine(ex.Description);
            }
        }

        private void InitializeSets()
        {
            foreach (Set set in Sets)
            {
                
            }    
        }

        public void LightsOff(int delay)
        {
            foreach (DisplayLight light in DisplayLights)
            {
                light.Off();
                System.Threading.Thread.Sleep(delay);
            }
        }

        public void LightsOn(int delay)
        {
            foreach (DisplayLight light in DisplayLights)
            {
                light.On();
                System.Threading.Thread.Sleep(delay);
            }
        }

        public void LightsOff(int delay, string color)
        {
            foreach (DisplayLight light in DisplayLights)
            {
                if (light.Color == color)
                {
                    light.Off();
                    System.Threading.Thread.Sleep(delay);
                }
            }
        }

        public void LightsOn(int delay, string color)
        {
            foreach (DisplayLight light in DisplayLights)
            {
                if (light.Color == color)
                {
                    light.On();
                    System.Threading.Thread.Sleep(delay);
                }
            }
        }

        private void LoadSets()
        {
            DebugWindow.Common.WriteToDebugWindow(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (XElement setInfo in _ShowXml.Element("Sets").Elements("Set"))
            {
                Set set = new Set();

                set.SetXml = setInfo;
                set.DisplayLights = _DisplayLights;

                // Add the set to the collection
                Sets.Add(set);

                set.Prepare();
            }
        }

        /// <summary>
        /// Prepare() - Prepares the Show by parsing the ShowXml  
        /// and initializing the properties
        /// and loading the Sets.
        /// </summary>
        public void Prepare()
        {
            Name = _ShowXml.Attribute("Name").Value;
            Description = _ShowXml.Attribute("Description").Value;
            SetDelay = int.Parse(_ShowXml.Attribute("SetDelay").Value);
            Duration = _ShowXml.Attribute("Duration").Value;

            if (! UseDebugLightGrid)
            {
                InitializePhidgets();
            }


            InitializeLights();

            LoadSets();

            //InitializeSets();
        }

        /// <summary>
        /// Present() - Presents the Show by displaying each Set separated by SetDelay.
        /// </summary>
        public void Present()
        {
            foreach (Set set in Sets)
            {
                set.Present();
                DebugWindow.Common.WriteToDebugWindow(string.Format("Delaying between sets for {0} seconds", SetDelay));
                System.Threading.Thread.Sleep(SetDelay);
            }
        }

        #endregion

        #region Phidget Event Handlers

        //Attach event handler...Display the serial number of the attached InterfaceKit 
        //to the console      
        static void ifKit_Attach(object sender, AttachEventArgs e)
        {
            Trace.WriteLine(string.Format("InterfaceKit {0} attached!",
                                e.Device.SerialNumber.ToString()));
        }

        //Detach event handler...Display the serial number of the detached InterfaceKit 
        //to the console
        static void ifKit_Detach(object sender, DetachEventArgs e)
        {
            Trace.WriteLine(string.Format("InterfaceKit {0} detached!",
                                e.Device.SerialNumber.ToString()));
        }

        //Error event handler...Display the error description to the console
        static void ifKit_Error(object sender, ErrorEventArgs e)
        {
            Trace.WriteLine(e.Description);
        }

        #endregion

    }
}
