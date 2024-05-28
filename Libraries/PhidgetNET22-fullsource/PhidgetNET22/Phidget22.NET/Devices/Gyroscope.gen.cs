using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Gyroscope class definition </summary>
	public partial class Gyroscope : Phidget {
		#region Constructor/Destructor
		/// <summary> Gyroscope Constructor </summary>
		public Gyroscope() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetGyroscope_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Gyroscope Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Gyroscope() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetGyroscope_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The last reported angular rate </summary>
		/// <remarks>The most recent angular rate value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinAngularRate</c> and
		/// <c>MaxAngularRate</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double[] AngularRate {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetGyroscope_getAngularRate(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The angular rate values </summary>
		/// <remarks>The minimum value the <c>AngularRateUpdate</c> event will report.
		/// </remarks>
		public double[] MinAngularRate {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetGyroscope_getMinAngularRate(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The angular rate values </summary>
		/// <remarks>The maximum value the <c>AngularRateUpdate</c> event will report.
		/// </remarks>
		public double[] MaxAngularRate {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetGyroscope_getMaxAngularRate(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Axis count value </summary>
		/// <remarks>The number of axes the channel can measure angular rate on.
		/// <list>
		/// <item>See your device's User Guide for more information about the number of axes and their
		/// orientation.</item>
		/// </list>
		/// 
		/// </remarks>
		public int AxisCount {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGyroscope_getAxisCount(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>AngularRateUpdate</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGyroscope_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetGyroscope_setDataInterval(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The minimum value that <c>DataInterval</c> can be set to.
		/// </remarks>
		public int MinDataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGyroscope_getMinDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The maximum value that <c>DataInterval</c> can be set to.
		/// </remarks>
		public int MaxDataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGyroscope_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The timestamp value </summary>
		/// <remarks>The most recent timestamp value that the channel has reported. This is an extremely accurate time
		/// measurement streamed from the device.
		/// <list>
		/// <item>If your application requires a time measurement, you should use this value over a local
		/// software timestamp.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Timestamp {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetGyroscope_getTimestamp(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Re-zeros the gyroscope in 1-2 seconds.
		/// <list>
		/// <item>The device must be stationary when zeroing.</item>
		/// <item>The angular rate will be reported as 0.0°/s while zeroing.</item>
		/// <item>Zeroing the gyroscope is a method of compensating for the drift that is inherent to all
		/// gyroscopes. See your device's User Guide for more information on dealing with drift.</item>
		/// </list>
		/// 
		/// </remarks>
		public void Zero() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetGyroscope_zero(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeAngularRateUpdateEventCallback = new Phidget22Imports.GyroscopeAngularRateUpdateEvent(nativeAngularRateUpdateEvent);
			result = Phidget22Imports.PhidgetGyroscope_setOnAngularRateUpdateHandler(chandle, nativeAngularRateUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetGyroscope_setOnAngularRateUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent angular rate and timestamp values the channel has measured will be reported in this
		/// event, which occurs when the <c>DataInterval</c> has elapsed.
		/// </remarks>
		public event GyroscopeAngularRateUpdateEventHandler AngularRateUpdate;
		internal void OnAngularRateUpdate(GyroscopeAngularRateUpdateEventArgs e) {
			if (AngularRateUpdate != null) {
				foreach (GyroscopeAngularRateUpdateEventHandler AngularRateUpdateHandler in AngularRateUpdate.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = AngularRateUpdateHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(AngularRateUpdateHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						AngularRateUpdateHandler(this, e);
				}
			}
		}
		Phidget22Imports.GyroscopeAngularRateUpdateEvent nativeAngularRateUpdateEventCallback;
		internal void nativeAngularRateUpdateEvent(IntPtr phid, IntPtr ctx, double[] angularRate, double timestamp) {
			OnAngularRateUpdate(new GyroscopeAngularRateUpdateEventArgs(angularRate, timestamp));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_zero(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getAngularRate(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] AngularRate);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getMinAngularRate(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MinAngularRate);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getMaxAngularRate(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MaxAngularRate);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getAxisCount(IntPtr phid, out int AxisCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_getTimestamp(IntPtr phid, out double Timestamp);
		public delegate void GyroscopeAngularRateUpdateEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] angularRate, double timestamp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGyroscope_setOnAngularRateUpdateHandler(IntPtr phid, GyroscopeAngularRateUpdateEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Gyroscope AngularRateUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A GyroscopeAngularRateUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void GyroscopeAngularRateUpdateEventHandler(object sender, GyroscopeAngularRateUpdateEventArgs e);
	/// <summary> Gyroscope AngularRateUpdate Event data </summary>
	public class GyroscopeAngularRateUpdateEventArgs : EventArgs {
		/// <summary>The angular rate values
		/// </summary>
		public readonly double[] AngularRate;
		/// <summary>The timestamp value
		/// </summary>
		public readonly double Timestamp;
		internal GyroscopeAngularRateUpdateEventArgs(double[] angularRate, double timestamp) {
			this.AngularRate = angularRate;
			this.Timestamp = timestamp;
		}
	}

}
