using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> DataAdapter class definition </summary>
	public partial class DataAdapter : Phidget {
		#region Constructor/Destructor
		/// <summary> DataAdapter Constructor </summary>
		public DataAdapter() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDataAdapter_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> DataAdapter Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~DataAdapter() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDataAdapter_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The maximum length of a packet. </summary>
		/// <remarks>The maximum length of a packet that can be sent in bytes.
		/// </remarks>
		public int MaxPacketLength {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDataAdapter_getMaxPacketLength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Sends a specified set of bytes as a packet to the VINTSPI to be read by the master. May overwrite
		/// data that has yet to be read by the master.
		/// </remarks>
		public void SendPacket(byte[] data) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.PhidgetDataAdapter_sendPacket(chandle, data, dataLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Sends a specified set of bytes as a packet to the VINTSPI to be read by the master. May overwrite
		/// data that has yet to be read by the master.
		/// </remarks>
		public IAsyncResult BeginSendPacket(byte[] data, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SendPacket");
			try {
				ErrorCode result;
				IntPtr dataLen = new IntPtr(data.Length);
				result = Phidget22Imports.PhidgetDataAdapter_sendPacket_async(chandle, data, dataLen, asyncResult.cCallbackDelegate, IntPtr.Zero);
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
			nativePacketEventCallback = new Phidget22Imports.DataAdapterPacketEvent(nativePacketEvent);
			result = Phidget22Imports.PhidgetDataAdapter_setOnPacketHandler(chandle, nativePacketEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetDataAdapter_setOnPacketHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent packet that the channel has received will be reported in this event.
		/// </remarks>
		public event DataAdapterPacketEventHandler Packet;
		internal void OnPacket(DataAdapterPacketEventArgs e) {
			if (Packet != null) {
				foreach (DataAdapterPacketEventHandler PacketHandler in Packet.GetInvocationList()) {
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
		Phidget22Imports.DataAdapterPacketEvent nativePacketEventCallback;
		internal void nativePacketEvent(IntPtr phid, IntPtr ctx, byte[] data, IntPtr dataLen, bool overrun) {
			OnPacket(new DataAdapterPacketEventArgs(data, overrun));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_sendPacket(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_sendPacket_async(IntPtr phid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data, IntPtr dataLen, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_getMaxPacketLength(IntPtr phid, out int MaxPacketLength);
		public delegate void DataAdapterPacketEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data, IntPtr dataLen, bool overrun);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDataAdapter_setOnPacketHandler(IntPtr phid, DataAdapterPacketEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A DataAdapter Packet Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DataAdapterPacketEventArg object contains data and information related to the Event.</param>
	public delegate void DataAdapterPacketEventHandler(object sender, DataAdapterPacketEventArgs e);
	/// <summary> DataAdapter Packet Event data </summary>
	public class DataAdapterPacketEventArgs : EventArgs {
		/// <summary>The packet as an array of bytes.
		/// </summary>
		public readonly byte[] Data;
		/// <summary>Indicates if there has been an overrun in the reveived data buffer. If TRUE, one or more packets
		/// from the master has been lost.
		/// </summary>
		public readonly bool Overrun;
		internal DataAdapterPacketEventArgs(byte[] data, bool overrun) {
			this.Data = data;
			this.Overrun = overrun;
		}
	}

}
