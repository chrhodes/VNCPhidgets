using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> GPS class definition </summary>
	public partial class GPS : Phidget {
		#region Constructor/Destructor
		/// <summary> GPS Constructor </summary>
		public GPS() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetGPS_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> GPS Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~GPS() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetGPS_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> Altitude of the GPS </summary>
		/// <remarks>The altitude above mean sea level in meters.
		/// </remarks>
		public double Altitude {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGPS_getAltitude(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Date of last position </summary>
		/// <remarks>The UTC date of the last received position.
		/// </remarks>
		public GPSDate Date {
			get {
				ErrorCode result;
				IntPtr val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GPSDate)));
				result = Phidget22Imports.PhidgetGPS_getDate(chandle, val);
				if (result != 0) {
					Marshal.FreeHGlobal(val);
					throw PhidgetException.CreateByCode(result);
				}
				GPSDate val1 = (GPSDate)Marshal.PtrToStructure(val, typeof(GPSDate));
				Marshal.FreeHGlobal(val);
				return val1;
			}
		}

		/// <summary> Heading of the GPS </summary>
		/// <remarks>The current true course over ground of the GPS
		/// </remarks>
		public double Heading {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGPS_getHeading(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Latitude of the GPS </summary>
		/// <remarks>The latitude of the GPS in degrees
		/// </remarks>
		public double Latitude {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGPS_getLatitude(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Longtidue of the GPS </summary>
		/// <remarks>The longitude of the GPS.
		/// </remarks>
		public double Longitude {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGPS_getLongitude(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> NMEA Data structure </summary>
		/// <remarks>The NMEA data structure.
		/// </remarks>
		public NMEAData NMEAData {
			get {
				ErrorCode result;
				IntPtr val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NMEAData)));
				result = Phidget22Imports.PhidgetGPS_getNMEAData(chandle, val);
				if (result != 0) {
					Marshal.FreeHGlobal(val);
					throw PhidgetException.CreateByCode(result);
				}
				NMEAData val1 = (NMEAData)Marshal.PtrToStructure(val, typeof(NMEAData));
				Marshal.FreeHGlobal(val);
				return val1;
			}
		}

		/// <summary> Status of the position fix </summary>
		/// <remarks>The status of the position fix
		/// <list>
		/// <item>True if a fix is available and latitude, longitude, and altitude can be read. False if the fix
		/// is not available.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool PositionFixState {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetGPS_getPositionFixState(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Current time </summary>
		/// <remarks>The current UTC time of the GPS
		/// </remarks>
		public GPSTime Time {
			get {
				ErrorCode result;
				IntPtr val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GPSTime)));
				result = Phidget22Imports.PhidgetGPS_getTime(chandle, val);
				if (result != 0) {
					Marshal.FreeHGlobal(val);
					throw PhidgetException.CreateByCode(result);
				}
				GPSTime val1 = (GPSTime)Marshal.PtrToStructure(val, typeof(GPSTime));
				Marshal.FreeHGlobal(val);
				return val1;
			}
		}

		/// <summary> Velocity of the GPS </summary>
		/// <remarks>The current speed over ground of the GPS.
		/// </remarks>
		public double Velocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGPS_getVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeHeadingChangeEventCallback = new Phidget22Imports.GPSHeadingChangeEvent(nativeHeadingChangeEvent);
			result = Phidget22Imports.PhidgetGPS_setOnHeadingChangeHandler(chandle, nativeHeadingChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativePositionChangeEventCallback = new Phidget22Imports.GPSPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetGPS_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativePositionFixStateChangeEventCallback = new Phidget22Imports.GPSPositionFixStateChangeEvent(nativePositionFixStateChangeEvent);
			result = Phidget22Imports.PhidgetGPS_setOnPositionFixStateChangeHandler(chandle, nativePositionFixStateChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetGPS_setOnHeadingChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetGPS_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetGPS_setOnPositionFixStateChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent heading and velocity values will be reported in this event, which occurs when the
		/// GPS heading changes.
		/// </remarks>
		public event GPSHeadingChangeEventHandler HeadingChange;
		internal void OnHeadingChange(GPSHeadingChangeEventArgs e) {
			if (HeadingChange != null) {
				foreach (GPSHeadingChangeEventHandler HeadingChangeHandler in HeadingChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = HeadingChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(HeadingChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						HeadingChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.GPSHeadingChangeEvent nativeHeadingChangeEventCallback;
		internal void nativeHeadingChangeEvent(IntPtr phid, IntPtr ctx, double heading, double velocity) {
			OnHeadingChange(new GPSHeadingChangeEventArgs(heading, velocity));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent values the channel has measured will be reported in this event, which occurs when
		/// the GPS position changes.
		/// </remarks>
		public event GPSPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(GPSPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (GPSPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PositionChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PositionChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PositionChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.GPSPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, double latitude, double longitude, double altitude) {
			OnPositionChange(new GPSPositionChangeEventArgs(latitude, longitude, altitude));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a position fix is obtained or lost.
		/// </remarks>
		public event GPSPositionFixStateChangeEventHandler PositionFixStateChange;
		internal void OnPositionFixStateChange(GPSPositionFixStateChangeEventArgs e) {
			if (PositionFixStateChange != null) {
				foreach (GPSPositionFixStateChangeEventHandler PositionFixStateChangeHandler in PositionFixStateChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PositionFixStateChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PositionFixStateChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PositionFixStateChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.GPSPositionFixStateChangeEvent nativePositionFixStateChangeEventCallback;
		internal void nativePositionFixStateChangeEvent(IntPtr phid, IntPtr ctx, bool positionFixState) {
			OnPositionFixStateChange(new GPSPositionFixStateChangeEventArgs(positionFixState));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getAltitude(IntPtr phid, out double Altitude);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getDate(IntPtr phid, IntPtr Date);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getHeading(IntPtr phid, out double Heading);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getLatitude(IntPtr phid, out double Latitude);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getLongitude(IntPtr phid, out double Longitude);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getNMEAData(IntPtr phid, IntPtr NMEAData);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getPositionFixState(IntPtr phid, out bool PositionFixState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getTime(IntPtr phid, IntPtr Time);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_getVelocity(IntPtr phid, out double Velocity);
		public delegate void GPSHeadingChangeEvent(IntPtr phid, IntPtr ctx, double heading, double velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_setOnHeadingChangeHandler(IntPtr phid, GPSHeadingChangeEvent fptr, IntPtr ctx);
		public delegate void GPSPositionChangeEvent(IntPtr phid, IntPtr ctx, double latitude, double longitude, double altitude);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_setOnPositionChangeHandler(IntPtr phid, GPSPositionChangeEvent fptr, IntPtr ctx);
		public delegate void GPSPositionFixStateChangeEvent(IntPtr phid, IntPtr ctx, bool positionFixState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGPS_setOnPositionFixStateChangeHandler(IntPtr phid, GPSPositionFixStateChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A GPS HeadingChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A GPSHeadingChangeEventArg object contains data and information related to the Event.</param>
	public delegate void GPSHeadingChangeEventHandler(object sender, GPSHeadingChangeEventArgs e);
	/// <summary> GPS HeadingChange Event data </summary>
	public class GPSHeadingChangeEventArgs : EventArgs {
		/// <summary>The current heading
		/// </summary>
		public readonly double Heading;
		/// <summary>The current velocity
		/// </summary>
		public readonly double Velocity;
		internal GPSHeadingChangeEventArgs(double heading, double velocity) {
			this.Heading = heading;
			this.Velocity = velocity;
		}
	}

	/// <summary> A GPS PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A GPSPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void GPSPositionChangeEventHandler(object sender, GPSPositionChangeEventArgs e);
	/// <summary> GPS PositionChange Event data </summary>
	public class GPSPositionChangeEventArgs : EventArgs {
		/// <summary>The current latitude
		/// </summary>
		public readonly double Latitude;
		/// <summary>The current longitude
		/// </summary>
		public readonly double Longitude;
		/// <summary>The current altitude
		/// </summary>
		public readonly double Altitude;
		internal GPSPositionChangeEventArgs(double latitude, double longitude, double altitude) {
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.Altitude = altitude;
		}
	}

	/// <summary> A GPS PositionFixStateChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A GPSPositionFixStateChangeEventArg object contains data and information related to the Event.</param>
	public delegate void GPSPositionFixStateChangeEventHandler(object sender, GPSPositionFixStateChangeEventArgs e);
	/// <summary> GPS PositionFixStateChange Event data </summary>
	public class GPSPositionFixStateChangeEventArgs : EventArgs {
		/// <summary>The state of the position fix. True indicates a fix is obtained. False indicates no fix found.
		/// </summary>
		public readonly bool PositionFixState;
		internal GPSPositionFixStateChangeEventArgs(bool positionFixState) {
			this.PositionFixState = positionFixState;
		}
	}

}
