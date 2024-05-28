using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> FirmwareUpgrade class definition </summary>
	public partial class FirmwareUpgrade : Phidget {
		#region Constructor/Destructor
		/// <summary> FirmwareUpgrade Constructor </summary>
		public FirmwareUpgrade() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetFirmwareUpgrade_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> FirmwareUpgrade Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~FirmwareUpgrade() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetFirmwareUpgrade_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> Device ID </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public DeviceID ActualDeviceID {
			get {
				ErrorCode result;
				DeviceID val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getActualDeviceID(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Name of the device </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public string ActualDeviceName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getActualDeviceName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> Device SKU </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public string ActualDeviceSKU {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getActualDeviceSKU(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> Firmware version </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public int ActualDeviceVersion {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getActualDeviceVersion(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Device VINT ID </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public int ActualDeviceVINTID {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getActualDeviceVINTID(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Firmware update progress </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public double Progress {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFirmwareUpgrade_getProgress(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void SendFirmware(byte[] data) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.PhidgetFirmwareUpgrade_sendFirmware(chandle, data, dataLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeProgressChangeEventCallback = new Phidget22Imports.FirmwareUpgradeProgressChangeEvent(nativeProgressChangeEvent);
			result = Phidget22Imports.PhidgetFirmwareUpgrade_setOnProgressChangeHandler(chandle, nativeProgressChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetFirmwareUpgrade_setOnProgressChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs on firmware upgrade progress.
		/// </remarks>
		public event FirmwareUpgradeProgressChangeEventHandler ProgressChange;
		internal void OnProgressChange(FirmwareUpgradeProgressChangeEventArgs e) {
			if (ProgressChange != null) {
				foreach (FirmwareUpgradeProgressChangeEventHandler ProgressChangeHandler in ProgressChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = ProgressChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(ProgressChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						ProgressChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.FirmwareUpgradeProgressChangeEvent nativeProgressChangeEventCallback;
		internal void nativeProgressChangeEvent(IntPtr phid, IntPtr ctx, double progress) {
			OnProgressChange(new FirmwareUpgradeProgressChangeEventArgs(progress));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_sendFirmware(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getActualDeviceID(IntPtr phid, out DeviceID ActualDeviceID);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getActualDeviceName(IntPtr phid, out IntPtr ActualDeviceName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getActualDeviceSKU(IntPtr phid, out IntPtr ActualDeviceSKU);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getActualDeviceVersion(IntPtr phid, out int ActualDeviceVersion);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getActualDeviceVINTID(IntPtr phid, out int ActualDeviceVINTID);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_getProgress(IntPtr phid, out double Progress);
		public delegate void FirmwareUpgradeProgressChangeEvent(IntPtr phid, IntPtr ctx, double progress);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFirmwareUpgrade_setOnProgressChangeHandler(IntPtr phid, FirmwareUpgradeProgressChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A FirmwareUpgrade ProgressChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A FirmwareUpgradeProgressChangeEventArg object contains data and information related to the Event.</param>
	public delegate void FirmwareUpgradeProgressChangeEventHandler(object sender, FirmwareUpgradeProgressChangeEventArgs e);
	/// <summary> FirmwareUpgrade ProgressChange Event data </summary>
	public class FirmwareUpgradeProgressChangeEventArgs : EventArgs {
		/// <summary>The progress, range is 0-1.
		/// </summary>
		public readonly double Progress;
		internal FirmwareUpgradeProgressChangeEventArgs(double progress) {
			this.Progress = progress;
		}
	}

}
