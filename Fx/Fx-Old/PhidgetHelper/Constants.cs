using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidgetHelper
{
    public class Constants
    {
        public const int DATA_RATE_MAX = 16;        // Can go faster if local. 16ms from Web.
        public const int DATA_RATE_MIN = 1000;      // See what comes back from sensor.
        public const int DATA_RATE_DEFAULT = 250;   // 0.25 seconds

        public const int ANALOG_MAX = 1000;
        public const int ANALOG_MIN = 0;

        public const int SENSITIVITY_MAX = 0;
        public const int SENSITIVITY_MIN = 1000;
        public const int SENSITIVITY_DEFAULT = 10;

        public const int SENSOR_CHANGE_TRIGGER = 10;
    }
}
