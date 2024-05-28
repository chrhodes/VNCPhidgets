using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Phidgets;

namespace Fx.Lights
{
    public class FlashingLight : DisplayLight
    {
        private int _OnDuration = 250;
        public int OnDuration
        {
            get
            {
                return _OnDuration;
            }
            set
            {
                _OnDuration = value;
            }
        }

        private int _OffDuration = 250;
        public int OffDuration
        {
            get
            {
                return _OffDuration;
            }
            set
            {
                _OffDuration = value;
            }
        }
              
        private int _LoopDelay = 0;
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

        private int _Loops = 0;

                               
        public int Loops
        {
            get
            {
                return _Loops;
            }
            set
            {
                _Loops = value;
            }
        }
        
        public FlashingLight(InterfaceKit interfaceKit, int portNumber)
        {
            IOInterFaceKit = interfaceKit;
            PortNumber = portNumber;
        }

        public FlashingLight(InterfaceKit interfaceKit, int portNumber, int onDuration, int offDuration, int loops, int loopDelay)
        {
            IOInterFaceKit = interfaceKit;
            PortNumber = portNumber;
            OnDuration = onDuration;
            OffDuration = offDuration;
            Loops = loops;
            LoopDelay = loopDelay;
        }



        /// <summary>
        /// Turn light on based on InterfaceKit and PortNumber
        /// </summary>
        public void On(int duration)
        {
            On();
            // Sleep for Duration
        }



        /// <summary>
        /// Turn light off based on InterfaceKit and PortNumber
        /// </summary>
        public void Off(int duration)
        {
            Off();   
            // Sleep for duration
        }

        /// <summary>
        /// Display light for the number of loops and durations set in properties
        /// </summary>
        public void Display()
        {
            // Display light based on Properties
 
            for(int i = 0 ; i < Loops; i++)
            {
                On(_OnDuration);
                Off(_OffDuration);
                // Sleep for LoopDelay
            }
        }

    }
}
