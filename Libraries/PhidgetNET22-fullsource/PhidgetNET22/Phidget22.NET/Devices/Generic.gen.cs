using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Generic class definition </summary>
	public partial class Generic : Phidget {
		#region Constructor/Destructor
		/// <summary> Generic Constructor </summary>
		public Generic() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetGeneric_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Generic Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Generic() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetGeneric_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> Length of the incoming packet </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public int INPacketLength {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGeneric_getINPacketLength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Length of the outgoing packet </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public int OUTPacketLength {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetGeneric_getOUTPacketLength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public void SendPacket(byte[] packet) {
			ErrorCode result;
			IntPtr packetLen = new IntPtr(packet.Length);
			result = Phidget22Imports.PhidgetGeneric_sendPacket(chandle, packet, packetLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public IAsyncResult BeginSendPacket(byte[] packet, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SendPacket");
			try {
				ErrorCode result;
				IntPtr packetLen = new IntPtr(packet.Length);
				result = Phidget22Imports.PhidgetGeneric_sendPacket_async(chandle, packet, packetLen, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSendPacket</param>
		public void EndSendPacket(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SendPacket");
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativePacketEventCallback = new Phidget22Imports.GenericPacketEvent(nativePacketEvent);
			result = Phidget22Imports.PhidgetGeneric_setOnPacketHandler(chandle, nativePacketEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetGeneric_setOnPacketHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>TODO: Text Here
		/// </remarks>
		public event GenericPacketEventHandler Packet;
		internal void OnPacket(GenericPacketEventArgs e) {
			if (Packet != null) {
				foreach (GenericPacketEventHandler PacketHandler in Packet.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PacketHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PacketHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PacketHandler(this, e);
				}
			}
		}
		Phidget22Imports.GenericPacketEvent nativePacketEventCallback;
		internal void nativePacketEvent(IntPtr phid, IntPtr ctx, byte[] packet, IntPtr packetLen) {
			OnPacket(new GenericPacketEventArgs(packet));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_sendPacket(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] packet, IntPtr packetLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_sendPacket_async(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] packet, IntPtr packetLen, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_getINPacketLength(IntPtr phid, out int INPacketLength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_getOUTPacketLength(IntPtr phid, out int OUTPacketLength);
		public delegate void GenericPacketEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] packet, IntPtr packetLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetGeneric_setOnPacketHandler(IntPtr phid, GenericPacketEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Generic Packet Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A GenericPacketEventArg object contains data and information related to the Event.</param>
	public delegate void GenericPacketEventHandler(object sender, GenericPacketEventArgs e);
	/// <summary> Generic Packet Event data </summary>
	public class GenericPacketEventArgs : EventArgs {
		/// <summary>The Phidget data packet
		/// </summary>
		public readonly byte[] Packet;
		internal GenericPacketEventArgs(byte[] packet) {
			this.Packet = packet;
		}
	}

}
