using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Manager class definition </summary>
	public partial class Manager {
		#region Constructor/Destructor
		/// <summary> Manager Constructor </summary>
		public Manager() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetManager_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Manager Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Manager() {
			uninitializeEvents();
			Phidget22Imports.PhidgetManager_delete(ref chandle);
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Closes a Phidget Manager that has been opened. <c>Close()</c> will release the Phidget
		/// Manager, and should be called prior to delete.
		/// </remarks>
		public void Close() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetManager_close(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Opens the Phidget Manager.
		/// 
		/// Be sure to register <b>Attach</b> and <b>Detach</b> event handlers for the Manager before opening
		/// it, to ensure you program doesn't miss the events reported for devices already connected to your
		/// system.
		/// </remarks>
		public void Open() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetManager_open(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal virtual void initializeEvents() { initializeBaseEvents(); }
		internal void initializeBaseEvents() {
			ErrorCode result;
			nativeAttachEventCallback = new Phidget22Imports.ManagerAttachEvent(nativeAttachEvent);
			result = Phidget22Imports.PhidgetManager_setOnAttachHandler(chandle, nativeAttachEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeDetachEventCallback = new Phidget22Imports.ManagerDetachEvent(nativeDetachEvent);
			result = Phidget22Imports.PhidgetManager_setOnDetachHandler(chandle, nativeDetachEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal virtual void uninitializeEvents() { uninitializeBaseEvents(); }
		internal void uninitializeBaseEvents() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetManager_setOnAttachHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetManager_setOnDetachHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a channel is attached.
		/// <list>
		/// <item>Phidget channels you get from the manager are informational only, you can read information
		/// about them such as serial number, class, name, etc. but they are not opened. In order to interact
		/// with one, you must <c>create</c> and <c>open</c> a Phidget object of the correct
		/// type.</item>
		/// </list>
		/// 
		/// </remarks>
		public event ManagerAttachEventHandler Attach;
		internal void OnAttach(ManagerAttachEventArgs e) {
			if (Attach != null) {
				foreach (ManagerAttachEventHandler AttachHandler in Attach.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = AttachHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(AttachHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						AttachHandler(this, e);
				}
			}
		}
		Phidget22Imports.ManagerAttachEvent nativeAttachEventCallback;
		internal void nativeAttachEvent(IntPtr phid, IntPtr ctx, IntPtr channel) {
			OnAttach(new ManagerAttachEventArgs(PhidgetMarshaler.Instance.MarshalNativeToManaged(channel)));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a channel is detached.
		/// <list>
		/// <item>Phidget channels you get from the manager are informational only, you can read information
		/// about them such as serial number, class, name, etc. but they are not opened. In order to interact
		/// with one, you must <c>create</c> and <c>open</c> a Phidget object of the correct
		/// type.</item>
		/// </list>
		/// 
		/// </remarks>
		public event ManagerDetachEventHandler Detach;
		internal void OnDetach(ManagerDetachEventArgs e) {
			if (Detach != null) {
				foreach (ManagerDetachEventHandler DetachHandler in Detach.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = DetachHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(DetachHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						DetachHandler(this, e);
				}
			}
		}
		Phidget22Imports.ManagerDetachEvent nativeDetachEventCallback;
		internal void nativeDetachEvent(IntPtr phid, IntPtr ctx, IntPtr channel) {
			OnDetach(new ManagerDetachEventArgs(PhidgetMarshaler.Instance.MarshalNativeToManaged(channel)));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_create(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_delete(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_close(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_open(IntPtr phid);
		public delegate void ManagerAttachEvent(IntPtr phid, IntPtr ctx, IntPtr Channel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_setOnAttachHandler(IntPtr phid, ManagerAttachEvent fptr, IntPtr ctx);
		public delegate void ManagerDetachEvent(IntPtr phid, IntPtr ctx, IntPtr Channel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetManager_setOnDetachHandler(IntPtr phid, ManagerDetachEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Manager Attach Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A ManagerAttachEventArg object contains data and information related to the Event.</param>
	public delegate void ManagerAttachEventHandler(object sender, ManagerAttachEventArgs e);
	/// <summary> Manager Attach Event data </summary>
	public class ManagerAttachEventArgs : EventArgs {
		/// <summary>The Phidget channel that attached
		/// </summary>
		public readonly Phidget Channel;
		internal ManagerAttachEventArgs(Phidget channel) {
			this.Channel = channel;
		}
	}

	/// <summary> A Manager Detach Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A ManagerDetachEventArg object contains data and information related to the Event.</param>
	public delegate void ManagerDetachEventHandler(object sender, ManagerDetachEventArgs e);
	/// <summary> Manager Detach Event data </summary>
	public class ManagerDetachEventArgs : EventArgs {
		/// <summary>The Phidget channel that detached
		/// </summary>
		public readonly Phidget Channel;
		internal ManagerDetachEventArgs(Phidget channel) {
			this.Channel = channel;
		}
	}

}
