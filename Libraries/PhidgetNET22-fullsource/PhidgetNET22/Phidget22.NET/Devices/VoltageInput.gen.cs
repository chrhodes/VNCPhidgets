using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> VoltageInput class definition </summary>
	public partial class VoltageInput : Phidget {
		#region Constructor/Destructor
		/// <summary> VoltageInput Constructor </summary>
		public VoltageInput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetVoltageInput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> VoltageInput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~VoltageInput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetVoltageInput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between events can also affected by the change trigger values.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetVoltageInput_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetVoltageInput_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetVoltageInput_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The power supply value </summary>
		/// <remarks>Choose the power supply voltage.
		/// <list>
		/// <item>Set this to the voltage specified in the attached sensor's data sheet to power it.</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>Set to POWER_SUPPLY_OFF to turn off the supply to save power.</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public PowerSupply PowerSupply {
			get {
				ErrorCode result;
				PowerSupply val;
				result = Phidget22Imports.PhidgetVoltageInput_getPowerSupply(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setPowerSupply(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The sensor type value </summary>
		/// <remarks>We sell a variety of analog sensors that do not have their own API, they simply output a voltage
		/// that can be converted to a digital value using a specific formula. By matching the
		/// <c>SensorType</c> to your analog sensor, the correct formula will automatically be applied to
		/// data when you get the <c>SensorValue</c> or subscribe to the <c>SensorChange</c> event.
		/// <list>
		/// <item>The <c>SensorChange</c> event has its own change trigger associated with it:
		/// <c>SensorValueChangeTrigger</c>.</item>
		/// <item>Any data from getting the <c>SensorValue</c> or subscribing to the
		/// <c>SensorChange</c> event will have a <c>SensorUnit</c> associated with it.</item>
		/// </list>
		/// <strong>Note:</strong> Unlike other properties such as <c>DeviceSerialNumber</c> or
		/// <c>Channel</c>, <c>SensorType</c> is set after the device is opened, not before.
		/// </remarks>
		public VoltageSensorType SensorType {
			get {
				ErrorCode result;
				VoltageSensorType val;
				result = Phidget22Imports.PhidgetVoltageInput_getSensorType(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setSensorType(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The sensor unit information corresponding to the <code>SensorValue</code>. </summary>
		/// <remarks>The unit of measurement that applies to the sensor values of the <c>SensorType</c> that has
		/// been selected.
		/// <list>
		/// <item>Helps keep track of the type of information being calculated from the voltage input.</item>
		/// </list>
		/// 
		/// </remarks>
		public UnitInfo SensorUnit {
			get {
				ErrorCode result;
				IntPtr val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UnitInfo)));
				result = Phidget22Imports.PhidgetVoltageInput_getSensorUnit(chandle, val);
				if (result != 0) {
					Marshal.FreeHGlobal(val);
					throw PhidgetException.CreateByCode(result);
				}
				UnitInfo val1 = UnitInfoMarshaler.Instance.MarshalNativeToManaged(val);
				Marshal.FreeHGlobal(val);
				return val1;
			}
		}

		/// <summary> The sensor value </summary>
		/// <remarks>The most recent sensor value that the channel has reported.
		/// <list>
		/// <item>Use <c>SensorUnit</c> to get the measurement units that are associated with the
		/// <c>SensorValue</c></item>
		/// </list>
		/// 
		/// </remarks>
		public double SensorValue {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getSensorValue(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>SensorChange</c> event until the sensor value has changed by
		/// the amount specified by the <c>SensorValueChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>SensorChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double SensorValueChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getSensorValueChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setSensorValueChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The most recent voltage value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinVoltage</c> and <c>MaxVoltage</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Voltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The minimum value the <c>VoltageChange</c> event will report.
		/// </remarks>
		public double MinVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getMinVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The maximum value the <c>VoltageChange</c> event will report.
		/// </remarks>
		public double MaxVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getMaxVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>VoltageChange</c> event until the voltage value has changed
		/// by the amount specified by the <c>VoltageChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>VoltageChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double VoltageChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getVoltageChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setVoltageChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>VoltageChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinVoltageChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getMinVoltageChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>VoltageChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxVoltageChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageInput_getMaxVoltageChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage range value </summary>
		/// <remarks>The voltage range you choose should allow you to measure the full range of your input signal.
		/// <list>
		/// <item>A larger <c>VoltageRange</c> equates to less resolution.</item>
		/// <item>If a <c>Saturation</c> event occurs, increase the voltage range.</item>
		/// </list>
		/// 
		/// </remarks>
		public VoltageRange VoltageRange {
			get {
				ErrorCode result;
				VoltageRange val;
				result = Phidget22Imports.PhidgetVoltageInput_getVoltageRange(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageInput_setVoltageRange(chandle, value);
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
			nativeSensorChangeEventCallback = new Phidget22Imports.VoltageInputSensorChangeEvent(nativeSensorChangeEvent);
			result = Phidget22Imports.PhidgetVoltageInput_setOnSensorChangeHandler(chandle, nativeSensorChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVoltageChangeEventCallback = new Phidget22Imports.VoltageInputVoltageChangeEvent(nativeVoltageChangeEvent);
			result = Phidget22Imports.PhidgetVoltageInput_setOnVoltageChangeHandler(chandle, nativeVoltageChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetVoltageInput_setOnSensorChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetVoltageInput_setOnVoltageChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent sensor value the channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>SensorValueChangeTrigger</c> has been set to a non-zero value, the
		/// <c>SensorChange</c> event will not occur until the sensor value has changed by at least the
		/// <c>SensorValueChangeTrigger</c> value.</item>
		/// <item>This event only fires when <c>SensorType</c> is not set to
		/// <c>SENSOR_TYPE_VOLTAGE</c></item>
		/// </list>
		/// 
		/// </remarks>
		public event VoltageInputSensorChangeEventHandler SensorChange;
		internal void OnSensorChange(VoltageInputSensorChangeEventArgs e) {
			if (SensorChange != null) {
				foreach (VoltageInputSensorChangeEventHandler SensorChangeHandler in SensorChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = SensorChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(SensorChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						SensorChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.VoltageInputSensorChangeEvent nativeSensorChangeEventCallback;
		internal void nativeSensorChangeEvent(IntPtr phid, IntPtr ctx, double sensorValue, IntPtr sensorUnit) {
			OnSensorChange(new VoltageInputSensorChangeEventArgs(sensorValue, UnitInfoMarshaler.Instance.MarshalNativeToManaged(sensorUnit)));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent voltage value the channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>VoltageChangeTrigger</c> has been set to a non-zero value, the
		/// <c>VoltageChange</c> event will not occur until the voltage has changed by at least the
		/// <c>VoltageChangeTrigger</c> value.</item>
		/// <item>If <c>SensorType</c> is supported and set to anything other then
		/// <c>SENSOR_TYPE_VOLTAGE</c>, this event will not fire.</item>
		/// </list>
		/// 
		/// </remarks>
		public event VoltageInputVoltageChangeEventHandler VoltageChange;
		internal void OnVoltageChange(VoltageInputVoltageChangeEventArgs e) {
			if (VoltageChange != null) {
				foreach (VoltageInputVoltageChangeEventHandler VoltageChangeHandler in VoltageChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = VoltageChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(VoltageChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						VoltageChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.VoltageInputVoltageChangeEvent nativeVoltageChangeEventCallback;
		internal void nativeVoltageChangeEvent(IntPtr phid, IntPtr ctx, double voltage) {
			OnVoltageChange(new VoltageInputVoltageChangeEventArgs(voltage));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getPowerSupply(IntPtr phid, out PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setPowerSupply(IntPtr phid, PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getSensorType(IntPtr phid, out VoltageSensorType SensorType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setSensorType(IntPtr phid, VoltageSensorType SensorType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getSensorUnit(IntPtr phid, IntPtr SensorUnit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getSensorValue(IntPtr phid, out double SensorValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getSensorValueChangeTrigger(IntPtr phid, out double SensorValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setSensorValueChangeTrigger(IntPtr phid, double SensorValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getVoltage(IntPtr phid, out double Voltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMinVoltage(IntPtr phid, out double MinVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMaxVoltage(IntPtr phid, out double MaxVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getVoltageChangeTrigger(IntPtr phid, out double VoltageChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setVoltageChangeTrigger(IntPtr phid, double VoltageChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMinVoltageChangeTrigger(IntPtr phid, out double MinVoltageChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getMaxVoltageChangeTrigger(IntPtr phid, out double MaxVoltageChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_getVoltageRange(IntPtr phid, out VoltageRange VoltageRange);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setVoltageRange(IntPtr phid, VoltageRange VoltageRange);
		public delegate void VoltageInputSensorChangeEvent(IntPtr phid, IntPtr ctx, double sensorValue, IntPtr sensorUnit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setOnSensorChangeHandler(IntPtr phid, VoltageInputSensorChangeEvent fptr, IntPtr ctx);
		public delegate void VoltageInputVoltageChangeEvent(IntPtr phid, IntPtr ctx, double voltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageInput_setOnVoltageChangeHandler(IntPtr phid, VoltageInputVoltageChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A VoltageInput SensorChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A VoltageInputSensorChangeEventArg object contains data and information related to the Event.</param>
	public delegate void VoltageInputSensorChangeEventHandler(object sender, VoltageInputSensorChangeEventArgs e);
	/// <summary> VoltageInput SensorChange Event data </summary>
	public class VoltageInputSensorChangeEventArgs : EventArgs {
		/// <summary>The sensor value
		/// </summary>
		public readonly double SensorValue;
		/// <summary>The sensor unit information corresponding to the sensor value.
		/// <list>
		/// <item>Helps keep track of the type of information being calculated from the voltage input.</item>
		/// </list>
		/// 
		/// </summary>
		public readonly UnitInfo SensorUnit;
		internal VoltageInputSensorChangeEventArgs(double sensorValue, UnitInfo sensorUnit) {
			this.SensorValue = sensorValue;
			this.SensorUnit = sensorUnit;
		}
	}

	/// <summary> A VoltageInput VoltageChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A VoltageInputVoltageChangeEventArg object contains data and information related to the Event.</param>
	public delegate void VoltageInputVoltageChangeEventHandler(object sender, VoltageInputVoltageChangeEventArgs e);
	/// <summary> VoltageInput VoltageChange Event data </summary>
	public class VoltageInputVoltageChangeEventArgs : EventArgs {
		/// <summary>Measured voltage
		/// </summary>
		public readonly double Voltage;
		internal VoltageInputVoltageChangeEventArgs(double voltage) {
			this.Voltage = voltage;
		}
	}

}
