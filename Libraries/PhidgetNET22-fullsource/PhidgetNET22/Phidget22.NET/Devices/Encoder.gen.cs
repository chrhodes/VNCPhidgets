using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Encoder class definition </summary>
	public partial class Encoder : Phidget {
		#region Constructor/Destructor
		/// <summary> Encoder Constructor </summary>
		public Encoder() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetEncoder_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Encoder Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Encoder() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetEncoder_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The enabled value </summary>
		/// <remarks>The enabled state of the encoder.
		/// </remarks>
		public bool Enabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetEncoder_getEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetEncoder_setEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the channel will fire another
		/// <c>PositionChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>PositionChange</c> events can also affected by the
		/// <c>PositionChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetEncoder_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetEncoder_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetEncoder_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetEncoder_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The index position value </summary>
		/// <remarks>The most recent position of the index channel calculated by the Phidgets library.
		/// <list>
		/// <item>The index channel will usually pulse once per rotation.</item>
		/// <item>Setting the encoder position will move the index position the same amount so their relative
		/// position stays the same.</item>
		/// <item>Index position is tracked locally as the last position at which the index was triggered.
		/// Setting position will only affect the local copy of the index position value. This means that index
		/// positions seen by multiple network applications may not agree.</item>
		/// </list>
		/// 
		/// </remarks>
		public long IndexPosition {
			get {
				ErrorCode result;
				long val;
				result = Phidget22Imports.PhidgetEncoder_getIndexPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The IO mode value. </summary>
		/// <remarks>The encoder interface mode. Match the mode to the type of encoder you have attached.
		/// <list>
		/// <item>It is recommended to only change this when the encoder disabled in order to avoid unexpected
		/// results.</item>
		/// </list>
		/// 
		/// </remarks>
		public EncoderIOMode IOMode {
			get {
				ErrorCode result;
				EncoderIOMode val;
				result = Phidget22Imports.PhidgetEncoder_getIOMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetEncoder_setIOMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The most recent position value calculated by the Phidgets library.
		/// <list>
		/// <item>Position counts quadrature edges within a quadrature cycle. This means there are four counts
		/// per full quadrature cycle.</item>
		/// <item>Position is tracked locally as the total position change from the time the channel is opened.
		/// Setting position will only affect the local copy of the position value. This means that positions
		/// seen by multiple network applications may not agree.</item>
		/// </list>
		/// 
		/// </remarks>
		public long Position {
			get {
				ErrorCode result;
				long val;
				result = Phidget22Imports.PhidgetEncoder_getPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetEncoder_setPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The channel will not issue a <c>PositionChange</c> event until the position value has changed
		/// by the amount specified by the <c>PositionChangeTrigger</c>.
		/// <list>
		/// <item>Setting the <c>PositionChangeTrigger</c> to 0 will result in the channel firing events
		/// every <c>DataInterval</c>. This is useful for applications that implement their own data
		/// filtering</item>
		/// </list>
		/// 
		/// </remarks>
		public int PositionChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetEncoder_getPositionChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetEncoder_setPositionChangeTrigger(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The minimum value that <c>PositionChangeTrigger</c> can be set to.
		/// </remarks>
		public int MinPositionChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetEncoder_getMinPositionChangeTrigger(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The change trigger value </summary>
		/// <remarks>The maximum value that <c>PositionChangeTrigger</c> can be set to.
		/// </remarks>
		public int MaxPositionChangeTrigger {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetEncoder_getMaxPositionChangeTrigger(chandle, out val);
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
			nativePositionChangeEventCallback = new Phidget22Imports.EncoderPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetEncoder_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetEncoder_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent values the channel has measured will be reported in this event, which occurs when
		/// the <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>If a <c>PositionChangeTrigger</c> has been set to a non-zero value, the
		/// <c>PositionChange</c> event will not occur until the position has changed by at least the
		/// <c>PositionChangeTrigger</c> value.</item>
		/// </list>
		/// 
		/// </remarks>
		public event EncoderPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(EncoderPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (EncoderPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PositionChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PositionChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PositionChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.EncoderPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, int positionChange, double timeChange, bool indexTriggered) {
			OnPositionChange(new EncoderPositionChangeEventArgs(positionChange, timeChange, indexTriggered));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getEnabled(IntPtr phid, out bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setEnabled(IntPtr phid, bool Enabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getIndexPosition(IntPtr phid, out long IndexPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getIOMode(IntPtr phid, out EncoderIOMode IOMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setIOMode(IntPtr phid, EncoderIOMode IOMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getPosition(IntPtr phid, out long Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setPosition(IntPtr phid, long Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getPositionChangeTrigger(IntPtr phid, out int PositionChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setPositionChangeTrigger(IntPtr phid, int PositionChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getMinPositionChangeTrigger(IntPtr phid, out int MinPositionChangeTrigger);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_getMaxPositionChangeTrigger(IntPtr phid, out int MaxPositionChangeTrigger);
		public delegate void EncoderPositionChangeEvent(IntPtr phid, IntPtr ctx, int positionChange, double timeChange, bool indexTriggered);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetEncoder_setOnPositionChangeHandler(IntPtr phid, EncoderPositionChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Encoder PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A EncoderPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void EncoderPositionChangeEventHandler(object sender, EncoderPositionChangeEventArgs e);
	/// <summary> Encoder PositionChange Event data </summary>
	public class EncoderPositionChangeEventArgs : EventArgs {
		/// <summary>The amount the position changed since the last change event
		/// </summary>
		public readonly int PositionChange;
		/// <summary>The time elapsed since the last change event in milliseconds
		/// </summary>
		public readonly double TimeChange;
		/// <summary>True if the index was passed since the last change event
		/// </summary>
		public readonly bool IndexTriggered;
		internal EncoderPositionChangeEventArgs(int positionChange, double timeChange, bool indexTriggered) {
			this.PositionChange = positionChange;
			this.TimeChange = timeChange;
			this.IndexTriggered = indexTriggered;
		}
	}

}
