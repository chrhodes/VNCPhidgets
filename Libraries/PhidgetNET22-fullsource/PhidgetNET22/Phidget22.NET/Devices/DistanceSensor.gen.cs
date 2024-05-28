using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> DistanceSensor class definition </summary>
	public partial class DistanceSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> DistanceSensor Constructor </summary>
		public DistanceSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDistanceSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> DistanceSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~DistanceSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDistanceSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between events can also affected by the change trigger.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDistanceSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetDistanceSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetDistanceSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The distance value </summary>
		/// <remarks>The most recent distance value that the channel has reported.
		/// <list>
		/// <item>This value will always be between <c>MinDistance</c> and <c>MaxDistance</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int Distance {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getDistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The distance value </summary>
		/// <remarks>The minimum distance that a event will report.
		/// </remarks>
		public int MinDistance {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getMinDistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The distance value </summary>
		/// <remarks>The maximum distance that a event will report.
		/// </remarks>
		public int MaxDistance {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getMaxDistance(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue an event until the distance value has changed by the amount specified by
		/// the <c>DistanceChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>DistanceChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering,</item>
		/// </list>
		/// 
		/// </remarks>
		public int DistanceChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getDistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDistanceSensor_setDistanceChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>DistanceChangeTrigger</c> can be set to.
		/// </remarks>
		public int MinDistanceChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getMinDistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>DistanceChangeTrigger</c> can be set to.
		/// </remarks>
		public int MaxDistanceChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDistanceSensor_getMaxDistanceChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The quiet mode value </summary>
		/// <remarks>When set to true, the device will operate more quietly.
		/// <list>
		/// <item>The measurable range is reduced when operating in quiet mode.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool SonarQuietMode {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetDistanceSensor_getSonarQuietMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDistanceSensor_setSonarQuietMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>The most recent reflection values that the channel has reported.
		/// <list>
		/// <item>The distance values will always be between <c>MinDistance</c> and
		/// <c>MaxDistance</c>.</item>
		/// <item>The closest reflection will be placed at index 0 of the distances array, and the furthest
		/// reflection at index 7</item>
		/// <item>The amplitude values are relative amplitudes of the reflections that are normalized to an
		/// arbitrary scale.</item>
		/// </list>
		/// 
		/// </remarks>
		public DistanceSensorSonarReflections GetSonarReflections() {
			ErrorCode result;
			int[] distances = new int[8];
			int[] amplitudes = new int[8];
			int count;
			result = Phidget22Imports.PhidgetDistanceSensor_getSonarReflections(chandle, distances, amplitudes, out count);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			DistanceSensorSonarReflections ret = new DistanceSensorSonarReflections();
			ret.Distances = distances;
			ret.Amplitudes = amplitudes;
			ret.Count = count;
			return ret;
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeDistanceChangeEventCallback = new Phidget22Imports.DistanceSensorDistanceChangeEvent(nativeDistanceChangeEvent);
			result = Phidget22Imports.PhidgetDistanceSensor_setOnDistanceChangeHandler(chandle, nativeDistanceChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeSonarReflectionsUpdateEventCallback = new Phidget22Imports.DistanceSensorSonarReflectionsUpdateEvent(nativeSonarReflectionsUpdateEvent);
			result = Phidget22Imports.PhidgetDistanceSensor_setOnSonarReflectionsUpdateHandler(chandle, nativeSonarReflectionsUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetDistanceSensor_setOnDistanceChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetDistanceSensor_setOnSonarReflectionsUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent distance value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>DistanceChangeTrigger</c> has been set to a non-zero value, the
		/// <c>DistanceChange</c> event will not occur until the distance has changed by at least the
		/// <c>DistanceChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event DistanceSensorDistanceChangeEventHandler DistanceChange;
		internal void OnDistanceChange(DistanceSensorDistanceChangeEventArgs e) {
			if (DistanceChange != null) {
				foreach (DistanceSensorDistanceChangeEventHandler DistanceChangeHandler in DistanceChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = DistanceChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(DistanceChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						DistanceChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.DistanceSensorDistanceChangeEvent nativeDistanceChangeEventCallback;
		internal void nativeDistanceChangeEvent(IntPtr phid, IntPtr ctx, int distance) {
			OnDistanceChange(new DistanceSensorDistanceChangeEventArgs(distance));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent reflections the channel has detected will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>DistanceChangeTrigger</c> has been set to a non-zero value, the
		/// <c>SonarReflectionsUpdate</c> event will not occur until the distance has changed by at least
		/// the <c>DistanceChangeTrigger</c> value.</item>
		/// <item>The closest reflection will be placed at index 0 of the <small><i>distances</i></small> array,
		/// and the furthest reflection at index 7.</item>
		/// <item>If you are only interested in the closest reflection, you can simply use the
		/// <c>DistanceChange</c> event.</item>
		/// <item>The values reported as amplitudes are relative amplitudes of the reflections that are
		/// normalized to an arbitrary scale.</item>
		/// </list>
		/// 
		/// </remarks>
		public event DistanceSensorSonarReflectionsUpdateEventHandler SonarReflectionsUpdate;
		internal void OnSonarReflectionsUpdate(DistanceSensorSonarReflectionsUpdateEventArgs e) {
			if (SonarReflectionsUpdate != null) {
				foreach (DistanceSensorSonarReflectionsUpdateEventHandler SonarReflectionsUpdateHandler in SonarReflectionsUpdate.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = SonarReflectionsUpdateHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(SonarReflectionsUpdateHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						SonarReflectionsUpdateHandler(this, e);
				}
			}
		}
		Phidget22Imports.DistanceSensorSonarReflectionsUpdateEvent nativeSonarReflectionsUpdateEventCallback;
		internal void nativeSonarReflectionsUpdateEvent(IntPtr phid, IntPtr ctx, int[] distances, int[] amplitudes, int count) {
			OnSonarReflectionsUpdate(new DistanceSensorSonarReflectionsUpdateEventArgs(distances, amplitudes, count));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getSonarReflections(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] distances, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] amplitudes, out int count);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getDistance(IntPtr phid, out int Distance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMinDistance(IntPtr phid, out int MinDistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMaxDistance(IntPtr phid, out int MaxDistance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getDistanceChangeTrigger(IntPtr phid, out int DistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_setDistanceChangeTrigger(IntPtr phid, int DistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMinDistanceChangeTrigger(IntPtr phid, out int MinDistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getMaxDistanceChangeTrigger(IntPtr phid, out int MaxDistanceChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_getSonarQuietMode(IntPtr phid, out bool SonarQuietMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_setSonarQuietMode(IntPtr phid, bool SonarQuietMode);
		public delegate void DistanceSensorDistanceChangeEvent(IntPtr phid, IntPtr ctx, int distance);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_setOnDistanceChangeHandler(IntPtr phid, DistanceSensorDistanceChangeEvent fptr, IntPtr ctx);
		public delegate void DistanceSensorSonarReflectionsUpdateEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] distances, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] amplitudes, int count);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDistanceSensor_setOnSonarReflectionsUpdateHandler(IntPtr phid, DistanceSensorSonarReflectionsUpdateEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A DistanceSensor DistanceChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DistanceSensorDistanceChangeEventArg object contains data and information related to the Event.</param>
	public delegate void DistanceSensorDistanceChangeEventHandler(object sender, DistanceSensorDistanceChangeEventArgs e);
	/// <summary> DistanceSensor DistanceChange Event data </summary>
	public class DistanceSensorDistanceChangeEventArgs : EventArgs {
		/// <summary>The current distance
		/// </summary>
		public readonly int Distance;
		internal DistanceSensorDistanceChangeEventArgs(int distance) {
			this.Distance = distance;
		}
	}

	/// <summary> A DistanceSensor SonarReflectionsUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DistanceSensorSonarReflectionsUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void DistanceSensorSonarReflectionsUpdateEventHandler(object sender, DistanceSensorSonarReflectionsUpdateEventArgs e);
	/// <summary> DistanceSensor SonarReflectionsUpdate Event data </summary>
	public class DistanceSensorSonarReflectionsUpdateEventArgs : EventArgs {
		/// <summary>The reflection values
		/// </summary>
		public readonly int[] Distances;
		/// <summary>The amplitude values
		/// </summary>
		public readonly int[] Amplitudes;
		/// <summary>The number of reflections detected
		/// </summary>
		public readonly int Count;
		internal DistanceSensorSonarReflectionsUpdateEventArgs(int[] distances, int[] amplitudes, int count) {
			this.Distances = distances;
			this.Amplitudes = amplitudes;
			this.Count = count;
		}
	}

}
