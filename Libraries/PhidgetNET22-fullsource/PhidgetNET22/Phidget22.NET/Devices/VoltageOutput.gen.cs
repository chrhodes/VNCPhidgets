using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> VoltageOutput class definition </summary>
	public partial class VoltageOutput : Phidget {
		#region Constructor/Destructor
		/// <summary> VoltageOutput Constructor </summary>
		public VoltageOutput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetVoltageOutput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> VoltageOutput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~VoltageOutput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetVoltageOutput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The enabled value </summary>
		/// <remarks>Enable the output voltage by setting <c>Enabled</c> to true.
		/// <list>
		/// <item>Disable the output by seting <c>Enabled</c> to false to save power.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Enabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetVoltageOutput_getEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageOutput_setEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The voltage value that the channel will output.
		/// <list>
		/// <item>The <c>Voltage</c> value is bounded by <c>MinVoltage</c> and
		/// <c>MaxVoltage</c>.</item>
		/// <item>The voltage value will not be output until <c>Enabled</c> is set to true.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Voltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageOutput_getVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageOutput_setVoltage(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The minimum value that <c>Voltage</c> can be set to.
		/// </remarks>
		public double MinVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageOutput_getMinVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The maximum value that <c>Voltage</c> can be set to.
		/// </remarks>
		public double MaxVoltage {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetVoltageOutput_getMaxVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The output range value </summary>
		/// <remarks>Choose a <c>VoltageOutputRange</c> that best suits your application.
		/// <list>
		/// <item>Changing the <c>VoltageOutputRange</c> will also affect the <c>MinVoltage</c> and
		/// <c>MaxVoltage</c> values.</item>
		/// </list>
		/// 
		/// </remarks>
		public VoltageOutputRange VoltageOutputRange {
			get {
				ErrorCode result;
				VoltageOutputRange val;
				result = Phidget22Imports.PhidgetVoltageOutput_getVoltageOutputRange(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageOutput_setVoltageOutputRange(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods

		/// <summary>The voltage value </summary>
		/// <remarks>The voltage value that the channel will output.
		/// <list>
		/// <item>The <c>Voltage</c> value is bounded by <c>MinVoltage</c> and
		/// <c>MaxVoltage</c>.</item>
		/// <item>The voltage value will not be output until <c>Enabled</c> is set to true.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetVoltage(double voltage, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetVoltage");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetVoltageOutput_setVoltage_async(chandle, voltage, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetVoltage</param>
		public void EndSetVoltage(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetVoltage");
		}
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_setVoltage_async(IntPtr phid, double Voltage, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_getEnabled(IntPtr phid, out bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_setEnabled(IntPtr phid, bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_getVoltage(IntPtr phid, out double Voltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_setVoltage(IntPtr phid, double Voltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_getMinVoltage(IntPtr phid, out double MinVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_getMaxVoltage(IntPtr phid, out double MaxVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_getVoltageOutputRange(IntPtr phid, out VoltageOutputRange VoltageOutputRange);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetVoltageOutput_setVoltageOutputRange(IntPtr phid, VoltageOutputRange VoltageOutputRange);
	}
}

namespace Phidget22.Events {
}
