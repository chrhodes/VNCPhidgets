using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> RFID class definition </summary>
	public partial class RFID : Phidget {
		#region Constructor/Destructor
		/// <summary> RFID Constructor </summary>
		public RFID() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetRFID_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> RFID Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~RFID() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetRFID_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The state of the antenna </summary>
		/// <remarks>The on/off state of the antenna.
		/// <list>
		/// <item>You can turn the antenna off to save power.</item>
		/// <item>You must turn the antenna on in order to detect and read RFID tags.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool AntennaEnabled {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetRFID_getAntennaEnabled(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRFID_setAntennaEnabled(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> Tag is in range </summary>
		/// <remarks>This property is true if a compatibile RFID tag is being read by the reader.
		/// <list>
		/// <item><c>TagPresent</c> will remain true until the tag is out of range and can no longer be
		/// read.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool TagPresent {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetRFID_getTagPresent(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Gets the most recently read tag's data, even if that tag is no longer within read range.
		/// <list>
		/// <item>Only valid after at least one tag has been read.</item>
		/// </list>
		/// 
		/// </remarks>
		public RFIDTag GetLastTag() {
			ErrorCode result;
			IntPtr tagStringPtr = Marshal.AllocHGlobal(25);
			string tagString;
			IntPtr tagStringLen = new IntPtr(25);
			RFIDProtocol protocol;
			result = Phidget22Imports.PhidgetRFID_getLastTag(chandle, tagStringPtr, tagStringLen, out protocol);
			if (result != 0) {
				Marshal.FreeHGlobal(tagStringPtr);
				throw PhidgetException.CreateByCode(result);
			}
			tagString = UTF8Marshaler.Instance.MarshalNativeToManaged(tagStringPtr);
			RFIDTag ret = new RFIDTag();
			ret.TagString = tagString;
			ret.Protocol = protocol;
			Marshal.FreeHGlobal(tagStringPtr);
			return ret;
		}

		/// <summary> </summary>
		/// <remarks>Writes data to the tag being currently read by the reader.
		/// <list>
		/// <item>You cannot write to a read-only or locked tag.</item>
		/// </list>
		/// 
		/// </remarks>
		public void Write(string tagString, RFIDProtocol protocol, bool lockTag) {
			ErrorCode result;
			IntPtr tagStringPtr = UTF8Marshaler.Instance.MarshalManagedToNative(tagString);
			result = Phidget22Imports.PhidgetRFID_write(chandle, tagStringPtr, protocol, lockTag);
			if (result != 0) {
				Marshal.FreeHGlobal(tagStringPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(tagStringPtr);
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeTagEventCallback = new Phidget22Imports.RFIDTagEvent(nativeTagEvent);
			result = Phidget22Imports.PhidgetRFID_setOnTagHandler(chandle, nativeTagEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeTagLostEventCallback = new Phidget22Imports.RFIDTagLostEvent(nativeTagLostEvent);
			result = Phidget22Imports.PhidgetRFID_setOnTagLostHandler(chandle, nativeTagLostEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetRFID_setOnTagHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetRFID_setOnTagLostHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when an RFID tag is read.
		/// </remarks>
		public event RFIDTagEventHandler Tag;
		internal void OnTag(RFIDTagEventArgs e) {
			if (Tag != null) {
				foreach (RFIDTagEventHandler TagHandler in Tag.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TagHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TagHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TagHandler(this, e);
				}
			}
		}
		Phidget22Imports.RFIDTagEvent nativeTagEventCallback;
		internal void nativeTagEvent(IntPtr phid, IntPtr ctx, IntPtr tag, RFIDProtocol protocol) {
			OnTag(new RFIDTagEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(tag), protocol));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when an RFID tag that was being read is removed from the read range.
		/// </remarks>
		public event RFIDTagLostEventHandler TagLost;
		internal void OnTagLost(RFIDTagLostEventArgs e) {
			if (TagLost != null) {
				foreach (RFIDTagLostEventHandler TagLostHandler in TagLost.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TagLostHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TagLostHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TagLostHandler(this, e);
				}
			}
		}
		Phidget22Imports.RFIDTagLostEvent nativeTagLostEventCallback;
		internal void nativeTagLostEvent(IntPtr phid, IntPtr ctx, IntPtr tag, RFIDProtocol protocol) {
			OnTagLost(new RFIDTagLostEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(tag), protocol));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_getLastTag(IntPtr phid, IntPtr tagString, IntPtr tagStringLen, out RFIDProtocol protocol);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_write(IntPtr phid, IntPtr tagString, RFIDProtocol protocol, bool lockTag);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_getAntennaEnabled(IntPtr phid, out bool AntennaEnabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_setAntennaEnabled(IntPtr phid, bool AntennaEnabled);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_getTagPresent(IntPtr phid, out bool TagPresent);
		public delegate void RFIDTagEvent(IntPtr phid, IntPtr ctx, IntPtr Tag, RFIDProtocol Protocol);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_setOnTagHandler(IntPtr phid, RFIDTagEvent fptr, IntPtr ctx);
		public delegate void RFIDTagLostEvent(IntPtr phid, IntPtr ctx, IntPtr Tag, RFIDProtocol Protocol);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRFID_setOnTagLostHandler(IntPtr phid, RFIDTagLostEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A RFID Tag Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A RFIDTagEventArg object contains data and information related to the Event.</param>
	public delegate void RFIDTagEventHandler(object sender, RFIDTagEventArgs e);
	/// <summary> RFID Tag Event data </summary>
	public class RFIDTagEventArgs : EventArgs {
		/// <summary>Data from the tag
		/// </summary>
		public readonly string Tag;
		/// <summary>Communication protocol of the tag
		/// </summary>
		public readonly RFIDProtocol Protocol;
		internal RFIDTagEventArgs(string tag, RFIDProtocol protocol) {
			this.Tag = tag;
			this.Protocol = protocol;
		}
	}

	/// <summary> A RFID TagLost Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A RFIDTagLostEventArg object contains data and information related to the Event.</param>
	public delegate void RFIDTagLostEventHandler(object sender, RFIDTagLostEventArgs e);
	/// <summary> RFID TagLost Event data </summary>
	public class RFIDTagLostEventArgs : EventArgs {
		/// <summary>Data from the lost tag
		/// </summary>
		public readonly string Tag;
		/// <summary>Communication protocol of the lost tag
		/// </summary>
		public readonly RFIDProtocol Protocol;
		internal RFIDTagLostEventArgs(string tag, RFIDProtocol protocol) {
			this.Tag = tag;
			this.Protocol = protocol;
		}
	}

}
