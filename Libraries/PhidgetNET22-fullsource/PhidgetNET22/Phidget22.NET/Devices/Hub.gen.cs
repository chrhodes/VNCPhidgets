using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Hub class definition </summary>
	public partial class Hub : Phidget {
		#region Constructor/Destructor
		/// <summary> Hub Constructor </summary>
		public Hub() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHub_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Hub Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Hub() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetHub_delete(ref chandle);
			}
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Writes ADC calibration values to a VINT Hub. Used for manufacturing.
		/// </remarks>
		public void SetADCCalibrationValues(double[] voltageInputGain, double[] voltageRatioGain) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHub_setADCCalibrationValues(chandle, voltageInputGain, voltageRatioGain);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Sets a flag on this hub port which forces the next VINT devices plugged in to stay in firmware
		/// upgrade mode.
		/// </remarks>
		public void SetFirmwareUpgradeFlag(int port, int timeout) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHub_setFirmwareUpgradeFlag(chandle, port, timeout);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Sets the mode of the selected port.
		/// </remarks>
		public void SetPortMode(int port, HubPortMode mode) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHub_setPortMode(chandle, port, mode);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Controls power to the VINT Hub Ports
		/// </remarks>
		public void SetPortPower(int port, bool state) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetHub_setPortPower(chandle, port, state);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_setADCCalibrationValues(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] double[] voltageInputGain, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] double[] voltageRatioGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_setFirmwareUpgradeFlag(IntPtr phid, int port, int timeout);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_setPortMode(IntPtr phid, int port, HubPortMode mode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetHub_setPortPower(IntPtr phid, int port, bool state);
	}
}

namespace Phidget22.Events {
}
