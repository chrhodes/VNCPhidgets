using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> FrequencyCounter class definition </summary>
	public partial class FrequencyCounter : Phidget {
		#region Constructor/Destructor
		/// <summary> FrequencyCounter Constructor </summary>
		public FrequencyCounter() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetFrequencyCounter_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> FrequencyCounter Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~FrequencyCounter() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetFrequencyCounter_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The count value </summary>
		/// <remarks>The most recent count value the channel has reported.
		/// <list>
		/// <item>The count represents the total number of pulses since the the channel was opened, or last
		/// reset.</item>
		/// </list>
		/// 
		/// </remarks>
		public long Count {
			get {
				ErrorCode result;
				long val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getCount(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The enabled value </summary>
		/// <remarks>Enables or disables the channel.
		/// <list>
		/// <item>When a channel is disabled, it will not longer register counts, therefore the
		/// <c>TimeElapsed</c> and <c>Count</c> will not be updated until the channel is
		/// re-enabled.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Enabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>CountChange</c>/<c>FrequencyChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetFrequencyCounter_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetFrequencyCounter_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The filter value </summary>
		/// <remarks>Determines the signal type that the channel responds to.
		/// <list>
		/// <item>The filter type is chosen based on the type of input signal. See the
		/// <c>PhidgetFrequencyCounter_FilterType</c> entry under Enumerations for more information.</item>
		/// </list>
		/// 
		/// </remarks>
		public FrequencyFilterType FilterType {
			get {
				ErrorCode result;
				FrequencyFilterType val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getFilterType(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setFilterType(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The frequency value </summary>
		/// <remarks>The most recent frequency value that the channel has reported.
		/// <list>
		/// <item>This value will always be between 0 Hz and <c>MaxFrequency</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Frequency {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getFrequency(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The frequency value </summary>
		/// <remarks>The maximum value the <c>FrequencyChange</c> event will report.
		/// </remarks>
		public double MaxFrequency {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getMaxFrequency(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The frequency cutoff value </summary>
		/// <remarks>The frequency at which zero hertz is assumed.
		/// <list>
		/// <item>This means any frequency at or below the <c>FrequencyCutoff</c> value will be reported as
		/// 0 Hz.</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>This property is stored locally, so other users who have this Phidget open over a network
		/// connection won't see the effects of your selected cutoff.</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public double FrequencyCutoff {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getFrequencyCutoff(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setFrequencyCutoff(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The frequency value </summary>
		/// <remarks>The minimum value that <c>FrequencyCutoff</c> can be set to.
		/// </remarks>
		public double MinFrequencyCutoff {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getMinFrequencyCutoff(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The frequency value </summary>
		/// <remarks>The maximum value that <c>FrequencyCutoff</c> can be set to.
		/// </remarks>
		public double MaxFrequencyCutoff {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getMaxFrequencyCutoff(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

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
				result = Phidget22Imports.PhidgetFrequencyCounter_getInputMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setInputMode(chandle, value);
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
				result = Phidget22Imports.PhidgetFrequencyCounter_getPowerSupply(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetFrequencyCounter_setPowerSupply(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The time value </summary>
		/// <remarks>The amount of time the frequency counter has been enabled for.
		/// <list>
		/// <item>This property complements <c>Count</c>, the total number of pulses detected since the
		/// channel was opened, or last reset.</item>
		/// </list>
		/// 
		/// </remarks>
		public double TimeElapsed {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetFrequencyCounter_getTimeElapsed(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Resets the <c>Count</c> and <c>TimeElapsed</c>.
		/// <list>
		/// <item>For best results, reset should be called when the channel is disabled.</item>
		/// </list>
		/// 
		/// </remarks>
		public void Reset() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetFrequencyCounter_reset(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeCountChangeEventCallback = new Phidget22Imports.FrequencyCounterCountChangeEvent(nativeCountChangeEvent);
			result = Phidget22Imports.PhidgetFrequencyCounter_setOnCountChangeHandler(chandle, nativeCountChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeFrequencyChangeEventCallback = new Phidget22Imports.FrequencyCounterFrequencyChangeEvent(nativeFrequencyChangeEvent);
			result = Phidget22Imports.PhidgetFrequencyCounter_setOnFrequencyChangeHandler(chandle, nativeFrequencyChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetFrequencyCounter_setOnCountChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetFrequencyCounter_setOnFrequencyChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent values the channel has measured will be reported in this event, which occurs when
		/// the <c>DataInterval</c> has elapsed.
		/// </remarks>
		public event FrequencyCounterCountChangeEventHandler CountChange;
		internal void OnCountChange(FrequencyCounterCountChangeEventArgs e) {
			if (CountChange != null) {
				foreach (FrequencyCounterCountChangeEventHandler CountChangeHandler in CountChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = CountChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(CountChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						CountChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.FrequencyCounterCountChangeEvent nativeCountChangeEventCallback;
		internal void nativeCountChangeEvent(IntPtr phid, IntPtr ctx, long counts, double timeChange) {
			OnCountChange(new FrequencyCounterCountChangeEventArgs(counts, timeChange));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent frequency value the channel has measured will be reported in this event, which
		/// occurs when the <c>DataInterval</c> has elapsed.
		/// </remarks>
		public event FrequencyCounterFrequencyChangeEventHandler FrequencyChange;
		internal void OnFrequencyChange(FrequencyCounterFrequencyChangeEventArgs e) {
			if (FrequencyChange != null) {
				foreach (FrequencyCounterFrequencyChangeEventHandler FrequencyChangeHandler in FrequencyChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = FrequencyChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(FrequencyChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						FrequencyChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.FrequencyCounterFrequencyChangeEvent nativeFrequencyChangeEventCallback;
		internal void nativeFrequencyChangeEvent(IntPtr phid, IntPtr ctx, double frequency) {
			OnFrequencyChange(new FrequencyCounterFrequencyChangeEventArgs(frequency));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_reset(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getCount(IntPtr phid, out long Count);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getEnabled(IntPtr phid, out bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setEnabled(IntPtr phid, bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getFilterType(IntPtr phid, out FrequencyFilterType FilterType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setFilterType(IntPtr phid, FrequencyFilterType FilterType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getFrequency(IntPtr phid, out double Frequency);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getMaxFrequency(IntPtr phid, out double MaxFrequency);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getFrequencyCutoff(IntPtr phid, out double FrequencyCutoff);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setFrequencyCutoff(IntPtr phid, double FrequencyCutoff);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getMinFrequencyCutoff(IntPtr phid, out double MinFrequencyCutoff);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getMaxFrequencyCutoff(IntPtr phid, out double MaxFrequencyCutoff);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getInputMode(IntPtr phid, out InputMode InputMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setInputMode(IntPtr phid, InputMode InputMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getPowerSupply(IntPtr phid, out PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setPowerSupply(IntPtr phid, PowerSupply PowerSupply);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_getTimeElapsed(IntPtr phid, out double TimeElapsed);
		public delegate void FrequencyCounterCountChangeEvent(IntPtr phid, IntPtr ctx, long counts, double timeChange);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setOnCountChangeHandler(IntPtr phid, FrequencyCounterCountChangeEvent fptr, IntPtr ctx);
		public delegate void FrequencyCounterFrequencyChangeEvent(IntPtr phid, IntPtr ctx, double frequency);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetFrequencyCounter_setOnFrequencyChangeHandler(IntPtr phid, FrequencyCounterFrequencyChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A FrequencyCounter CountChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A FrequencyCounterCountChangeEventArg object contains data and information related to the Event.</param>
	public delegate void FrequencyCounterCountChangeEventHandler(object sender, FrequencyCounterCountChangeEventArgs e);
	/// <summary> FrequencyCounter CountChange Event data </summary>
	public class FrequencyCounterCountChangeEventArgs : EventArgs {
		/// <summary>The pulse count of the signal
		/// </summary>
		public readonly long Counts;
		/// <summary>The change in elapsed time since the last change
		/// </summary>
		public readonly double TimeChange;
		internal FrequencyCounterCountChangeEventArgs(long counts, double timeChange) {
			this.Counts = counts;
			this.TimeChange = timeChange;
		}
	}

	/// <summary> A FrequencyCounter FrequencyChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A FrequencyCounterFrequencyChangeEventArg object contains data and information related to the Event.</param>
	public delegate void FrequencyCounterFrequencyChangeEventHandler(object sender, FrequencyCounterFrequencyChangeEventArgs e);
	/// <summary> FrequencyCounter FrequencyChange Event data </summary>
	public class FrequencyCounterFrequencyChangeEventArgs : EventArgs {
		/// <summary>The calculated frequency of the signal
		/// </summary>
		public readonly double Frequency;
		internal FrequencyCounterFrequencyChangeEventArgs(double frequency) {
			this.Frequency = frequency;
		}
	}

}
