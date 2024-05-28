using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> ResistanceInput class definition </summary>
	public partial class ResistanceInput : Phidget {
		#region Constructor/Destructor
		/// <summary> ResistanceInput Constructor </summary>
		public ResistanceInput() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetResistanceInput_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> ResistanceInput Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~ResistanceInput() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetResistanceInput_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>ResistanceChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>ResistanceChange</c> events can also affected by the
		/// <c>ResistanceChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetResistanceInput_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetResistanceInput_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetResistanceInput_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetResistanceInput_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The resistance value </summary>
		/// <remarks>The most recent resistance value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinResistance</c> and
		/// <c>MaxResistance</c>.</item>
		/// <item>The <c>Resistance</c> value will change when the device is also being used as a
		/// temperature sensor. This is a side effect of increasing accuracy on the temperature channel.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Resistance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getResistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The minimum resistance </summary>
		/// <remarks>The minimum value the <c>ResistanceChange</c> event will report.
		/// <list>
		/// <item>When the device is also being used as a TemperatureSensor the <c>MinResistance</c> and
		/// <c>MaxResistance</c> will not represent the true input range. This is a side effect of
		/// increasing accuracy on the temperature channel.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinResistance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getMinResistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The resistance value </summary>
		/// <remarks>The maximum value the <c>ResistanceChange</c> event will report.
		/// </remarks>
		public double MaxResistance {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getMaxResistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>ResistanceChange</c> event until the resistance value has
		/// changed by the amount specified by the <c>ResistanceChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>ResistanceChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double ResistanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getResistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetResistanceInput_setResistanceChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that the <c>ResistanceChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinResistanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getMinResistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>ResistanceChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxResistanceChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetResistanceInput_getMaxResistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Wire setup value </summary>
		/// <remarks>Select the RTD wiring configuration.
		/// <list>
		/// <item>More information about RTD wiring can be found in the user guide.</item>
		/// </list>
		/// 
		/// </remarks>
		public RTDWireSetup RTDWireSetup {
			get {
				ErrorCode result;
				RTDWireSetup val;
				result = Phidget22Imports.PhidgetResistanceInput_getRTDWireSetup(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetResistanceInput_setRTDWireSetup(chandle, value);
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
			nativeResistanceChangeEventCallback = new Phidget22Imports.ResistanceInputResistanceChangeEvent(nativeResistanceChangeEvent);
			result = Phidget22Imports.PhidgetResistanceInput_setOnResistanceChangeHandler(chandle, nativeResistanceChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetResistanceInput_setOnResistanceChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent resistance value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>ResistanceChangeTrigger</c> has been set to a non-zero value, the
		/// <c>ResistanceChange</c> event will not occur until the resistance has changed by at least the
		/// <c>ResistanceChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event ResistanceInputResistanceChangeEventHandler ResistanceChange;
		internal void OnResistanceChange(ResistanceInputResistanceChangeEventArgs e) {
			if (ResistanceChange != null) {
				foreach (ResistanceInputResistanceChangeEventHandler ResistanceChangeHandler in ResistanceChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = ResistanceChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(ResistanceChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						ResistanceChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.ResistanceInputResistanceChangeEvent nativeResistanceChangeEventCallback;
		internal void nativeResistanceChangeEvent(IntPtr phid, IntPtr ctx, double resistance) {
			OnResistanceChange(new ResistanceInputResistanceChangeEventArgs(resistance));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getResistance(IntPtr phid, out double Resistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMinResistance(IntPtr phid, out double MinResistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMaxResistance(IntPtr phid, out double MaxResistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getResistanceChangeTrigger(IntPtr phid, out double ResistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_setResistanceChangeTrigger(IntPtr phid, double ResistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMinResistanceChangeTrigger(IntPtr phid, out double MinResistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getMaxResistanceChangeTrigger(IntPtr phid, out double MaxResistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_getRTDWireSetup(IntPtr phid, out RTDWireSetup RTDWireSetup);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_setRTDWireSetup(IntPtr phid, RTDWireSetup RTDWireSetup);
		public delegate void ResistanceInputResistanceChangeEvent(IntPtr phid, IntPtr ctx, double resistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetResistanceInput_setOnResistanceChangeHandler(IntPtr phid, ResistanceInputResistanceChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A ResistanceInput ResistanceChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A ResistanceInputResistanceChangeEventArg object contains data and information related to the Event.</param>
	public delegate void ResistanceInputResistanceChangeEventHandler(object sender, ResistanceInputResistanceChangeEventArgs e);
	/// <summary> ResistanceInput ResistanceChange Event data </summary>
	public class ResistanceInputResistanceChangeEventArgs : EventArgs {
		/// <summary>The resistance value
		/// </summary>
		public readonly double Resistance;
		internal ResistanceInputResistanceChangeEventArgs(double resistance) {
			this.Resistance = resistance;
		}
	}

}
