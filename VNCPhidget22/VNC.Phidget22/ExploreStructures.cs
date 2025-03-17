using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidget22;

namespace VNC.Phidget22
{
    public class ExploreStructures
    {
        public DistanceSensorSonarReflections DistanceSensorSonarReflections;
        public GPGGA GPGGA;
        public GPGSA GPGSA;
        public GPRMC GPRMC;
        public GPSDate GPSDate;
        public GPSTime GPSTime;
        public GPVTG GPVTG;
        public IRCode IRCode;
        public IRCodeInfo IRCodeInfo;
        public IRLearnedCode IRCodeLearned;
        public LCDFontSize LCDFontSize;
        public LogRotating LogRotating;
        public NMEAData NMEAData;
        public PhidgetClientVersion PhidgetClientVersion;
        public PhidgetServer PhidgetServer;
        public PhidgetServerVersion PhidgetServerVersion;
        public RFIDTag RFIDTag;
        public SpatialEulerAngles SpatialEulerAngles;
        public UnitInfo UnitInfo;
    }
}
