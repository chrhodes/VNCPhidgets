using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> PressureSensor class definition </summary>
	public partial class PressureSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> PressureSensor Constructor </summary>
		public PressureSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetPressureSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> PressureSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~PressureSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetPressureSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>PressureChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>PressureChange</c> events can also affected by the
		/// <c>PressureChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetPressureSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPressureSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetPressureSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetPressureSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pressure value </summary>
		/// <remarks>The most recent pressure value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinPressure</c> and <c>MaxPressure</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Pressure {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getPressure(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pressure value </summary>
		/// <remarks>The minimum value the <c>PressureChange</c> event will report.
		/// </remarks>
		public double MinPressure {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getMinPressure(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pressure value </summary>
		/// <remarks>The maximum value the <c>PressureChange</c> event will report.
		/// </remarks>
		public double MaxPressure {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getMaxPressure(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>PressureChange</c> event until the pressure value has changed
		/// by the amount specified by the <c>PressureChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>PressureChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double PressureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getPressureChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetPressureSensor_setPressureChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>PressureChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinPressureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getMinPressureChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>PressureChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxPressureChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetPressureSensor_getMaxPressureChangeTrigger(chandle, out val);
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
			nativePressureChangeEventCallback = new Phidget22Imports.PressureSensorPressureChangeEvent(nativePressureChangeEvent);
			result = Phidget22Imports.PhidgetPressureSensor_setOnPressureChangeHandler(chandle, nativePressureChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetPressureSensor_setOnPressureChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent pressure value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>PressureChangeTrigger</c> has been set to a non-zero value, the
		/// <c>PressureChange</c> event will not occur until the pressure has changed by at least the
		/// <c>PressureChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event PressureSensorPressureChangeEventHandler PressureChange;
		internal void OnPressureChange(PressureSensorPressureChangeEventArgs e) {
			if (PressureChange != null) {
				foreach (PressureSensorPressureChangeEventHandler PressureChangeHandler in PressureChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PressureChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PressureChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PressureChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.PressureSensorPressureChangeEvent nativePressureChangeEventCallback;
		internal void nativePressureChangeEvent(IntPtr phid, IntPtr ctx, double pressure) {
			OnPressureChange(new PressureSensorPressureChangeEventArgs(pressure));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getPressure(IntPtr phid, out double Pressure);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMinPressure(IntPtr phid, out double MinPressure);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMaxPressure(IntPtr phid, out double MaxPressure);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getPressureChangeTrigger(IntPtr phid, out double PressureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_setPressureChangeTrigger(IntPtr phid, double PressureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMinPressureChangeTrigger(IntPtr phid, out double MinPressureChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_getMaxPressureChangeTrigger(IntPtr phid, out double MaxPressureChangeTrigger);
		public delegate void PressureSensorPressureChangeEvent(IntPtr phid, IntPtr ctx, double pressure);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetPressureSensor_setOnPressureChangeHandler(IntPtr phid, PressureSensorPressureChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A PressureSensor PressureChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A PressureSensorPressureChangeEventArg object contains data and information related to the Event.</param>
	public delegate void PressureSensorPressureChangeEventHandler(object sender, PressureSensorPressureChangeEventArgs e);
	/// <summary> PressureSensor PressureChange Event data </summary>
	public class PressureSensorPressureChangeEventArgs : EventArgs {
		/// <summary>The new measured pressure
		/// </summary>
		public readonly double Pressure;
		internal PressureSensorPressureChangeEventArgs(double pressure) {
			this.Pressure = pressure;
		}
	}

}
