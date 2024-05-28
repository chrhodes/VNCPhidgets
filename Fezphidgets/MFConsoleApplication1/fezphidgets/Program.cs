/*
 * Code by Christophe Gerbier, 2010
 *
*/
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

using GHIElectronics.NETMF.FEZ;
using FEZPhidgets;

namespace MFConsoleApplication1
{
    public class Program
    {
        static Phidget1018 IF1018 = new Phidget1018();
        static Phidget1052 Encoder1052 = new Phidget1052();
        static OutputPort Led = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, false);
        static InterruptPort Bouton = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.LDR, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        
        public static void Main()
        {
            Bouton.OnInterrupt += Bouton_OnInterrupt;
            IF1018.InputChange += Phidget1018_InputChange;
            IF1018.SensorChange += Phidget1018_SensorChange;
            IF1018.Attached += IF1018_Attached;

            Encoder1052.PositionChange += Phidget1052_PositionChange;
            Encoder1052.InputChange += Phidget1052_InputChange;
            Thread.Sleep(Timeout.Infinite);
        }

        static void IF1018_Attached(object sender, Phidget1018AttachedEventArgs e)
        {
            Debug.Print("Attached event fired : "+e.DeviceName+", "+e.SerialNum);
        }

        static void Bouton_OnInterrupt(uint data1, uint data2, System.DateTime time)
        {
            IF1018.Output(7, false);
            Led.Write(false);
        }

        static void Phidget1052_InputChange(object sender, Phidget1052InputChangeEventArgs e)
        {
            Debug.Print("Bouton : " + (e.Value ? "Enfoncé" : "Relâché"));
        }

        static void Phidget1052_PositionChange(object sender, Phidget1052PositionChangeEventArgs e)
        {
            Debug.Print("Position change event. Position : " + e.Position.ToString() + ", Direction : " + (e.Direction ? "Forward" : "Backward")+", Relative move : "+e.RelativeMove.ToString());
        }

        static void Phidget1018_SensorChange(object sender, Phidget1018SensorChangeEventArgs e)
        {
            Debug.Print("Sensor event. Index " + e.Index.ToString() + ", Value " + e.Value.ToString()+", Raw value = "+IF1018.RawSensor[e.Index].ToString());
        }

        static void Phidget1018_InputChange(object sender, Phidget1018InputChangeEventArgs e)
        {
            Debug.Print("Digital input event. Index " + e.Index.ToString() + ", Value " + e.Value.ToString());
            if (e.Index == 6) { IF1018.Output(7, true); Led.Write(true); }
        }
    }
}
