using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> CurrentInput class definition </summary>
	public partial class CurrentInput : Phidget {
		#region Constructor/Destructor
		/// <summary> CurrentInput Constructor </summary>
		public CurrentInput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetCurrentInput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> CurrentInput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~CurrentInput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetCurrentInput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The current value </summary>
		/// <remarks>The most recent current value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinCurrent</c> and <c>MaxCurrent</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Current {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getCurrent(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The minimum value the <c>CurrentChange</c> event will report.
		/// </remarks>
		public double MinCurrent {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getMinCurrent(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The maximum value the <c>CurrentChange</c> event will report.
		/// </remarks>
		public double MaxCurrent {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getMaxCurrent(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>CurrentChange</c> event until the current value has changed
		/// by the amount specified by the <c>CurrentChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>CurrentChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double CurrentChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getCurrentChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCurrentInput_setCurrentChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>CurrentChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinCurrentChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getMinCurrentChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>CurrentChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxCurrentChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCurrentInput_getMaxCurrentChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>CurrentChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>CurrentChange</c> events can also affected by the
		/// <c>CurrentChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetCurrentInput_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCurrentInput_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetCurrentInput_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetCurrentInput_getMaxDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetCurrentInput_getPowerSupply(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCurrentInput_setPowerSupply(chandle, value);
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
			nativeCurrentChangeEventCallback = new Phidget22Imports.CurrentInputCurrentChangeEvent(nativeCurrentChangeEvent);
			result = Phidget22Imports.PhidgetCurrentInput_setOnCurrentChangeHandler(chandle, nativeCurrentChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetCurrentInput_setOnCurrentChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent current value the channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>CurrentChangeTrigger</c> has been set to a non-zero value, the
		/// <c>CurrentChange</c> event will not occur until the current value has changed by at least the
		/// <c>CurrentChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event CurrentInputCurrentChangeEventHandler CurrentChange;
		internal void OnCurrentChange(CurrentInputCurrentChangeEventArgs e) {
			if (CurrentChange != null) {
				foreach (CurrentInputCurrentChangeEventHandler CurrentChangeHandler in CurrentChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = CurrentChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(CurrentChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						CurrentChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.CurrentInputCurrentChangeEvent nativeCurrentChangeEventCallback;
		internal void nativeCurrentChangeEvent(IntPtr phid, IntPtr ctx, double current) {
			OnCurrentChange(new CurrentInputCurrentChangeEventArgs(current));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getCurrent(IntPtr phid, out double Current);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMinCurrent(IntPtr phid, out double MinCurrent);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMaxCurrent(IntPtr phid, out double MaxCurrent);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getCurrentChangeTrigger(IntPtr phid, out double CurrentChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_setCurrentChangeTrigger(IntPtr phid, double CurrentChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMinCurrentChangeTrigger(IntPtr phid, out double MinCurrentChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMaxCurrentChangeTrigger(IntPtr phid, out double MaxCurrentChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_getPowerSupply(IntPtr phid, out PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_setPowerSupply(IntPtr phid, PowerSupply PowerSupply);
		public delegate void CurrentInputCurrentChangeEvent(IntPtr phid, IntPtr ctx, double current);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCurrentInput_setOnCurrentChangeHandler(IntPtr phid, CurrentInputCurrentChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A CurrentInput CurrentChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A CurrentInputCurrentChangeEventArg object contains data and information related to the Event.</param>
	public delegate void CurrentInputCurrentChangeEventHandler(object sender, CurrentInputCurrentChangeEventArgs e);
	/// <summary> CurrentInput CurrentChange Event data </summary>
	public class CurrentInputCurrentChangeEventArgs : EventArgs {
		/// <summary>The current value
		/// </summary>
		public readonly double Current;
		internal CurrentInputCurrentChangeEventArgs(double current) {
			this.Current = current;
		}
	}

}
