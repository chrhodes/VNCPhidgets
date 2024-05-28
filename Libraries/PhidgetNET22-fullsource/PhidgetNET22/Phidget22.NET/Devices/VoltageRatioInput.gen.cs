using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> VoltageRatioInput class definition </summary>
	public partial class VoltageRatioInput : Phidget {
		#region Constructor/Destructor
		/// <summary> VoltageRatioInput Constructor </summary>
		public VoltageRatioInput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetVoltageRatioInput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> VoltageRatioInput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~VoltageRatioInput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetVoltageRatioInput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The enabled value </summary>
		/// <remarks>Enable power to and data from the input by setting <c>BridgeEnabled</c> to true.
		/// </remarks>
		public bool BridgeEnabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getBridgeEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setBridgeEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The bridge gain value </summary>
		/// <remarks>Choose a <c>BridgeGain</c> that best suits your application.
		/// <list>
		/// <item>For more information about the range and accuracy of each <c>BridgeGain</c> to decide
		/// which best suits your application, see your device's User Guide.</item>
		/// </list>
		/// 
		/// </remarks>
		public BridgeGain BridgeGain {
			get {
				ErrorCode result;
				BridgeGain val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getBridgeGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setBridgeGain(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The data interval for the channel </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between events can also affected by the change trigger.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
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
		public VoltageRatioSensorType SensorType {
			get {
				ErrorCode result;
				VoltageRatioSensorType val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getSensorType(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setSensorType(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The sensor unit information corresponding to the <code>SensorValue</code>. </summary>
		/// <remarks>The unit of measurement that applies to the sensor values of the <c>SensorType</c> that has
		/// been selected.
		/// <list>
		/// <item>Helps keep track of the type of information being calculated from the voltage ratio input.</item>
		/// </list>
		/// 
		/// </remarks>
		public UnitInfo SensorUnit {
			get {
				ErrorCode result;
				IntPtr val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UnitInfo)));
				result = Phidget22Imports.PhidgetVoltageRatioInput_getSensorUnit(chandle, val);
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
				result = Phidget22Imports.PhidgetVoltageRatioInput_getSensorValue(chandle, out val);
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
				result = Phidget22Imports.PhidgetVoltageRatioInput_getSensorValueChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setSensorValueChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage ratio value </summary>
		/// <remarks>The most recent voltage ratio value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinVoltageRatio</c> and
		/// <c>MaxVoltageRatio</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double VoltageRatio {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getVoltageRatio(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage ratio value </summary>
		/// <remarks>The minimum value the <c>VoltageRatioChange</c> event will report.
		/// </remarks>
		public double MinVoltageRatio {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMinVoltageRatio(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage ratio value </summary>
		/// <remarks>The maximum value the <c>VoltageRatioChange</c> event will report.
		/// </remarks>
		public double MaxVoltageRatio {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMaxVoltageRatio(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>VoltageRatioChange</c> event until the voltage ratio value
		/// has changed by the amount specified by the <c>VoltageRatioChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>VoltageRatioChangeTrigger</c> to 0 will result in the channel firing
		/// events every <c>DataInterval</c>. This is useful for applications that implement their own
		/// data filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double VoltageRatioChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getVoltageRatioChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageRatioInput_setVoltageRatioChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>VoltageRatioChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinVoltageRatioChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMinVoltageRatioChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>VoltageRatioChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxVoltageRatioChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageRatioInput_getMaxVoltageRatioChangeTrigger(chandle, out val);
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
			nativeSensorChangeEventCallback = new Phidget22Imports.VoltageRatioInputSensorChangeEvent(nativeSensorChangeEvent);
			result = Phidget22Imports.PhidgetVoltageRatioInput_setOnSensorChangeHandler(chandle, nativeSensorChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVoltageRatioChangeEventCallback = new Phidget22Imports.VoltageRatioInputVoltageRatioChangeEvent(nativeVoltageRatioChangeEvent);
			result = Phidget22Imports.PhidgetVoltageRatioInput_setOnVoltageRatioChangeHandler(chandle, nativeVoltageRatioChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetVoltageRatioInput_setOnSensorChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetVoltageRatioInput_setOnVoltageRatioChangeHandler(chandle, null, IntPtr.Zero);
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
		/// <c>SENSOR_TYPE_VOLTAGERATIO</c></item>
		/// </list>
		/// 
		/// </remarks>
		public event VoltageRatioInputSensorChangeEventHandler SensorChange;
		internal void OnSensorChange(VoltageRatioInputSensorChangeEventArgs e) {
			if (SensorChange != null) {
				foreach (VoltageRatioInputSensorChangeEventHandler SensorChangeHandler in SensorChange.GetInvocationList()) {
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
		Phidget22Imports.VoltageRatioInputSensorChangeEvent nativeSensorChangeEventCallback;
		internal void nativeSensorChangeEvent(IntPtr phid, IntPtr ctx, double sensorValue, IntPtr sensorUnit) {
			OnSensorChange(new VoltageRatioInputSensorChangeEventArgs(sensorValue, UnitInfoMarshaler.Instance.MarshalNativeToManaged(sensorUnit)));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent voltage ratio value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>VoltageRatioChangeTrigger</c> has been set to a non-zero value, the
		/// <c>VoltageRatioChange</c> event will not occur until the voltage has changed by at least the
		/// <c>VoltageRatioChangeTrigger</c> value.</item>
		/// <item>If <c>SensorType</c> is supported and set to anything other then
		/// <c>SENSOR_TYPE_VOLTAGERATIO</c>, this event will not fire.</item>
		/// </list>
		/// 
		/// </remarks>
		public event VoltageRatioInputVoltageRatioChangeEventHandler VoltageRatioChange;
		internal void OnVoltageRatioChange(VoltageRatioInputVoltageRatioChangeEventArgs e) {
			if (VoltageRatioChange != null) {
				foreach (VoltageRatioInputVoltageRatioChangeEventHandler VoltageRatioChangeHandler in VoltageRatioChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = VoltageRatioChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(VoltageRatioChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						VoltageRatioChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.VoltageRatioInputVoltageRatioChangeEvent nativeVoltageRatioChangeEventCallback;
		internal void nativeVoltageRatioChangeEvent(IntPtr phid, IntPtr ctx, double voltageRatio) {
			OnVoltageRatioChange(new VoltageRatioInputVoltageRatioChangeEventArgs(voltageRatio));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getBridgeEnabled(IntPtr phid, out bool BridgeEnabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setBridgeEnabled(IntPtr phid, bool BridgeEnabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getBridgeGain(IntPtr phid, out BridgeGain BridgeGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setBridgeGain(IntPtr phid, BridgeGain BridgeGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getSensorType(IntPtr phid, out VoltageRatioSensorType SensorType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setSensorType(IntPtr phid, VoltageRatioSensorType SensorType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getSensorUnit(IntPtr phid, IntPtr SensorUnit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getSensorValue(IntPtr phid, out double SensorValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getSensorValueChangeTrigger(IntPtr phid, out double SensorValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setSensorValueChangeTrigger(IntPtr phid, double SensorValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getVoltageRatio(IntPtr phid, out double VoltageRatio);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMinVoltageRatio(IntPtr phid, out double MinVoltageRatio);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMaxVoltageRatio(IntPtr phid, out double MaxVoltageRatio);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getVoltageRatioChangeTrigger(IntPtr phid, out double VoltageRatioChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setVoltageRatioChangeTrigger(IntPtr phid, double VoltageRatioChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMinVoltageRatioChangeTrigger(IntPtr phid, out double MinVoltageRatioChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_getMaxVoltageRatioChangeTrigger(IntPtr phid, out double MaxVoltageRatioChangeTrigger);
		public delegate void VoltageRatioInputSensorChangeEvent(IntPtr phid, IntPtr ctx, double sensorValue, IntPtr sensorUnit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setOnSensorChangeHandler(IntPtr phid, VoltageRatioInputSensorChangeEvent fptr, IntPtr ctx);
		public delegate void VoltageRatioInputVoltageRatioChangeEvent(IntPtr phid, IntPtr ctx, double voltageRatio);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageRatioInput_setOnVoltageRatioChangeHandler(IntPtr phid, VoltageRatioInputVoltageRatioChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A VoltageRatioInput SensorChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A VoltageRatioInputSensorChangeEventArg object contains data and information related to the Event.</param>
	public delegate void VoltageRatioInputSensorChangeEventHandler(object sender, VoltageRatioInputSensorChangeEventArgs e);
	/// <summary> VoltageRatioInput SensorChange Event data </summary>
	public class VoltageRatioInputSensorChangeEventArgs : EventArgs {
		/// <summary>The sensor value
		/// </summary>
		public readonly double SensorValue;
		/// <summary>The sensor unit information corresponding to the <c>SensorValue</c>.
		/// <list>
		/// <item>Helps keep track of the type of information being calculated from the voltage ratio input.</item>
		/// </list>
		/// 
		/// </summary>
		public readonly UnitInfo SensorUnit;
		internal VoltageRatioInputSensorChangeEventArgs(double sensorValue, UnitInfo sensorUnit) {
			this.SensorValue = sensorValue;
			this.SensorUnit = sensorUnit;
		}
	}

	/// <summary> A VoltageRatioInput VoltageRatioChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A VoltageRatioInputVoltageRatioChangeEventArg object contains data and information related to the Event.</param>
	public delegate void VoltageRatioInputVoltageRatioChangeEventHandler(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e);
	/// <summary> VoltageRatioInput VoltageRatioChange Event data </summary>
	public class VoltageRatioInputVoltageRatioChangeEventArgs : EventArgs {
		/// <summary>The voltage ratio
		/// </summary>
		public readonly double VoltageRatio;
		internal VoltageRatioInputVoltageRatioChangeEventArgs(double voltageRatio) {
			this.VoltageRatio = voltageRatio;
		}
	}

}
