using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Spatial class definition </summary>
	public partial class Spatial : Phidget {
		#region Constructor/Destructor
		/// <summary> Spatial Constructor </summary>
		public Spatial() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSpatial_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Spatial Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Spatial() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetSpatial_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>SpatialData</c> event.
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
				result = Phidget22Imports.PhidgetSpatial_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetSpatial_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetSpatial_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetSpatial_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Calibrate your device for the environment it will be used in.
		/// <list>
		/// <item>Due to physical location, hard and soft iron offsets, and even bias errors, your device should
		/// be calibrated. We have created a calibration program that will provide you with the
		/// <c>MagnetometerCorrectionParameters</c> for your specific situation. See your device's User
		/// Guide for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void SetMagnetometerCorrectionParameters(double magneticField, double offset0, double offset1, double offset2, double gain0, double gain1, double gain2, double T0, double T1, double T2, double T3, double T4, double T5) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSpatial_setMagnetometerCorrectionParameters(chandle, magneticField, offset0, offset1, offset2, gain0, gain1, gain2, T0, T1, T2, T3, T4, T5);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Resets the <c>MagnetometerCorrectionParameters</c> to their default values.
		/// <list>
		/// <item>Due to physical location, hard and soft iron offsets, and even bias errors, your device should
		/// be calibrated. We have created a calibration program that will provide you with the
		/// <c>MagnetometerCorrectionParameters</c> for your specific situation. See your device's User
		/// Guide for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void ResetMagnetometerCorrectionParameters() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSpatial_resetMagnetometerCorrectionParameters(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Saves the <c>MagnetometerCorrectionParameters</c>.
		/// <list>
		/// <item>Due to physical location, hard and soft iron offsets, and even bias errors, your device should
		/// be calibrated. We have created a calibration program that will provide you with the
		/// <c>MagnetometerCorrectionParameters</c> for your specific situation. See your device's User
		/// Guide for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void SaveMagnetometerCorrectionParameters() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSpatial_saveMagnetometerCorrectionParameters(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

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
		public void ZeroGyroscope() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSpatial_zeroGyro(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeSpatialDataEventCallback = new Phidget22Imports.SpatialSpatialDataEvent(nativeSpatialDataEvent);
			result = Phidget22Imports.PhidgetSpatial_setOnSpatialDataHandler(chandle, nativeSpatialDataEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetSpatial_setOnSpatialDataHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent values that your channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// </remarks>
		public event SpatialSpatialDataEventHandler SpatialData;
		internal void OnSpatialData(SpatialSpatialDataEventArgs e) {
			if (SpatialData != null) {
				foreach (SpatialSpatialDataEventHandler SpatialDataHandler in SpatialData.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = SpatialDataHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(SpatialDataHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						SpatialDataHandler(this, e);
				}
			}
		}
		Phidget22Imports.SpatialSpatialDataEvent nativeSpatialDataEventCallback;
		internal void nativeSpatialDataEvent(IntPtr phid, IntPtr ctx, double[] acceleration, double[] angularRate, double[] magneticField, double timestamp) {
			OnSpatialData(new SpatialSpatialDataEventArgs(acceleration, angularRate, magneticField, timestamp));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_setMagnetometerCorrectionParameters(IntPtr phid, double magneticField, double offset0, double offset1, double offset2, double gain0, double gain1, double gain2, double T0, double T1, double T2, double T3, double T4, double T5);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_resetMagnetometerCorrectionParameters(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_saveMagnetometerCorrectionParameters(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_zeroGyro(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		public delegate void SpatialSpatialDataEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] acceleration, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] angularRate, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] magneticField, double timestamp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSpatial_setOnSpatialDataHandler(IntPtr phid, SpatialSpatialDataEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Spatial SpatialData Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A SpatialSpatialDataEventArg object contains data and information related to the Event.</param>
	public delegate void SpatialSpatialDataEventHandler(object sender, SpatialSpatialDataEventArgs e);
	/// <summary> Spatial SpatialData Event data </summary>
	public class SpatialSpatialDataEventArgs : EventArgs {
		/// <summary>The acceleration vaulues
		/// </summary>
		public readonly double[] Acceleration;
		/// <summary>The angular rate values
		/// </summary>
		public readonly double[] AngularRate;
		/// <summary>The field strength values
		/// </summary>
		public readonly double[] MagneticField;
		/// <summary>The timestamp value
		/// </summary>
		public readonly double Timestamp;
		internal SpatialSpatialDataEventArgs(double[] acceleration, double[] angularRate, double[] magneticField, double timestamp) {
			this.Acceleration = acceleration;
			this.AngularRate = angularRate;
			this.MagneticField = magneticField;
			this.Timestamp = timestamp;
		}
	}

}
