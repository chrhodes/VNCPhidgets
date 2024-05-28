using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhidgetHelper
{
    public class DistanceSensorEventArgs : EventArgs
    {
        public DistanceSensorEventArgs(double distance)
        {
            _distance = distance;
        }

        private double _distance;

        public double Distance
        {
            get { return _distance; }
        } 
    }
}
