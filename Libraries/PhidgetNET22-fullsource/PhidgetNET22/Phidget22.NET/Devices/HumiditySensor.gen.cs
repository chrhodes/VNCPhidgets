using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> HumiditySensor class definition </summary>
	public partial class HumiditySensor : Phidget {
		#region Constructor/Destructor
		/// <summary> HumiditySensor Constructor </summary>
		public HumiditySensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHumiditySensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> HumiditySensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~HumiditySensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetHumiditySensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>HumidityChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>HumidityChange</c> events can also affected by the
		/// <c>HumidityChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetHumiditySensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetHumiditySensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetHumiditySensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetHumiditySensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The humidity value </summary>
		/// <remarks>The most recent humidity value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinHumidity</c> and <c>MaxHumidity</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Humidity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getHumidity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The humidity value </summary>
		/// <remarks>The minimum value that the <c>HumidityChange</c> event will report.
		/// </remarks>
		public double MinHumidity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getMinHumidity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The humidity value </summary>
		/// <remarks>The maximum value that the <c>HumidityChange</c> event will report.
		/// </remarks>
		public double MaxHumidity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getMaxHumidity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>HumidityChange</c> event until the humidity value has changed
		/// by the amount specified by the <c>HumidityChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>HumidityChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering.</item>
		/// </list>
		/// 
		/// </remarks>
		public double HumidityChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getHumidityChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetHumiditySensor_setHumidityChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>HumidityChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinHumidityChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getMinHumidityChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>HumidityChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxHumidityChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetHumiditySensor_getMaxHumidityChangeTrigger(chandle, out val);
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
			nativeHumidityChangeEventCallback = new Phidget22Imports.HumiditySensorHumidityChangeEvent(nativeHumidityChangeEvent);
			result = Phidget22Imports.PhidgetHumiditySensor_setOnHumidityChangeHandler(chandle, nativeHumidityChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetHumiditySensor_setOnHumidityChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent humidity value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>HumidityChangeTrigger</c> has been set to a non-zero value, the
		/// <c>HumidityChange</c> event will not occur until the humidity has changed by at least the
		/// <c>HumidityChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event HumiditySensorHumidityChangeEventHandler HumidityChange;
		internal void OnHumidityChange(HumiditySensorHumidityChangeEventArgs e) {
			if (HumidityChange != null) {
				foreach (HumiditySensorHumidityChangeEventHandler HumidityChangeHandler in HumidityChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = HumidityChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(HumidityChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						HumidityChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.HumiditySensorHumidityChangeEvent nativeHumidityChangeEventCallback;
		internal void nativeHumidityChangeEvent(IntPtr phid, IntPtr ctx, double humidity) {
			OnHumidityChange(new HumiditySensorHumidityChangeEventArgs(humidity));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getHumidity(IntPtr phid, out double Humidity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMinHumidity(IntPtr phid, out double MinHumidity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMaxHumidity(IntPtr phid, out double MaxHumidity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getHumidityChangeTrigger(IntPtr phid, out double HumidityChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_setHumidityChangeTrigger(IntPtr phid, double HumidityChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMinHumidityChangeTrigger(IntPtr phid, out double MinHumidityChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_getMaxHumidityChangeTrigger(IntPtr phid, out double MaxHumidityChangeTrigger);
		public delegate void HumiditySensorHumidityChangeEvent(IntPtr phid, IntPtr ctx, double humidity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHumiditySensor_setOnHumidityChangeHandler(IntPtr phid, HumiditySensorHumidityChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A HumiditySensor HumidityChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A HumiditySensorHumidityChangeEventArg object contains data and information related to the Event.</param>
	public delegate void HumiditySensorHumidityChangeEventHandler(object sender, HumiditySensorHumidityChangeEventArgs e);
	/// <summary> HumiditySensor HumidityChange Event data </summary>
	public class HumiditySensorHumidityChangeEventArgs : EventArgs {
		/// <summary>The ambient relative humidity
		/// </summary>
		public readonly double Humidity;
		internal HumiditySensorHumidityChangeEventArgs(double humidity) {
			this.Humidity = humidity;
		}
	}

}
