using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> LightSensor class definition </summary>
	public partial class LightSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> LightSensor Constructor </summary>
		public LightSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLightSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> LightSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~LightSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetLightSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>IlluminanceChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>IlluminanceChange</c> events can also affected by the
		/// <c>IlluminanceChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetLightSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLightSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetLightSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetLightSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The illuminance value </summary>
		/// <remarks>The most recent illuminance value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinIlluminance</c> and
		/// <c>MaxIlluminance</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Illuminance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getIlluminance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The illuminance value </summary>
		/// <remarks>The minimum value the <c>IlluminanceChange</c> event will report.
		/// </remarks>
		public double MinIlluminance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getMinIlluminance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The illuminance value </summary>
		/// <remarks>The maximum value the <c>IlluminanceChange</c> event will report.
		/// </remarks>
		public double MaxIlluminance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getMaxIlluminance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>IlluminanceChange</c> event until the illuminance value has
		/// changed by the amount specified by the <c>IlluminanceChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>IlluminanceChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double IlluminanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getIlluminanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetLightSensor_setIlluminanceChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>IlluminanceChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinIlluminanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getMinIlluminanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>IlluminanceChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxIlluminanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetLightSensor_getMaxIlluminanceChangeTrigger(chandle, out val);
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
			nativeIlluminanceChangeEventCallback = new Phidget22Imports.LightSensorIlluminanceChangeEvent(nativeIlluminanceChangeEvent);
			result = Phidget22Imports.PhidgetLightSensor_setOnIlluminanceChangeHandler(chandle, nativeIlluminanceChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetLightSensor_setOnIlluminanceChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent illuminance value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>IlluminanceChangeTrigger</c> has been set to a non-zero value, the
		/// <c>IlluminanceChange</c> event will not occur until the illuminance has changed by at least
		/// the <c>IlluminanceChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event LightSensorIlluminanceChangeEventHandler IlluminanceChange;
		internal void OnIlluminanceChange(LightSensorIlluminanceChangeEventArgs e) {
			if (IlluminanceChange != null) {
				foreach (LightSensorIlluminanceChangeEventHandler IlluminanceChangeHandler in IlluminanceChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = IlluminanceChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(IlluminanceChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						IlluminanceChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.LightSensorIlluminanceChangeEvent nativeIlluminanceChangeEventCallback;
		internal void nativeIlluminanceChangeEvent(IntPtr phid, IntPtr ctx, double illuminance) {
			OnIlluminanceChange(new LightSensorIlluminanceChangeEventArgs(illuminance));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getIlluminance(IntPtr phid, out double Illuminance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMinIlluminance(IntPtr phid, out double MinIlluminance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMaxIlluminance(IntPtr phid, out double MaxIlluminance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getIlluminanceChangeTrigger(IntPtr phid, out double IlluminanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_setIlluminanceChangeTrigger(IntPtr phid, double IlluminanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMinIlluminanceChangeTrigger(IntPtr phid, out double MinIlluminanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_getMaxIlluminanceChangeTrigger(IntPtr phid, out double MaxIlluminanceChangeTrigger);
		public delegate void LightSensorIlluminanceChangeEvent(IntPtr phid, IntPtr ctx, double illuminance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLightSensor_setOnIlluminanceChangeHandler(IntPtr phid, LightSensorIlluminanceChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A LightSensor IlluminanceChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A LightSensorIlluminanceChangeEventArg object contains data and information related to the Event.</param>
	public delegate void LightSensorIlluminanceChangeEventHandler(object sender, LightSensorIlluminanceChangeEventArgs e);
	/// <summary> LightSensor IlluminanceChange Event data </summary>
	public class LightSensorIlluminanceChangeEventArgs : EventArgs {
		/// <summary>The current illuminance
		/// </summary>
		public readonly double Illuminance;
		internal LightSensorIlluminanceChangeEventArgs(double illuminance) {
			this.Illuminance = illuminance;
		}
	}

}
