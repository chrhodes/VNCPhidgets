using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> DigitalOutput class definition </summary>
	public partial class DigitalOutput : Phidget {
		#region Constructor/Destructor
		/// <summary> DigitalOutput Constructor </summary>
		public DigitalOutput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDigitalOutput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> DigitalOutput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~DigitalOutput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDigitalOutput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The duty cycle value </summary>
		/// <remarks>The <c>DutyCycle</c> represents the fraction of time the output is on (high).
		/// <list>
		/// <item>A <c>DutyCycle</c> of 1.0 translates to a high output, a <c>DutyCycle</c> of 0
		/// translates to a low output.</item>
		/// <item>A <c>DutyCycle</c> of 0.5 translates to an output that is high half the time, which
		/// results in an average output voltage of (output voltage x 0.5)</item>
		/// <item>You can use the <c>DutyCycle</c> to create a dimming effect on LEDs.</item>
		/// </list>
		/// 
		/// </remarks>
		public double DutyCycle {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getDutyCycle(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setDutyCycle(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The duty cycle value </summary>
		/// <remarks>The minimum value that <c>DutyCycle</c> can be set to.
		/// </remarks>
		public double MinDutyCycle {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getMinDutyCycle(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The duty cycle value </summary>
		/// <remarks>The maximum value that <c>DutyCycle</c> can be set to.
		/// </remarks>
		public double MaxDutyCycle {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getMaxDutyCycle(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current limit value </summary>
		/// <remarks>The <c>LEDCurrentLimit</c> is the maximum amount of current that the controller will provide
		/// to the output.
		/// <list>
		/// <item>Reference the data sheet of the LED you are using before setting this value.</item>
		/// </list>
		/// 
		/// </remarks>
		public double LEDCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getLEDCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setLEDCurrentLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current limit value </summary>
		/// <remarks>The minimum value that <c>LEDCurrentLimit</c> can be set to.
		/// </remarks>
		public double MinLEDCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getMinLEDCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current limit value </summary>
		/// <remarks>The maximum value that <c>LEDCurrentLimit</c> can be set to.
		/// </remarks>
		public double MaxLEDCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDigitalOutput_getMaxLEDCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The forward voltage value </summary>
		/// <remarks>The <c>LEDForwardVoltage</c> is the voltage that will be available to your LED.
		/// <list>
		/// <item>Reference the data sheet of the LED you are using before setting this value. Choose the
		/// <c>LEDForwardVoltage</c> that is closest to the forward voltage specified in the data
		/// sheet.</item>
		/// <item>This forward voltage is shared for all channels on this device. Setting the LEDForwardVoltage
		/// on any channel will set the LEDForwardVoltage for all channels on the device.</item>
		/// </list>
		/// 
		/// </remarks>
		public LEDForwardVoltage LEDForwardVoltage {
			get {
				ErrorCode result;
				LEDForwardVoltage val;
				result = Phidget22Imports.PhidgetDigitalOutput_getLEDForwardVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setLEDForwardVoltage(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The state value </summary>
		/// <remarks>The <c>State</c> will indicate whether the output is high (TRUE) or low (FALSE).
		/// <list>
		/// <item>If a <c>DutyCycle</c> has been set, the state will return as TRUE if the DutyCycle is
		/// above 0.5, or FALSE otherwise.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool State {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetDigitalOutput_getState(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setState(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods

		/// <summary>The duty cycle value </summary>
		/// <remarks>The <c>DutyCycle</c> represents the fraction of time the output is on (high).
		/// <list>
		/// <item>This will override the <c>State</c> setting on the channel.</item>
		/// <item>A <c>DutyCycle</c> of 1.0 translates to a high output, a <c>DutyCycle</c> of 0
		/// translates to a low output.</item>
		/// </list>
		/// <list>
		/// <item>This is equivalent to setting a <c>State</c> of TRUE and FALSE respectively.</item>
		/// </list>
		/// <list>
		/// <item>A <c>DutyCycle</c> of 0.5 translates to an output that is high half the time, which
		/// results in an average output voltage of (output voltage x 0.5)</item>
		/// <item>You can use the <c>DutyCycle</c> to create a dimming effect on LEDs.</item>
		/// <item>If the DigitalOutput channel you are using does not support PWM, then this value may only be
		/// set to 1.0 or 0.0</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetDutyCycle(double dutyCycle, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetDutyCycle");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setDutyCycle_async(chandle, dutyCycle, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetDutyCycle</param>
		public void EndSetDutyCycle(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetDutyCycle");
		}

		/// <summary>The current limit value </summary>
		/// <remarks>The <c>LEDCurrentLimit</c> is the maximum amount of current that the controller will provide
		/// to the output.
		/// <list>
		/// <item>Reference the data sheet of the LED you are using before setting this value.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetLEDCurrentLimit(double LEDCurrentLimit, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetLEDCurrentLimit");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setLEDCurrentLimit_async(chandle, LEDCurrentLimit, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetLEDCurrentLimit</param>
		public void EndSetLEDCurrentLimit(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetLEDCurrentLimit");
		}

		/// <summary>The state value </summary>
		/// <remarks>The <c>State</c> will dictate whether the output is constantly high (TRUE) or low (FALSE).
		/// <list>
		/// <item>This will override any <c>DutyCycle</c> that may have been set on the channel.</item>
		/// <item>Setting the <c>State</c> to TRUE is the same as setting <c>DutyCycle</c> to 1.0,
		/// and setting the <c>State</c> to FALSE is the same as setting a <c>DutyCycle</c> of
		/// 0.0.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetState(bool state, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetState");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalOutput_setState_async(chandle, state, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetState</param>
		public void EndSetState(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetState");
		}
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setDutyCycle_async(IntPtr phid, double DutyCycle, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setLEDCurrentLimit_async(IntPtr phid, double LEDCurrentLimit, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setState_async(IntPtr phid, bool State, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getDutyCycle(IntPtr phid, out double DutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setDutyCycle(IntPtr phid, double DutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getMinDutyCycle(IntPtr phid, out double MinDutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getMaxDutyCycle(IntPtr phid, out double MaxDutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getLEDCurrentLimit(IntPtr phid, out double LEDCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setLEDCurrentLimit(IntPtr phid, double LEDCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getMinLEDCurrentLimit(IntPtr phid, out double MinLEDCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getMaxLEDCurrentLimit(IntPtr phid, out double MaxLEDCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getLEDForwardVoltage(IntPtr phid, out LEDForwardVoltage LEDForwardVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setLEDForwardVoltage(IntPtr phid, LEDForwardVoltage LEDForwardVoltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_getState(IntPtr phid, out bool State);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalOutput_setState(IntPtr phid, bool State);
	}
}

namespace Phidget22.Events {
}
