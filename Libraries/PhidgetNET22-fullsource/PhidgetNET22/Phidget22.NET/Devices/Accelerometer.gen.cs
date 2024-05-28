using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Accelerometer class definition </summary>
	public partial class Accelerometer : Phidget {
		#region Constructor/Destructor
		/// <summary> Accelerometer Constructor </summary>
		public Accelerometer() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetAccelerometer_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Accelerometer Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Accelerometer() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetAccelerometer_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The acceleration values </summary>
		/// <remarks>The most recent acceleration value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinAcceleration</c> and
		/// <c>MaxAcceleration</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double[] Acceleration {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetAccelerometer_getAcceleration(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The minimum acceleration value </summary>
		/// <remarks>The minimum value the <c>AccelerationChange</c> event will report.
		/// </remarks>
		public double[] MinAcceleration {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetAccelerometer_getMinAcceleration(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum acceleration values </summary>
		/// <remarks>The maximum value the <c>AccelerationChange</c> event will report.
		/// </remarks>
		public double[] MaxAcceleration {
			get {
				ErrorCode result;
				double[] val = new double[3];
				result = Phidget22Imports.PhidgetAccelerometer_getMaxAcceleration(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>AccelerationChange</c> event until the acceleration value has
		/// changed by the amount specified by the <c>AccelerationChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>AccelerationChangeTrigger</c> to 0 will result in the channel firing
		/// events every <c>DataInterval</c>. This is useful for applications that implement their own
		/// data filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double AccelerationChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetAccelerometer_getAccelerationChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetAccelerometer_setAccelerationChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The minimum change trigger value </summary>
		/// <remarks>The minimum value that <c>AccelerationChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinAccelerationChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetAccelerometer_getMinAccelerationChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The maximum change trigger value </summary>
		/// <remarks>The maximum value that <c>AccelerationChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxAccelerationChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetAccelerometer_getMaxAccelerationChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The number of axes </summary>
		/// <remarks>The number of axes the channel can measure acceleration on.
		/// <list>
		/// <item>See your device's User Guide for more information about the number of axes and their
		/// orientation.</item>
		/// </list>
		/// 
		/// </remarks>
		public int AxisCount {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetAccelerometer_getAxisCount(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>AccelerationChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>AccelerationChange</c> events can also affected by the
		/// <c>AccelerationChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetAccelerometer_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetAccelerometer_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetAccelerometer_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetAccelerometer_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The timestamp value </summary>
		/// <remarks>The most recent timestamp value that the channel has reported. This is an extremely accurate time
		/// measurement streamed from the device.
		/// <list>
		/// <item>If your application requires a time measurement, you should use this value over a local
		/// software timestamp.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Timestamp {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetAccelerometer_getTimestamp(chandle, out val);
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
			nativeAccelerationChangeEventCallback = new Phidget22Imports.AccelerometerAccelerationChangeEvent(nativeAccelerationChangeEvent);
			result = Phidget22Imports.PhidgetAccelerometer_setOnAccelerationChangeHandler(chandle, nativeAccelerationChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetAccelerometer_setOnAccelerationChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent acceleration values the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>AccelerationChangeTrigger</c> has been set to a non-zero value, the
		/// <c>AccelerationChange</c> event will not occur until the acceleration has changed by at least
		/// the <c>AccelerationChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event AccelerometerAccelerationChangeEventHandler AccelerationChange;
		internal void OnAccelerationChange(AccelerometerAccelerationChangeEventArgs e) {
			if (AccelerationChange != null) {
				foreach (AccelerometerAccelerationChangeEventHandler AccelerationChangeHandler in AccelerationChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = AccelerationChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(AccelerationChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						AccelerationChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.AccelerometerAccelerationChangeEvent nativeAccelerationChangeEventCallback;
		internal void nativeAccelerationChangeEvent(IntPtr phid, IntPtr ctx, double[] acceleration, double timestamp) {
			OnAccelerationChange(new AccelerometerAccelerationChangeEventArgs(acceleration, timestamp));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getAcceleration(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMinAcceleration(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMaxAcceleration(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getAccelerationChangeTrigger(IntPtr phid, out double AccelerationChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_setAccelerationChangeTrigger(IntPtr phid, double AccelerationChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMinAccelerationChangeTrigger(IntPtr phid, out double MinAccelerationChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMaxAccelerationChangeTrigger(IntPtr phid, out double MaxAccelerationChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getAxisCount(IntPtr phid, out int AxisCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_getTimestamp(IntPtr phid, out double Timestamp);
		public delegate void AccelerometerAccelerationChangeEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeConst = 3)] double[] acceleration, double timestamp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetAccelerometer_setOnAccelerationChangeHandler(IntPtr phid, AccelerometerAccelerationChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Accelerometer AccelerationChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A AccelerometerAccelerationChangeEventArg object contains data and information related to the Event.</param>
	public delegate void AccelerometerAccelerationChangeEventHandler(object sender, AccelerometerAccelerationChangeEventArgs e);
	/// <summary> Accelerometer AccelerationChange Event data </summary>
	public class AccelerometerAccelerationChangeEventArgs : EventArgs {
		/// <summary>The acceleration values
		/// </summary>
		public readonly double[] Acceleration;
		/// <summary>The timestamp value
		/// </summary>
		public readonly double Timestamp;
		internal AccelerometerAccelerationChangeEventArgs(double[] acceleration, double timestamp) {
			this.Acceleration = acceleration;
			this.Timestamp = timestamp;
		}
	}

}
