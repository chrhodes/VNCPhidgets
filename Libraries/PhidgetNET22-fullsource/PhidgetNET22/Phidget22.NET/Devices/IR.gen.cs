using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> IR class definition </summary>
	public partial class IR : Phidget {
		#region Constructor/Destructor
		/// <summary> IR Constructor </summary>
		public IR() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetIR_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> IR Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~IR() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetIR_delete(ref chandle);
			}
		}
		#endregion

		#region Constants

		/// <summary> The value for a long space in raw data </summary>
		public const int RawDataLongSpace = -1;

		/// <summary> Maximum bit count for sent / received data </summary>
		public const int MaxCodeBitCount = 128;

		/// <summary> Maximum bit count for sent / received data </summary>
		public const int MaxCodeStringLength = 33;
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>The last code the channel has received.
		/// <list>
		/// <item>The code is represented by a hexadecimal string (array of bytes).</item>
		/// </list>
		/// 
		/// </remarks>
		public IRCode GetLastCode() {
			ErrorCode result;
			IntPtr codePtr = Marshal.AllocHGlobal(33);
			string code;
			IntPtr codeLen = new IntPtr(33);
			int bitCount;
			result = Phidget22Imports.PhidgetIR_getLastCode(chandle, codePtr, codeLen, out bitCount);
			if (result != 0) {
				Marshal.FreeHGlobal(codePtr);
				throw PhidgetException.CreateByCode(result);
			}
			code = UTF8Marshaler.Instance.MarshalNativeToManaged(codePtr);
			IRCode ret = new IRCode();
			ret.Code = code;
			ret.BitCount = bitCount;
			Marshal.FreeHGlobal(codePtr);
			return ret;
		}

		/// <summary> </summary>
		/// <remarks>The last code the channel has learned.
		/// <list>
		/// <item>The code is represented by a hexadecimal string (array of bytes).</item>
		/// <item>The <c>codeInfo</c> structure holds data that describes the learned code.</item>
		/// </list>
		/// 
		/// </remarks>
		public IRLearnedCode GetLastLearnedCode() {
			ErrorCode result;
			IntPtr codePtr = Marshal.AllocHGlobal(33);
			string code;
			IntPtr codeLen = new IntPtr(33);
			IntPtr codeInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IRCodeInfo)));
			result = Phidget22Imports.PhidgetIR_getLastLearnedCode(chandle, codePtr, codeLen, codeInfoPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(codePtr);
				Marshal.FreeHGlobal(codeInfoPtr);
				throw PhidgetException.CreateByCode(result);
			}
			code = UTF8Marshaler.Instance.MarshalNativeToManaged(codePtr);
			IRCodeInfo codeInfo = (IRCodeInfo)Marshal.PtrToStructure(codeInfoPtr, typeof(IRCodeInfo));
			IRLearnedCode ret = new IRLearnedCode();
			ret.Code = code;
			ret.CodeInfo = codeInfo;
			Marshal.FreeHGlobal(codePtr);
			Marshal.FreeHGlobal(codeInfoPtr);
			return ret;
		}

		/// <summary> </summary>
		/// <remarks>Transmits a code
		/// <list>
		/// <item><c>code</c> data is transmitted MSBit first.</item>
		/// <item>MSByte is in array index 0 of <c>code</c></item>
		/// <item>LSBit is right justified, therefore, MSBit may be in bit position 0-7 (of array index 0)
		/// depending on the bit count.</item>
		/// </list>
		/// 
		/// </remarks>
		public void Transmit(string code, IRCodeInfo codeInfo) {
			ErrorCode result;
			IntPtr codePtr = UTF8Marshaler.Instance.MarshalManagedToNative(code);
			result = Phidget22Imports.PhidgetIR_transmit(chandle, codePtr, codeInfo);
			if (result != 0) {
				Marshal.FreeHGlobal(codePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(codePtr);
		}

		/// <summary> </summary>
		/// <remarks>Transmits <b>raw</b> data as a series of pulses and spaces.
		/// <list>
		/// <item><c>data</c> must start and end with a pulse.
		/// <list>
		/// <item>Each element is a positive time in μs</item>
		/// </list>
		/// </item>
		/// <item><c>dataLength</c> has a maximum length of 200, however, streams should be kept must
		/// shorter than this (less than 100ms between gaps).</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item><c>dataLength</c> must be an odd number</item>
		/// </list>
		/// </item>
		/// <item>Leave <c>carrierFrequency</c> as 0 for default.</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item><c>carrierFrequency</c> has a range of 10kHz - 1MHz</item>
		/// </list>
		/// </item>
		/// <item>Leave <c>dutyCycle</c> as 0 for default</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item><c>dutyCycle</c> can have a value between 0.1 and 0.5</item>
		/// </list>
		/// </item>
		/// <item>Specifying a <c>gap</c> will guarantee a gap time (no transmitting) after data is
		/// sent.</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>gap time is in μs</item>
		/// <item>gap time can be set to 0</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public void TransmitRaw(int[] data, int carrierFrequency, double dutyCycle, int gap) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.PhidgetIR_transmitRaw(chandle, data, dataLen, carrierFrequency, dutyCycle, gap);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Transmits a repeat of the last transmited code.
		/// <list>
		/// <item>Depending on the CodeInfo structure, this may be a retransmission of the code itself, or there
		/// may be a special repeat code.</item>
		/// </list>
		/// 
		/// </remarks>
		public void TransmitRepeat() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetIR_transmitRepeat(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeCodeEventCallback = new Phidget22Imports.IRCodeEvent(nativeCodeEvent);
			result = Phidget22Imports.PhidgetIR_setOnCodeHandler(chandle, nativeCodeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeLearnEventCallback = new Phidget22Imports.IRLearnEvent(nativeLearnEvent);
			result = Phidget22Imports.PhidgetIR_setOnLearnHandler(chandle, nativeLearnEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeRawDataEventCallback = new Phidget22Imports.IRRawDataEvent(nativeRawDataEvent);
			result = Phidget22Imports.PhidgetIR_setOnRawDataHandler(chandle, nativeRawDataEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetIR_setOnCodeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetIR_setOnLearnHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetIR_setOnRawDataHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>This event is fired every time a code is received and correctly decoded.
		/// <list>
		/// <item>The code is represented by a hexadecimal string (array of bytes) with a length of 1/4 of
		/// <c>bitCount</c>.</item>
		/// <item>The MSBit is considered to be the first bit received and will be in array index 0 of
		/// <c>code</c></item>
		/// <item>Repeat will be true if a repeat is detected (either timing wise or via a repeat code)</item>
		/// <item style="list-style: none; display: inline">
		/// <list>
		/// <item>False repeasts can happen if two separate button presses happen close together</item>
		/// </list>
		/// </item>
		/// </list>
		/// 
		/// </remarks>
		public event IRCodeEventHandler Code;
		internal void OnCode(IRCodeEventArgs e) {
			if (Code != null) {
				foreach (IRCodeEventHandler CodeHandler in Code.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = CodeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(CodeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						CodeHandler(this, e);
				}
			}
		}
		Phidget22Imports.IRCodeEvent nativeCodeEventCallback;
		internal void nativeCodeEvent(IntPtr phid, IntPtr ctx, IntPtr code, int bitCount, bool isRepeat) {
			OnCode(new IRCodeEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(code), bitCount, isRepeat));
		}
		/// <summary>  </summary>
		/// <remarks>This event fires when a button has been held down long enough for the channel to have learned the
		/// CodeInfo values
		/// <list>
		/// <item>A code is usually learned after 1 second, or after 4 repeats.</item>
		/// </list>
		/// 
		/// </remarks>
		public event IRLearnEventHandler Learn;
		internal void OnLearn(IRLearnEventArgs e) {
			if (Learn != null) {
				foreach (IRLearnEventHandler LearnHandler in Learn.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = LearnHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(LearnHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						LearnHandler(this, e);
				}
			}
		}
		Phidget22Imports.IRLearnEvent nativeLearnEventCallback;
		internal void nativeLearnEvent(IntPtr phid, IntPtr ctx, IntPtr code, IntPtr codeInfo) {
			OnLearn(new IRLearnEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(code), (IRCodeInfo)Marshal.PtrToStructure(codeInfo, typeof(IRCodeInfo))));
		}
		/// <summary>  </summary>
		/// <remarks>This event will fire every time the channel gets more data
		/// <list>
		/// <item>This will happen at most once every 8ms.</item>
		/// </list>
		/// 
		/// </remarks>
		public event IRRawDataEventHandler RawData;
		internal void OnRawData(IRRawDataEventArgs e) {
			if (RawData != null) {
				foreach (IRRawDataEventHandler RawDataHandler in RawData.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = RawDataHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(RawDataHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						RawDataHandler(this, e);
				}
			}
		}
		Phidget22Imports.IRRawDataEvent nativeRawDataEventCallback;
		internal void nativeRawDataEvent(IntPtr phid, IntPtr ctx, int[] data, IntPtr dataLen) {
			OnRawData(new IRRawDataEventArgs(data));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_getLastCode(IntPtr phid, IntPtr code, IntPtr codeLen, out int bitCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_getLastLearnedCode(IntPtr phid, IntPtr code, IntPtr codeLen, IntPtr codeInfo);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_transmit(IntPtr phid, IntPtr code, IRCodeInfo codeInfo);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_transmitRaw(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] data, IntPtr dataLen, int carrierFrequency, double dutyCycle, int gap);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_transmitRepeat(IntPtr phid);
		public delegate void IRCodeEvent(IntPtr phid, IntPtr ctx, IntPtr code, int bitCount, bool isRepeat);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_setOnCodeHandler(IntPtr phid, IRCodeEvent fptr, IntPtr ctx);
		public delegate void IRLearnEvent(IntPtr phid, IntPtr ctx, IntPtr code, IntPtr codeInfo);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_setOnLearnHandler(IntPtr phid, IRLearnEvent fptr, IntPtr ctx);
		public delegate void IRRawDataEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetIR_setOnRawDataHandler(IntPtr phid, IRRawDataEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A IR Code Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A IRCodeEventArg object contains data and information related to the Event.</param>
	public delegate void IRCodeEventHandler(object sender, IRCodeEventArgs e);
	/// <summary> IR Code Event data </summary>
	public class IRCodeEventArgs : EventArgs {
		/// <summary>The code string
		/// </summary>
		public readonly string Code;
		/// <summary>The length of the received code in bits
		/// </summary>
		public readonly int BitCount;
		/// <summary>'true' if a repeat is detected
		/// </summary>
		public readonly bool IsRepeat;
		internal IRCodeEventArgs(string code, int bitCount, bool isRepeat) {
			this.Code = code;
			this.BitCount = bitCount;
			this.IsRepeat = isRepeat;
		}
	}

	/// <summary> A IR Learn Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A IRLearnEventArg object contains data and information related to the Event.</param>
	public delegate void IRLearnEventHandler(object sender, IRLearnEventArgs e);
	/// <summary> IR Learn Event data </summary>
	public class IRLearnEventArgs : EventArgs {
		/// <summary>The code string
		/// </summary>
		public readonly string Code;
		/// <summary>Contains the data for characterizing the code.
		/// </summary>
		public readonly IRCodeInfo CodeInfo;
		internal IRLearnEventArgs(string code, IRCodeInfo codeInfo) {
			this.Code = code;
			this.CodeInfo = codeInfo;
		}
	}

	/// <summary> A IR RawData Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A IRRawDataEventArg object contains data and information related to the Event.</param>
	public delegate void IRRawDataEventHandler(object sender, IRRawDataEventArgs e);
	/// <summary> IR RawData Event data </summary>
	public class IRRawDataEventArgs : EventArgs {
		/// <summary>The data being received
		/// </summary>
		public readonly int[] Data;
		internal IRRawDataEventArgs(int[] data) {
			this.Data = data;
		}
	}

}
