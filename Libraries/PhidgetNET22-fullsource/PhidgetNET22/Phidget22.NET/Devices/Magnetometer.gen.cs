using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Magnetometer class definition </summary>
	public partial class Magnetometer : Phidget {
		#region Constructor/Destructor
		/// <summary> Magnetometer Constructor </summary>
		public Magnetometer() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMagnetometer_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Magnetometer Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Magnetometer() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetMagnetometer_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The axis count value </summary>
		/// <remarks>The number of axes the channel can measure field strength on.
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
				result = Phidget22Imports.PhidgetMagnetometer_getAxisCount(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>MagneticFieldChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>MagneticFieldChange</c> events can also affected by the
		/// <c>MagneticFieldChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetMagnetometer_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMagnetometer_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetMagnetometer_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetMagnetometer_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The channel's measured MagneticField </summary>
		/// <remarks>The most recent field strength value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinMagneticField</c> and
		/// <c>MaxMagneticField</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double[] MagneticField {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetMagnetometer_getMagneticField(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The field strength value </summary>
		/// <remarks>The minimum value the <c>MagneticFieldChange</c> event will report.
		/// </remarks>
		public double[] MinMagneticField {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetMagnetometer_getMinMagneticField(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The field strength value </summary>
		/// <remarks>The maximum value the <c>MagneticFieldChange</c> event will report.
		/// </remarks>
		public double[] MaxMagneticField {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetMagnetometer_getMaxMagneticField(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>MagneticFieldChange</c> event until the field strength value
		/// has changed by the amount specified by the <c>MagneticFieldChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>MagneticFieldChangeTrigger</c> to 0 will result in the channel firing
		/// events every <c>DataInterval</c>. This is useful for applications that implement their own
		/// data filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double MagneticFieldChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMagnetometer_getMagneticFieldChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMagnetometer_setMagneticFieldChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>MagneticFieldChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinMagneticFieldChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMagnetometer_getMinMagneticFieldChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>MagneticFieldChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxMagneticFieldChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMagnetometer_getMaxMagneticFieldChangeTrigger(chandle, out val);
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
				result = Phidget22Imports.PhidgetMagnetometer_getTimestamp(chandle, out val);
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
		/// <c>CompassCorrectionParameters</c> for your specific situation. See your device's User Guide
		/// for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void SetCorrectionParameters(double magneticField, double offset0, double offset1, double offset2, double gain0, double gain1, double gain2, double T0, double T1, double T2, double T3, double T4, double T5) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMagnetometer_setCorrectionParameters(chandle, magneticField, offset0, offset1, offset2, gain0, gain1, gain2, T0, T1, T2, T3, T4, T5);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Resets the <c>CompassCorrectionParameters</c> to their default values.
		/// <list>
		/// <item>Due to physical location, hard and soft iron offsets, and even bias errors, your device should
		/// be calibrated. We have created a calibration program that will provide you with the
		/// <c>CompassCorrectionParameters</c> for your specific situation. See your device's User Guide
		/// for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void ResetCorrectionParameters() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMagnetometer_resetCorrectionParameters(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Saves the <c>CalibrationParameters</c>.
		/// <list>
		/// <item>Due to physical location, hard and soft iron offsets, and even bias errors, your device should
		/// be calibrated. We have created a calibration program that will provide you with the
		/// <c>CompassCorrectionParameters</c> for your specific situation. See your device's User Guide
		/// for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public void SaveCorrectionParameters() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMagnetometer_saveCorrectionParameters(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeMagneticFieldChangeEventCallback = new Phidget22Imports.MagnetometerMagneticFieldChangeEvent(nativeMagneticFieldChangeEvent);
			result = Phidget22Imports.PhidgetMagnetometer_setOnMagneticFieldChangeHandler(chandle, nativeMagneticFieldChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetMagnetometer_setOnMagneticFieldChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent magnetic field values the channel has measured will be reported in this event,
		/// which occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>MagneticFieldChangeTrigger</c> has been set to a non-zero value, the
		/// <c>MagneticFieldChange</c> event will not occur until the field strength has changed by at
		/// least the <c>MagneticFieldChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event MagnetometerMagneticFieldChangeEventHandler MagneticFieldChange;
		internal void OnMagneticFieldChange(MagnetometerMagneticFieldChangeEventArgs e) {
			if (MagneticFieldChange != null) {
				foreach (MagnetometerMagneticFieldChangeEventHandler MagneticFieldChangeHandler in MagneticFieldChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = MagneticFieldChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(MagneticFieldChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						MagneticFieldChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.MagnetometerMagneticFieldChangeEvent nativeMagneticFieldChangeEventCallback;
		internal void nativeMagneticFieldChangeEvent(IntPtr phid, IntPtr ctx, double[] magneticField, double timestamp) {
			OnMagneticFieldChange(new MagnetometerMagneticFieldChangeEventArgs(magneticField, timestamp));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_setCorrectionParameters(IntPtr phid, double magneticField, double offset0, double offset1, double offset2, double gain0, double gain1, double gain2, double T0, double T1, double T2, double T3, double T4, double T5);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_resetCorrectionParameters(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_saveCorrectionParameters(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getAxisCount(IntPtr phid, out int AxisCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMagneticField(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MagneticField);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMinMagneticField(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MinMagneticField);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMaxMagneticField(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MaxMagneticField);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMagneticFieldChangeTrigger(IntPtr phid, out double MagneticFieldChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_setMagneticFieldChangeTrigger(IntPtr phid, double MagneticFieldChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMinMagneticFieldChangeTrigger(IntPtr phid, out double MinMagneticFieldChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getMaxMagneticFieldChangeTrigger(IntPtr phid, out double MaxMagneticFieldChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_getTimestamp(IntPtr phid, out double Timestamp);
		public delegate void MagnetometerMagneticFieldChangeEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] magneticField, double timestamp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMagnetometer_setOnMagneticFieldChangeHandler(IntPtr phid, MagnetometerMagneticFieldChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Magnetometer MagneticFieldChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A MagnetometerMagneticFieldChangeEventArg object contains data and information related to the Event.</param>
	public delegate void MagnetometerMagneticFieldChangeEventHandler(object sender, MagnetometerMagneticFieldChangeEventArgs e);
	/// <summary> Magnetometer MagneticFieldChange Event data </summary>
	public class MagnetometerMagneticFieldChangeEventArgs : EventArgs {
		/// <summary>The magnetic field values
		/// </summary>
		public readonly double[] MagneticField;
		/// <summary>The timestamp value
		/// </summary>
		public readonly double Timestamp;
		internal MagnetometerMagneticFieldChangeEventArgs(double[] magneticField, double timestamp) {
			this.MagneticField = magneticField;
			this.Timestamp = timestamp;
		}
	}

}
