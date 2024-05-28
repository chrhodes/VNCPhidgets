using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> TemperatureSensor class definition </summary>
	public partial class TemperatureSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> TemperatureSensor Constructor </summary>
		public TemperatureSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetTemperatureSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> TemperatureSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~TemperatureSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetTemperatureSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>TemperatureChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>TemperatureChange</c> events can also affected by the
		/// <c>TemperatureChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetTemperatureSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetTemperatureSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetTemperatureSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The RTD type </summary>
		/// <remarks>The <c>RTDType</c> must correspond to the RTD type you are using in your application.
		/// <list>
		/// <item>If you are unsure which <c>RTDType</c> to use, visit your device's User Guide for more
		/// information.</item>
		/// </list>
		/// 
		/// </remarks>
		public RTDType RTDType {
			get {
				ErrorCode result;
				RTDType val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getRTDType(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetTemperatureSensor_setRTDType(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The RTD wire setup </summary>
		/// <remarks>The <c>RTDWireSetup</c> must correspond to the wire configuration you are using in your
		/// application.
		/// <list>
		/// <item>If you are unsure which <c>RTDWireSetup</c> to use, visit your device's User Guide for
		/// more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public RTDWireSetup RTDWireSetup {
			get {
				ErrorCode result;
				RTDWireSetup val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getRTDWireSetup(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetTemperatureSensor_setRTDWireSetup(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The temperature value </summary>
		/// <remarks>The most recent temperature value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinTemperature</c> and
		/// <c>MaxTemperature</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Temperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The temperature value </summary>
		/// <remarks>The minimum value the <c>TemperatureChange</c> event will report.
		/// </remarks>
		public double MinTemperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getMinTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The temperature value </summary>
		/// <remarks>The maximum value the <c>TemperatureChange</c> event will report.
		/// </remarks>
		public double MaxTemperature {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getMaxTemperature(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>TemperatureChange</c> event until the temperature value has
		/// changed by the amount specified by the <c>TemperatureChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>TemperatureChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double TemperatureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getTemperatureChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetTemperatureSensor_setTemperatureChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>TemperatureChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinTemperatureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getMinTemperatureChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>TemperatureChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxTemperatureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getMaxTemperatureChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The thermocouple type </summary>
		/// <remarks>The <c>ThermocoupleType</c> must correspond to the thermocouple type you are using in your
		/// application.
		/// <list>
		/// <item>If you are unsure which <c>ThermocoupleType</c> to use, visit the <a href=
		/// 'https://www.phidgets.com/docs/Thermocouple_Primer' target='_blank'>Thermocouple Primer</a> for
		/// more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public ThermocoupleType ThermocoupleType {
			get {
				ErrorCode result;
				ThermocoupleType val;
				result = Phidget22Imports.PhidgetTemperatureSensor_getThermocoupleType(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetTemperatureSensor_setThermocoupleType(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeTemperatureChangeEventCallback = new Phidget22Imports.TemperatureSensorTemperatureChangeEvent(nativeTemperatureChangeEvent);
			result = Phidget22Imports.PhidgetTemperatureSensor_setOnTemperatureChangeHandler(chandle, nativeTemperatureChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetTemperatureSensor_setOnTemperatureChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent temperature value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>TemperatureChangeTrigger</c> has been set to a non-zero value, the
		/// <c>TemperatureChange</c> event will not occur until the temperature has changed by at least
		/// the <c>TemperatureChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event TemperatureSensorTemperatureChangeEventHandler TemperatureChange;
		internal void OnTemperatureChange(TemperatureSensorTemperatureChangeEventArgs e) {
			if (TemperatureChange != null) {
				foreach (TemperatureSensorTemperatureChangeEventHandler TemperatureChangeHandler in TemperatureChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TemperatureChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TemperatureChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TemperatureChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.TemperatureSensorTemperatureChangeEvent nativeTemperatureChangeEventCallback;
		internal void nativeTemperatureChangeEvent(IntPtr phid, IntPtr ctx, double temperature) {
			OnTemperatureChange(new TemperatureSensorTemperatureChangeEventArgs(temperature));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getRTDType(IntPtr phid, out RTDType RTDType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setRTDType(IntPtr phid, RTDType RTDType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getRTDWireSetup(IntPtr phid, out RTDWireSetup RTDWireSetup);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setRTDWireSetup(IntPtr phid, RTDWireSetup RTDWireSetup);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getTemperature(IntPtr phid, out double Temperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMinTemperature(IntPtr phid, out double MinTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMaxTemperature(IntPtr phid, out double MaxTemperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getTemperatureChangeTrigger(IntPtr phid, out double TemperatureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setTemperatureChangeTrigger(IntPtr phid, double TemperatureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMinTemperatureChangeTrigger(IntPtr phid, out double MinTemperatureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getMaxTemperatureChangeTrigger(IntPtr phid, out double MaxTemperatureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_getThermocoupleType(IntPtr phid, out ThermocoupleType ThermocoupleType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setThermocoupleType(IntPtr phid, ThermocoupleType ThermocoupleType);
		public delegate void TemperatureSensorTemperatureChangeEvent(IntPtr phid, IntPtr ctx, double temperature);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetTemperatureSensor_setOnTemperatureChangeHandler(IntPtr phid, TemperatureSensorTemperatureChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A TemperatureSensor TemperatureChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A TemperatureSensorTemperatureChangeEventArg object contains data and information related to the Event.</param>
	public delegate void TemperatureSensorTemperatureChangeEventHandler(object sender, TemperatureSensorTemperatureChangeEventArgs e);
	/// <summary> TemperatureSensor TemperatureChange Event data </summary>
	public class TemperatureSensorTemperatureChangeEventArgs : EventArgs {
		/// <summary>The temperature
		/// </summary>
		public readonly double Temperature;
		internal TemperatureSensorTemperatureChangeEventArgs(double temperature) {
			this.Temperature = temperature;
		}
	}

}
