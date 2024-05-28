using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> CapacitiveTouch class definition </summary>
	public partial class CapacitiveTouch : Phidget {
		#region Constructor/Destructor
		/// <summary> CapacitiveTouch Constructor </summary>
		public CapacitiveTouch() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetCapacitiveTouch_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> CapacitiveTouch Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~CapacitiveTouch() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetCapacitiveTouch_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>Touch</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>Touch</c> events can also affected by the
		/// <c>TouchValueChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCapacitiveTouch_setDataInterval(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The minimum data interval value </summary>
		/// <remarks>The minimum value that <c>DataInterval</c> can be set to.
		/// </remarks>
		public int MinDataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The sensitivity value </summary>
		/// <remarks>Determines the sensitivity of all capacitive regions on the device.
		/// <list>
		/// <item>Higher values result in greater touch sensitivity.</item>
		/// <item>The sensitivity value is bounded by <c>MinSensitivity</c> and
		/// <c>MaxSensitivity</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Sensitivity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getSensitivity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCapacitiveTouch_setSensitivity(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The minimum sensitivity value </summary>
		/// <remarks>The minimum value that <c>Sensitivity</c> can be set to.
		/// </remarks>
		public double MinSensitivity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMinSensitivity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum sensitivity value </summary>
		/// <remarks>The maximum value that <c>Sensitivity</c> can be set to.
		/// </remarks>
		public double MaxSensitivity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMaxSensitivity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The touched state </summary>
		/// <remarks>The most recent touch state that the channel has reported.
		/// <list>
		/// <item>This will be 0 or 1</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>0 is not touched</item>
		/// <item>1 is touched</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public bool IsTouched {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getIsTouched(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The touch input value </summary>
		/// <remarks>The most recent touch value that the channel has reported.
		/// <list>
		/// <item>This will be 0 or 1 for button-type inputs, or a ratio between 0-1 for axis-type inputs.</item>
		/// <item>This value is bounded by <c>MinTouchValue</c> and <c>MaxTouchValue</c></item>
		/// <item>The value is not reset when the touch ends</item>
		/// </list>
		/// 
		/// </remarks>
		public double TouchValue {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getTouchValue(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The minimum touch input value </summary>
		/// <remarks>The minimum value the <c>Touch</c> event will report.
		/// </remarks>
		public double MinTouchValue {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMinTouchValue(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum touch input value </summary>
		/// <remarks>The maximum value the <c>Touch</c> event will report.
		/// </remarks>
		public double MaxTouchValue {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMaxTouchValue(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>Touch</c> event until the touch value has changed by the
		/// amount specified by the <c>TouchValueChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>TouchValueChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double TouchValueChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getTouchValueChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetCapacitiveTouch_setTouchValueChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The minimum change trigger value </summary>
		/// <remarks>The minimum value that <c>TouchValueChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinTouchValueChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMinTouchValueChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum change trigger value </summary>
		/// <remarks>The maximum value that <c>TouchValueChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxTouchValueChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetCapacitiveTouch_getMaxTouchValueChangeTrigger(chandle, out val);
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
			nativeTouchEventCallback = new Phidget22Imports.CapacitiveTouchTouchEvent(nativeTouchEvent);
			result = Phidget22Imports.PhidgetCapacitiveTouch_setOnTouchHandler(chandle, nativeTouchEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeTouchEndEventCallback = new Phidget22Imports.CapacitiveTouchTouchEndEvent(nativeTouchEndEvent);
			result = Phidget22Imports.PhidgetCapacitiveTouch_setOnTouchEndHandler(chandle, nativeTouchEndEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetCapacitiveTouch_setOnTouchHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetCapacitiveTouch_setOnTouchEndHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent touch value the channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>TouchValueChangeTrigger</c> has been set to a non-zero value, the
		/// <c>Touch</c> event will not occur until the touch value has changed by at least the
		/// <c>TouchValueChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event CapacitiveTouchTouchEventHandler Touch;
		internal void OnTouch(CapacitiveTouchTouchEventArgs e) {
			if (Touch != null) {
				foreach (CapacitiveTouchTouchEventHandler TouchHandler in Touch.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TouchHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TouchHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TouchHandler(this, e);
				}
			}
		}
		Phidget22Imports.CapacitiveTouchTouchEvent nativeTouchEventCallback;
		internal void nativeTouchEvent(IntPtr phid, IntPtr ctx, double touchValue) {
			OnTouch(new CapacitiveTouchTouchEventArgs(touchValue));
		}
		/// <summary>  </summary>
		/// <remarks>The channel will report a <c>TouchEnd</c> event to signify that it is no longer detecting a
		/// touch.
		/// </remarks>
		public event CapacitiveTouchTouchEndEventHandler TouchEnd;
		internal void OnTouchEnd(CapacitiveTouchTouchEndEventArgs e) {
			if (TouchEnd != null) {
				foreach (CapacitiveTouchTouchEndEventHandler TouchEndHandler in TouchEnd.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TouchEndHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TouchEndHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TouchEndHandler(this, e);
				}
			}
		}
		Phidget22Imports.CapacitiveTouchTouchEndEvent nativeTouchEndEventCallback;
		internal void nativeTouchEndEvent(IntPtr phid, IntPtr ctx) {
			OnTouchEnd(new CapacitiveTouchTouchEndEventArgs());
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getSensitivity(IntPtr phid, out double Sensitivity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_setSensitivity(IntPtr phid, double Sensitivity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMinSensitivity(IntPtr phid, out double MinSensitivity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMaxSensitivity(IntPtr phid, out double MaxSensitivity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getIsTouched(IntPtr phid, out bool IsTouched);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getTouchValue(IntPtr phid, out double TouchValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMinTouchValue(IntPtr phid, out double MinTouchValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMaxTouchValue(IntPtr phid, out double MaxTouchValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getTouchValueChangeTrigger(IntPtr phid, out double TouchValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_setTouchValueChangeTrigger(IntPtr phid, double TouchValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMinTouchValueChangeTrigger(IntPtr phid, out double MinTouchValueChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_getMaxTouchValueChangeTrigger(IntPtr phid, out double MaxTouchValueChangeTrigger);
		public delegate void CapacitiveTouchTouchEvent(IntPtr phid, IntPtr ctx, double touchValue);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_setOnTouchHandler(IntPtr phid, CapacitiveTouchTouchEvent fptr, IntPtr ctx);
		public delegate void CapacitiveTouchTouchEndEvent(IntPtr phid, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetCapacitiveTouch_setOnTouchEndHandler(IntPtr phid, CapacitiveTouchTouchEndEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A CapacitiveTouch Touch Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A CapacitiveTouchTouchEventArg object contains data and information related to the Event.</param>
	public delegate void CapacitiveTouchTouchEventHandler(object sender, CapacitiveTouchTouchEventArgs e);
	/// <summary> CapacitiveTouch Touch Event data </summary>
	public class CapacitiveTouchTouchEventArgs : EventArgs {
		/// <summary>Value of the touch input axis.
		/// </summary>
		public readonly double TouchValue;
		internal CapacitiveTouchTouchEventArgs(double touchValue) {
			this.TouchValue = touchValue;
		}
	}

	/// <summary> A CapacitiveTouch TouchEnd Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A CapacitiveTouchTouchEndEventArg object contains data and information related to the Event.</param>
	public delegate void CapacitiveTouchTouchEndEventHandler(object sender, CapacitiveTouchTouchEndEventArgs e);
	/// <summary> CapacitiveTouch TouchEnd Event data </summary>
	public class CapacitiveTouchTouchEndEventArgs : EventArgs {
		internal CapacitiveTouchTouchEndEventArgs() {
		}
	}

}
