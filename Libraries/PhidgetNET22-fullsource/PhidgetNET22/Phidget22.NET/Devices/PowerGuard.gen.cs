using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> PowerGuard class definition </summary>
	public partial class PowerGuard : Phidget {
		#region Constructor/Destructor
		/// <summary> PowerGuard Constructor </summary>
		public PowerGuard() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetPowerGuard_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> PowerGuard Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~PowerGuard() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetPowerGuard_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The fan mode value </summary>
		/// <remarks>The <c>FanMode</c> dictates the operating condition of the fan.
		/// <list>
		/// <item>Choose between on, off, or automatic (based on temperature).</item>
		/// <item>If the <c>FanMode</c> is set to automatic, the fan will turn on when the temperature
		/// reaches 70°C and it will remain on until the temperature falls below 55°C.</item>
		/// <item>If the <c>FanMode</c> is off, the device will still turn on the fan if the temperature
		/// reaches 85°C and it will remain on until it falls below 70°C.</item>
		/// </list>
		/// 
		/// </remarks>
		public FanMode FanMode {
			get {
				ErrorCode result;
				FanMode val;
				result = Phidget22Imports.PhidgetPowerGuard_getFanMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPowerGuard_setFanMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The device constantly monitors the output voltage, and if it exceeds the <c>OverVoltage</c>
		/// value, it will disconnect the input from the output.
		/// <list>
		/// <item>This functionality is critical for protecting power supplies from regenerated voltage coming
		/// from motors. Many power supplies assume that a higher than output expected voltage is related to an
		/// internal failure to the power supply, and will permanently disable themselves to protect the
		/// system. A typical safe value is to set OverVoltage to 1-2 volts higher than the output voltage of
		/// the supply. For instance, a 12V supply would be protected by setting OverVoltage to 13V.</item>
		/// <item>The device will connect the input to the output again when the voltage drops to
		/// (<c>OverVoltage</c> - 1V)</item>
		/// </list>
		/// 
		/// </remarks>
		public double OverVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPowerGuard_getOverVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPowerGuard_setOverVoltage(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The minimum value that <c>OverVoltage</c> can be set to.
		/// </remarks>
		public double MinOverVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPowerGuard_getMinOverVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The maximum value that <c>OverVoltage</c> can be set to.
		/// </remarks>
		public double MaxOverVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPowerGuard_getMaxOverVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The power enabled value. </summary>
		/// <remarks>When <c>PowerEnabled</c> is true, the device will connect the input to the output and begin
		/// monitoring.
		/// <list>
		/// <item>The output voltage is constantly monitored and will be automatically disconnected from the
		/// input when the output exceeds the <c>OverVoltage</c> value.</item>
		/// <item><c>PowerEnabled</c> allows the device to operate as a Solid State Relay, powering on or
		/// off all devices connected to the output.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool PowerEnabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetPowerGuard_getPowerEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPowerGuard_setPowerEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_getFanMode(IntPtr phid, out FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_setFanMode(IntPtr phid, FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_getOverVoltage(IntPtr phid, out double OverVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_setOverVoltage(IntPtr phid, double OverVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_getMinOverVoltage(IntPtr phid, out double MinOverVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_getMaxOverVoltage(IntPtr phid, out double MaxOverVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_getPowerEnabled(IntPtr phid, out bool PowerEnabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPowerGuard_setPowerEnabled(IntPtr phid, bool PowerEnabled);
	}
}

namespace Phidget22.Events {
}
