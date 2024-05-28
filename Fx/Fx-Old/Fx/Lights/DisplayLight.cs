using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Phidgets;
using Fx.ServiceReference1;

namespace Fx.Lights
{
    public class DisplayLight
    {
        ServiceReference1.DisplayLightClient displayLightClient = new DisplayLightClient();

        public DisplayLight()
        {

        }
        
        private string _Color;
        public string Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
            }
        }
        
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
            }
        }
        
        private InterfaceKit _IOInterFaceKit;
        public InterfaceKit IOInterFaceKit
        {
            get
            {
                return _IOInterFaceKit;
            }
            set
            {
                _IOInterFaceKit = value;
            }
        }

        private int _PortNumber = 0;                                  
        public int PortNumber
        {
            get
            {
                return _PortNumber;
            }
            set
            {
                _PortNumber = value;
            }
        }

        /// <summary>
        /// Turn light off based on InterfaceKit and PortNumber
        /// </summary>
        public void Off()
        {
            if (FxShow.UseDebugLightGrid)
            {
                // Turn light off based on Id.  This will be a problem with non-numeric Ids.
                //FxShow.DebugLightGrid.Off(int.Parse(_Id));
                displayLightClient.Off(int.Parse(_Id));
            }
            else
            {
                // Turn light off based on InterFaceKit and PortNumber 
                _IOInterFaceKit.outputs[_PortNumber] = false;               
            }
        }

        /// <summary>
        /// Turn light on based on InterfaceKit and PortNumber
        /// </summary>
        public void On()
        {
            if(FxShow.UseDebugLightGrid)
            {
                // Turn light On based on Id.  This will be a problem with non-numeric Ids.
                //FxShow.DebugLightGrid.On(int.Parse(_Id));
                displayLightClient.On(int.Parse(_Id));
            }
            else
            {
                // Turn light off based on InterFaceKit and PortNumber 
                _IOInterFaceKit.outputs[_PortNumber] = true;
            }
        }
    }
}
