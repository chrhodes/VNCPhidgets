using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> DigitalInput class definition </summary>
	public partial class DigitalInput : Phidget {
		#region Constructor/Destructor
		/// <summary> DigitalInput Constructor </summary>
		public DigitalInput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDigitalInput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> DigitalInput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~DigitalInput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDigitalInput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The input mode value </summary>
		/// <remarks>The input polarity mode for your channel.
		/// <list>
		/// <item>See your device's User Guide for more information about what value to chooose for the
		/// <c>InputMode</c></item>
		/// </list>
		/// 
		/// </remarks>
		public InputMode InputMode {
			get {
				ErrorCode result;
				InputMode val;
				result = Phidget22Imports.PhidgetDigitalInput_getInputMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalInput_setInputMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
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
				result = Phidget22Imports.PhidgetDigitalInput_getPowerSupply(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDigitalInput_setPowerSupply(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The state value </summary>
		/// <remarks>The most recent state value that the channel has reported.
		/// </remarks>
		public bool State {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetDigitalInput_getState(chandle, out val);
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
			nativeStateChangeEventCallback = new Phidget22Imports.DigitalInputStateChangeEvent(nativeStateChangeEvent);
			result = Phidget22Imports.PhidgetDigitalInput_setOnStateChangeHandler(chandle, nativeStateChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetDigitalInput_setOnStateChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>This event will occur when the state of the digital input has changed.
		/// <list>
		/// <item>The value will either be 0 or 1 (true or false).</item>
		/// </list>
		/// 
		/// </remarks>
		public event DigitalInputStateChangeEventHandler StateChange;
		internal void OnStateChange(DigitalInputStateChangeEventArgs e) {
			if (StateChange != null) {
				foreach (DigitalInputStateChangeEventHandler StateChangeHandler in StateChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = StateChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(StateChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						StateChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.DigitalInputStateChangeEvent nativeStateChangeEventCallback;
		internal void nativeStateChangeEvent(IntPtr phid, IntPtr ctx, bool state) {
			OnStateChange(new DigitalInputStateChangeEventArgs(state));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_getInputMode(IntPtr phid, out InputMode InputMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_setInputMode(IntPtr phid, InputMode InputMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_getPowerSupply(IntPtr phid, out PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_setPowerSupply(IntPtr phid, PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_getState(IntPtr phid, out bool State);
		public delegate void DigitalInputStateChangeEvent(IntPtr phid, IntPtr ctx, bool state);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDigitalInput_setOnStateChangeHandler(IntPtr phid, DigitalInputStateChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A DigitalInput StateChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DigitalInputStateChangeEventArg object contains data and information related to the Event.</param>
	public delegate void DigitalInputStateChangeEventHandler(object sender, DigitalInputStateChangeEventArgs e);
	/// <summary> DigitalInput StateChange Event data </summary>
	public class DigitalInputStateChangeEventArgs : EventArgs {
		/// <summary>The state value
		/// </summary>
		public readonly bool State;
		internal DigitalInputStateChangeEventArgs(bool state) {
			this.State = state;
		}
	}

}
