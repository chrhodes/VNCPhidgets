using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> SoundSensor class definition </summary>
	public partial class SoundSensor : Phidget {
		#region Constructor/Destructor
		/// <summary> SoundSensor Constructor </summary>
		public SoundSensor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetSoundSensor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> SoundSensor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~SoundSensor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetSoundSensor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>SPLChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>SPLChange</c> events can also affected by the
		/// <c>SPLChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetSoundSensor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetSoundSensor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetSoundSensor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetSoundSensor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The dB value </summary>
		/// <remarks>The most recent dB SPL value that has been calculated.
		/// <list>
		/// <item>This value is bounded by <c>MaxdB</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double dB {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getdB(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The dB value </summary>
		/// <remarks>The maximum value the <c>SPLChange</c> event will report.
		/// </remarks>
		public double MaxdB {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getMaxdB(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The dBA value </summary>
		/// <remarks>The most recent dBA SPL value that has been calculated.
		/// <list>
		/// <item>The dBA SPL value is calculated by applying a A-weighted filter to the <c>Octaves</c>
		/// data.</item>
		/// </list>
		/// 
		/// </remarks>
		public double dBA {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getdBA(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The dBC value </summary>
		/// <remarks>The most recent dBC SPL value that has been calculated.
		/// <list>
		/// <item>The dBC SPL value is calculated by applying a C-weighted filter to the <c>Octaves</c>
		/// data.</item>
		/// </list>
		/// 
		/// </remarks>
		public double dBC {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getdBC(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The noise floor value. </summary>
		/// <remarks>The minimum SPL value that the channel can accurately measure.
		/// <list>
		/// <item>Input SPLs below this level will not produce an output from the microphone.</item>
		/// </list>
		/// 
		/// </remarks>
		public double NoiseFloor {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getNoiseFloor(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The octave values </summary>
		/// <remarks>The unweighted value of each frequency band.
		/// <list>
		/// <item>The following frequency bands are represented:</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>octaves[0] = 31.5 Hz</item>
		/// <item>octaves[1] = 63 Hz</item>
		/// <item>octaves[2] = 125 Hz</item>
		/// <item>octaves[3] = 250 Hz</item>
		/// <item>octaves[4] = 500 Hz</item>
		/// <item>octaves[5] = 1 kHz</item>
		/// <item>octaves[6] = 2 kHz</item>
		/// <item>octaves[7] = 4 kHz</item>
		/// <item>octaves[8] = 8 kHz</item>
		/// <item>octaves[9] = 16 kHz</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public double[] Octaves {
			get {
				ErrorCode result;
				double[] val = new double[10];
				result = Phidget22Imports.PhidgetSoundSensor_getOctaves(chandle, val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>SPLChange</c> event until the <c>dB</c> value has
		/// changed by the amount specified by the <c>SPLChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>SPLChangeTrigger</c> to 0 will result in the channel firing events every
		/// <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public double SPLChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getSPLChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetSoundSensor_setSPLChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>SPLChangeTrigger</c> can be set to.
		/// </remarks>
		public double MinSPLChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getMinSPLChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>SPLChangeTrigger</c> can be set to.
		/// </remarks>
		public double MaxSPLChangeTrigger {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetSoundSensor_getMaxSPLChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The range value. </summary>
		/// <remarks>When selecting a range, first decide how sensitive you want the microphone to be. Select a smaller
		/// range when you want more sensitivity from the microphone.
		/// <list>
		/// <item>If a <c>Saturation</c> event occurrs, increase the range.</item>
		/// </list>
		/// 
		/// </remarks>
		public SPLRange SPLRange {
			get {
				ErrorCode result;
				SPLRange val;
				result = Phidget22Imports.PhidgetSoundSensor_getSPLRange(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetSoundSensor_setSPLRange(chandle, value);
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
			nativeSPLChangeEventCallback = new Phidget22Imports.SoundSensorSPLChangeEvent(nativeSPLChangeEvent);
			result = Phidget22Imports.PhidgetSoundSensor_setOnSPLChangeHandler(chandle, nativeSPLChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetSoundSensor_setOnSPLChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent SPL values the channel has measured will be reported in this event, which occurs
		/// when the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>SPLChangeTrigger</c> has been set to a non-zero value, the <c>SPLChange</c>
		/// event will not occur until the <c>dB</c> SPL value has changed by at least the
		/// <c>SPLChangeTrigger</c> value.</item>
		/// <item>The dB SPL value is calculated from the <c>Octaves</c> data.</item>
		/// <item>The dBA SPL value is calculated by applying a A-weighted filter to the <c>Octaves</c>
		/// data.</item>
		/// <item>The dBC SPL value is calculated by applying a C-weighted filter to the <c>Octaves</c>
		/// data.</item>
		/// <item>The following frequency bands are represented:</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>octaves[0] = 31.5 Hz</item>
		/// <item>octaves[1] = 63 Hz</item>
		/// <item>octaves[2] = 125 Hz</item>
		/// <item>octaves[3] = 250 Hz</item>
		/// <item>octaves[4] = 500 Hz</item>
		/// <item>octaves[5] = 1 kHz</item>
		/// <item>octaves[6] = 2 kHz</item>
		/// <item>octaves[7] = 4 kHz</item>
		/// <item>octaves[8] = 8 kHz</item>
		/// <item>octaves[9] = 16 kHz</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public event SoundSensorSPLChangeEventHandler SPLChange;
		internal void OnSPLChange(SoundSensorSPLChangeEventArgs e) {
			if (SPLChange != null) {
				foreach (SoundSensorSPLChangeEventHandler SPLChangeHandler in SPLChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = SPLChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(SPLChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						SPLChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.SoundSensorSPLChangeEvent nativeSPLChangeEventCallback;
		internal void nativeSPLChangeEvent(IntPtr phid, IntPtr ctx, double dB, double dBA, double dBC, double[] octaves) {
			OnSPLChange(new SoundSensorSPLChangeEventArgs(dB, dBA, dBC, octaves));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getdB(IntPtr phid, out double dB);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getMaxdB(IntPtr phid, out double MaxdB);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getdBA(IntPtr phid, out double dBA);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getdBC(IntPtr phid, out double dBC);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getNoiseFloor(IntPtr phid, out double NoiseFloor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getOctaves(IntPtr phid, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] double[] Octaves);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getSPLChangeTrigger(IntPtr phid, out double SPLChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_setSPLChangeTrigger(IntPtr phid, double SPLChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getMinSPLChangeTrigger(IntPtr phid, out double MinSPLChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getMaxSPLChangeTrigger(IntPtr phid, out double MaxSPLChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_getSPLRange(IntPtr phid, out SPLRange SPLRange);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_setSPLRange(IntPtr phid, SPLRange SPLRange);
		public delegate void SoundSensorSPLChangeEvent(IntPtr phid, IntPtr ctx, double dB, double dBA, double dBC, [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] double[] Octaves);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetSoundSensor_setOnSPLChangeHandler(IntPtr phid, SoundSensorSPLChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A SoundSensor SPLChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A SoundSensorSPLChangeEventArg object contains data and information related to the Event.</param>
	public delegate void SoundSensorSPLChangeEventHandler(object sender, SoundSensorSPLChangeEventArgs e);
	/// <summary> SoundSensor SPLChange Event data </summary>
	public class SoundSensorSPLChangeEventArgs : EventArgs {
		/// <summary>The dB SPL value.
		/// </summary>
		public readonly double DB;
		/// <summary>The dBA SPL value.
		/// </summary>
		public readonly double DBA;
		/// <summary>The dBC SPL value.
		/// </summary>
		public readonly double DBC;
		/// <summary>The dB SPL value for each band.
		/// </summary>
		public readonly double[] Octaves;
		internal SoundSensorSPLChangeEventArgs(double dB, double dBA, double dBC, double[] octaves) {
			this.DB = dB;
			this.DBA = dBA;
			this.DBC = dBC;
			this.Octaves = octaves;
		}
	}

}
