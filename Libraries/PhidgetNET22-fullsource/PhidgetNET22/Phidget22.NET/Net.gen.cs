using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Net class definition </summary>
	public partial class Net {

		#region Constants

		/// <summary> PhidgetServer flag indicating that the server requires a password to authenticate </summary>
		public const int AuthRequired = 1;
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Registers a server that the client (your program) will try to connect to. The client will
		/// continually try to connect to the server, increasing the time between each attempt to a maximum
		/// interval of 16 seconds.
		/// 
		/// This call is intended for use when server discovery is not enabled, or to connect to a server that
		/// is not discoverable.
		/// 
		/// The server name used by this function does not have to match the name of the server running on the
		/// host machine. Only the address, port, and password need to match.
		/// 
		/// This call will fail if a server with the same name has already been discovered.
		/// 
		/// This call will fail if <c>SetServerPassword()</c> has already been called with the same
		/// server name, as <c>SetServerPassword()</c> registers the server entry anticipating the
		/// discovery of the server.
		/// 
		/// See:
		/// <list>
		/// <item><c>RemoveServer()</c></item>
		/// <item><c>EnableServerDiscovery()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void AddServer(string serverName, string address, int port, string password, int flags) {
			ErrorCode result;
			IntPtr serverNamePtr = UTF8Marshaler.Instance.MarshalManagedToNative(serverName);
			IntPtr addressPtr = UTF8Marshaler.Instance.MarshalManagedToNative(address);
			IntPtr passwordPtr = UTF8Marshaler.Instance.MarshalManagedToNative(password);
			result = Phidget22Imports.PhidgetNet_addServer(serverNamePtr, addressPtr, port, passwordPtr, flags);
			if (result != 0) {
				Marshal.FreeHGlobal(serverNamePtr);
				Marshal.FreeHGlobal(addressPtr);
				Marshal.FreeHGlobal(passwordPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(serverNamePtr);
			Marshal.FreeHGlobal(addressPtr);
			Marshal.FreeHGlobal(passwordPtr);
		}

		/// <summary> </summary>
		/// <remarks>Removes a registration for a server that the client (your program) is trying to connect to.If the
		/// client is currently connected to the server, the connection will be closed.
		/// 
		/// If the server was discovered (not added by <c>AddServer()</c>), the connection may be
		/// reestablished if and when the server is rediscovered. <c>DisableServer()</c> should be used
		/// to prevent the reconnection of a discovered server
		/// 
		/// See:
		/// <list>
		/// <item><c>AddServer()</c></item>
		/// <item><c>DisableServer()</c></item>
		/// <item><c>DisableServerDiscovery()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void RemoveServer(string serverName) {
			ErrorCode result;
			IntPtr serverNamePtr = UTF8Marshaler.Instance.MarshalManagedToNative(serverName);
			result = Phidget22Imports.PhidgetNet_removeServer(serverNamePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(serverNamePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(serverNamePtr);
		}

		/// <summary> </summary>
		/// <remarks>Enables attempts to connect to a discovered server, if attempts were previously disabled by
		/// <c>DisableServer()</c>. All servers are enabled by default.
		/// 
		/// This call will fail if the server was not previously added, disabled or discovered.
		/// 
		/// See:
		/// <list>
		/// <item><c>DisableServer()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void EnableServer(string serverName) {
			ErrorCode result;
			IntPtr serverNamePtr = UTF8Marshaler.Instance.MarshalManagedToNative(serverName);
			result = Phidget22Imports.PhidgetNet_enableServer(serverNamePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(serverNamePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(serverNamePtr);
		}

		/// <summary> </summary>
		/// <remarks>Prevents attempts to automatically connect to a server.
		/// <p>By default the client (your program) will continually attempt to connect to added or discovered
		/// servers.This call will disable those attempts, but will not close an already established
		/// connection.
		/// 
		/// See:</p>
		/// <list>
		/// <item><c>AddServer()</c></item>
		/// <item><c>EnableServer()</c></item>
		/// <item><c>EnableServerDiscovery()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void DisableServer(string serverName, int flags) {
			ErrorCode result;
			IntPtr serverNamePtr = UTF8Marshaler.Instance.MarshalManagedToNative(serverName);
			result = Phidget22Imports.PhidgetNet_disableServer(serverNamePtr, flags);
			if (result != 0) {
				Marshal.FreeHGlobal(serverNamePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(serverNamePtr);
		}

		/// <summary> </summary>
		/// <remarks>Enables the dynamic discovery of servers that publish their identity to the network. Currently
		/// Multicast DNS is used to discover and publish Phidget servers.
		/// <p>To connect to remote Phidgets, call this function with server type <b>DEVICEREMOTE</b>.</p>
		/// <p><c>EnableServerDiscovery()</c> must be called once for each server type your program
		/// requires. Multiple calls for the same server type are ignored</p>
		/// <p>This call will fail with the error code <b>EPHIDGET_UNSUPPORTED</b> if your computer does not
		/// have the required mDNS support. We recommend using Bonjour Print Services on Windows and Mac, or
		/// Avahi on Linux.</p>
		/// 
		/// 
		/// See:
		/// <list>
		/// <item><c>DisableServerDiscovery()</c></item>
		/// <item><c>AddServer()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void EnableServerDiscovery(ServerType serverType) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetNet_enableServerDiscovery(serverType);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Disables the dynamic discovery of servers that publish their identity.
		/// <p><c>DisableServerDiscovery()</c> does not disconnect already established connections.
		/// 
		/// See:</p>
		/// <list>
		/// <item><c>EnableServerDiscovery()</c></item>
		/// <item><c>DisableServer()</c></item>
		/// <item><c>RemoveServer()</c></item>
		/// </list>
		/// 
		/// </remarks>
		public static void DisableServerDiscovery(ServerType serverType) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetNet_disableServerDiscovery(serverType);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Sets the password that will be used to attempt to connect to the server. If the server has not
		/// already been added or discovered, a placeholder server entry will be registered to use this
		/// password on the server once it is discovered.
		/// </remarks>
		public static void SetServerPassword(string serverName, string password) {
			ErrorCode result;
			IntPtr serverNamePtr = UTF8Marshaler.Instance.MarshalManagedToNative(serverName);
			IntPtr passwordPtr = UTF8Marshaler.Instance.MarshalManagedToNative(password);
			result = Phidget22Imports.PhidgetNet_setServerPassword(serverNamePtr, passwordPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(serverNamePtr);
				Marshal.FreeHGlobal(passwordPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(serverNamePtr);
			Marshal.FreeHGlobal(passwordPtr);
		}
		#endregion

		#region Events
		static internal void initializeStaticEvents() {
			ErrorCode result;
			nativeServerAddedEventCallback = new Phidget22Imports.NetServerAddedEvent(nativeServerAddedEvent);
			result = Phidget22Imports.PhidgetNet_setOnServerAddedHandler(nativeServerAddedEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeServerRemovedEventCallback = new Phidget22Imports.NetServerRemovedEvent(nativeServerRemovedEvent);
			result = Phidget22Imports.PhidgetNet_setOnServerRemovedHandler(nativeServerRemovedEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Subscribe to this event if you would like to know when a server has been added.
		/// </remarks>
		static public event NetServerAddedEventHandler ServerAdded;
		static internal void OnServerAdded(NetServerAddedEventArgs e) {
			if (ServerAdded != null) {
				foreach (NetServerAddedEventHandler ServerAddedHandler in ServerAdded.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = ServerAddedHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(ServerAddedHandler, new object[] { e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						ServerAddedHandler(e);
				}
			}
		}
		static Phidget22Imports.NetServerAddedEvent nativeServerAddedEventCallback;
		static internal void nativeServerAddedEvent(IntPtr ctx, IntPtr server, IntPtr kv) {
			OnServerAdded(new NetServerAddedEventArgs(PhidgetServerMarshaler.Instance.MarshalNativeToManaged(server), kv));
		}
		/// <summary>  </summary>
		/// <remarks>Subscribe to this event if you would like to know when a server has been removed.
		/// </remarks>
		static public event NetServerRemovedEventHandler ServerRemoved;
		static internal void OnServerRemoved(NetServerRemovedEventArgs e) {
			if (ServerRemoved != null) {
				foreach (NetServerRemovedEventHandler ServerRemovedHandler in ServerRemoved.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = ServerRemovedHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(ServerRemovedHandler, new object[] { e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						ServerRemovedHandler(e);
				}
			}
		}
		static Phidget22Imports.NetServerRemovedEvent nativeServerRemovedEventCallback;
		static internal void nativeServerRemovedEvent(IntPtr ctx, IntPtr server) {
			OnServerRemoved(new NetServerRemovedEventArgs(PhidgetServerMarshaler.Instance.MarshalNativeToManaged(server)));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_addServer(IntPtr serverName, IntPtr address, int port, IntPtr password, int flags);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_removeServer(IntPtr serverName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_enableServer(IntPtr serverName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_disableServer(IntPtr serverName, int flags);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_enableServerDiscovery(ServerType serverType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_disableServerDiscovery(ServerType serverType);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_setServerPassword(IntPtr serverName, IntPtr password);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_startServer(int flags, int addressFamily, IntPtr serverName, IntPtr address, int port, IntPtr password, IntPtr server);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_stopServer(out PhidgetServer server);
		public delegate void NetServerAddedEvent(IntPtr ctx, IntPtr server, IntPtr kv);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_setOnServerAddedHandler(NetServerAddedEvent fptr, IntPtr ctx);
		public delegate void NetServerRemovedEvent(IntPtr ctx, IntPtr server);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetNet_setOnServerRemovedHandler(NetServerRemovedEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Net ServerAdded Event delegate </summary>
	/// <param name="e">A NetServerAddedEventArg object contains data and information related to the Event.</param>
	public delegate void NetServerAddedEventHandler(NetServerAddedEventArgs e);
	/// <summary> Net ServerAdded Event data </summary>
	public class NetServerAddedEventArgs : EventArgs {
		/// <summary>The server that has been added.
		/// </summary>
		public readonly PhidgetServer Server;
		/// <summary>Opaque structure containing keys related to the server
		/// </summary>
		public readonly IntPtr Kv;
		internal NetServerAddedEventArgs(PhidgetServer server, IntPtr kv) {
			this.Server = server;
			this.Kv = kv;
		}
	}

	/// <summary> A Net ServerRemoved Event delegate </summary>
	/// <param name="e">A NetServerRemovedEventArg object contains data and information related to the Event.</param>
	public delegate void NetServerRemovedEventHandler(NetServerRemovedEventArgs e);
	/// <summary> Net ServerRemoved Event data </summary>
	public class NetServerRemovedEventArgs : EventArgs {
		/// <summary>The server that has been removed.
		/// </summary>
		public readonly PhidgetServer Server;
		internal NetServerRemovedEventArgs(PhidgetServer server) {
			this.Server = server;
		}
	}

}
