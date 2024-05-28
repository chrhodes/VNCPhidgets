using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Explore
{
    public partial class Sensor1101_GP2Y0A02YK : UserControl
    {
        
        private bool _EnableEvents = false;
        private double _MaxRange = 150.0;
        private double _MinRange = 20.0;
        public int sensorValue;

        public double MinRange
        {
            get { return _MinRange; }
            set
            {
                _MinRange = value;
            }
        }

        public double MaxRange
        {
            get { return _MaxRange; }
            set
            {
                _MaxRange = value;
            }
        }

        public bool EnableEvents
        {
            get { return _EnableEvents; }
            set
            {
                _EnableEvents = value;
            }
        }
        
        static bool _IsIntialized = false;

        public delegate void OutOfRangeEventHandler();
        //public delegate void InRangeEventHandler2(object sender, DistanceSensorEventArgs e);

        //public static event InRangeEventHandler FireInRangeEvent;

        public event OutOfRangeEventHandler FireOutOfRangeEvent;


        public event EventHandler<DistanceSensorEventArgs> FireInRangeEvent;

        public Sensor1101_GP2Y0A02YK()
        {
            InitializeComponent();
        }

        public void changeDisplay(int val)
        {
            if ((val > 80) && (val < 490))
            {
                double tmp = 9462 / (val - 16.92);
                textBox1.Text = tmp.ToString("0.##") +"cm";

                if ((tmp > _MinRange) && (tmp < _MaxRange))
                {
                    FireInRange(new DistanceSensorEventArgs(tmp));
                }
            }
            else
            {
                FireOutOfRange();
                textBox1.Text = "Object Not Detected";
            }
        }

        private void FireOutOfRange()
        {
            OutOfRangeEventHandler temp = Interlocked.CompareExchange(ref FireOutOfRangeEvent, null, null);
            if (temp != null) temp();
        }

        private void FireInRange(DistanceSensorEventArgs distanceSensorEventArgs)
        {
            EventHandler<DistanceSensorEventArgs> temp = Interlocked.CompareExchange(ref FireInRangeEvent, null, null);
            if (temp != null) temp(this, distanceSensorEventArgs);
        }
    }
}
