using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> PHSensor class definition </summary>
	public partial class PHSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> PHSensor Constructor </summary>
		public PHSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetPHSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> PHSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~PHSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetPHSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The temperature of the solution to correct the pH measurement. </summary>
		/// <remarks>Set this property to the measured temperature of the solution to correct the slope of the pH
		/// conversion for temperature.
		/// </remarks>
		public double CorrectionTemperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getCorrectionTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPHSensor_setCorrectionTemperature(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The minimum temperature that can be corrected for. </summary>
		/// <remarks>The minimum value that <c>CorrectionTemperature</c> can be set to.
		/// </remarks>
		public double MinCorrectionTemperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMinCorrectionTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum temperature that can be corrected for. </summary>
		/// <remarks>The minimum value that <c>CorrectionTemperature</c> can be set to.
		/// </remarks>
		public double MaxCorrectionTemperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMaxCorrectionTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>PHChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>PHChange</c> events can also affected by the
		/// <c>PHChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetPHSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPHSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetPHSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetPHSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pH value </summary>
		/// <remarks>The most recent pH value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinPH</c> and <c>MaxPH</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double PH {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getPH(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pH value </summary>
		/// <remarks>The minimum value the <c>PHChange</c> event will report.
		/// </remarks>
		public double MinPH {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMinPH(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pH value </summary>
		/// <remarks>The maximum value the <c>PHChange</c> event will report.
		/// </remarks>
		public double MaxPH {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMaxPH(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>PHChange</c> event until the pH value has changed by the
		/// amount specified by the <c>PHChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>PHChangeTrigger</c> to 0 will result in the channel firing events every
		/// <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double PHChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getPHChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPHSensor_setPHChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>PHChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinPHChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMinPHChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>PHChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxPHChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPHSensor_getMaxPHChangeTrigger(chandle, out val);
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
			nativePHChangeEventCallback = new Phidget22Imports.PHSensorPHChangeEvent(nativePHChangeEvent);
			result = Phidget22Imports.PhidgetPHSensor_setOnPHChangeHandler(chandle, nativePHChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetPHSensor_setOnPHChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent pH value the channel has measured will be reported in this event, which occurs when
		/// the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>PHChangeTrigger</c> has been set to a non-zero value, the <c>PHChange</c>
		/// event will not occur until the pH has changed by at least the <c>PHChangeTrigger</c>
		/// value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event PHSensorPHChangeEventHandler PHChange;
		internal void OnPHChange(PHSensorPHChangeEventArgs e) {
			if (PHChange != null) {
				foreach (PHSensorPHChangeEventHandler PHChangeHandler in PHChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PHChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PHChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PHChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.PHSensorPHChangeEvent nativePHChangeEventCallback;
		internal void nativePHChangeEvent(IntPtr phid, IntPtr ctx, double PH) {
			OnPHChange(new PHSensorPHChangeEventArgs(PH));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getCorrectionTemperature(IntPtr phid, out double CorrectionTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_setCorrectionTemperature(IntPtr phid, double CorrectionTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMinCorrectionTemperature(IntPtr phid, out double MinCorrectionTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMaxCorrectionTemperature(IntPtr phid, out double MaxCorrectionTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getPH(IntPtr phid, out double PH);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMinPH(IntPtr phid, out double MinPH);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMaxPH(IntPtr phid, out double MaxPH);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getPHChangeTrigger(IntPtr phid, out double PHChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_setPHChangeTrigger(IntPtr phid, double PHChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMinPHChangeTrigger(IntPtr phid, out double MinPHChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_getMaxPHChangeTrigger(IntPtr phid, out double MaxPHChangeTrigger);
		public delegate void PHSensorPHChangeEvent(IntPtr phid, IntPtr ctx, double PH);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPHSensor_setOnPHChangeHandler(IntPtr phid, PHSensorPHChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A PHSensor PHChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A PHSensorPHChangeEventArg object contains data and information related to the Event.</param>
	public delegate void PHSensorPHChangeEventHandler(object sender, PHSensorPHChangeEventArgs e);
	/// <summary> PHSensor PHChange Event data </summary>
	public class PHSensorPHChangeEventArgs : EventArgs {
		/// <summary>The current pH
		/// </summary>
		public readonly double PH;
		internal PHSensorPHChangeEventArgs(double PH) {
			this.PH = PH;
		}
	}

}
