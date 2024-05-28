/*
 * Code by Christophe Gerbier, 2010
 *
*/
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

using GHIElectronics.NETMF.FEZ;
using FEZPhidgets;
using FEZPhidgets.Events;

namespace MFConsoleApplication1
{
    public class Program
    {
        static Phidget1018 IF1018 = new Phidget1018();
        static Phidget1052 Encoder1052 = new Phidget1052();
        static PhidgetRFID MyRFID = new PhidgetRFID();

        static OutputPort Led = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, false);
        static InterruptPort Bouton = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.LDR, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        
        public static void Main()
        {
            Bouton.OnInterrupt += Bouton_OnInterrupt;

            IF1018.InputChange += Phidget1018_InputChange;
            IF1018.SensorChange += Phidget1018_SensorChange;
            IF1018.Attach += IF1018_Attached;

            Encoder1052.PositionChange += Phidget1052_PositionChange;
            Encoder1052.InputChange += Phidget1052_InputChange;

            MyRFID.Attach += MyRFID_Attached;
            MyRFID.Tag += MyRFID_Tag;
            MyRFID.TagLost += MyRFID_TagLost;

            Thread.Sleep(Timeout.Infinite);
        }

        static void MyRFID_TagLost(object sender, TagEventArgs e)
        {
            Debug.Print("Tag lost");
        }

        static void MyRFID_Tag(object sender, TagEventArgs e)
        {
            if (MyRFID.TagPresent) { Debug.Print("Tag : " + e.Tag); }
        }

        static void MyRFID_Attached(object sender, PhidgetAttachEventArgs e)
        {
            Debug.Print("Attached event fired : " + e.DeviceName + ", " + e.SerialNum);
            Thread.Sleep(500);
            MyRFID.Antenna = true;
            MyRFID.LED = true;
        }

        static void IF1018_Attached(object sender, PhidgetAttachEventArgs e)
        {
            Debug.Print("Attached event fired : "+e.DeviceName+", "+e.SerialNum);
        }

        static void Bouton_OnInterrupt(uint data1, uint data2, System.DateTime time)
        {
            IF1018.Output(7, false);
            Led.Write(false);
        }

        static void Phidget1052_InputChange(object sender, PhidgetInputChangeEventArgs e)
        {
            Debug.Print("Bouton : " + (e.Value ? "Enfoncé" : "Relâché"));
        }

        static void Phidget1052_PositionChange(object sender, EncoderPositionChangeEventArgs e)
        {
            Debug.Print("Position change event. Position : " + e.Position.ToString() + ", Direction : " + (e.Direction ? "Forward" : "Backward")+", Relative move : "+e.RelativeMove.ToString());
        }

        static void Phidget1018_SensorChange(object sender, SensorChangeEventArgs e)
        {
            Debug.Print("Sensor event. Index " + e.Index.ToString() + ", Value " + e.Value.ToString()+", Raw value = "+IF1018.RawSensor[e.Index].ToString());
        }

        static void Phidget1018_InputChange(object sender, PhidgetInputChangeEventArgs e)
        {
            Debug.Print("Digital input event. Index " + e.Index.ToString() + ", Value " + e.Value.ToString());
            if (e.Index == 6) { IF1018.Output(7, true); Led.Write(true); }
        }
    }
}
