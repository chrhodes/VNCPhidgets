using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {

	public partial class GPS : Phidget {
		/// <summary>
		/// Gets the GPS date and time, in UTC.
		/// </summary>
		/// <exception cref="PhidgetException">If this Phidget is not opened and attached, or if the value is unknown.</exception>
		public DateTime DateAndTime {
			get {
				ErrorCode result;

				IntPtr datePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GPSDate)));
				result = Phidget22Imports.PhidgetGPS_getDate(chandle, datePtr);
				if (result != 0) {
					Marshal.FreeHGlobal(datePtr);
					throw PhidgetException.CreateByCode(result);
				}
				GPSDate date = (GPSDate)Marshal.PtrToStructure(datePtr, typeof(GPSDate));
				Marshal.FreeHGlobal(datePtr);

				IntPtr timePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GPSTime)));
				result = Phidget22Imports.PhidgetGPS_getTime(chandle, timePtr);
				if (result != 0) {
					Marshal.FreeHGlobal(timePtr);
					throw PhidgetException.CreateByCode(result);
				}
				GPSTime time = (GPSTime)Marshal.PtrToStructure(datePtr, typeof(GPSTime));
				Marshal.FreeHGlobal(timePtr);

				return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
			}
		}
	}
}
