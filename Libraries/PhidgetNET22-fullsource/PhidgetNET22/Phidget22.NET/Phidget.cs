using System;
using System.ComponentModel;
using Phidget22.Events;
using System.Runtime.InteropServices;

namespace Phidget22 {
	public partial class Phidget {

		internal IntPtr chandle;
		internal bool managerPhidget = false;

#if DOTNET_FRAMEWORK
		static private Boolean _InvokeEventCallbacks = true;
#endif

		/// <summary>Releases the underlying Phidget if it came from a Manager </summary>
		~Phidget() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
		}

#region Properties

		/// <summary>
		/// When true, event callbacks will be called via Invoke() if Invoke is required.
		/// </summary>
		/// <remarks>
		/// Calling events via Invoke allows you to access the GUI directly from the event callbacks. However, this can lead
		/// to hard to debug dead-lock behaviour is code is not written carefully, so this should be disabled.
		///
		/// However, this is enabled by default to maintain the same behaviour as phidget21.
		/// </remarks>
		static public Boolean InvokeEventCallbacks {
#if DOTNET_FRAMEWORK
			get { return _InvokeEventCallbacks; }
			set { _InvokeEventCallbacks = value; }
#else
			get { return false; }
			set { if (value != false) throw PhidgetException.CreateByCode(ErrorCode.Unsupported); }
#endif
		}

		/// <summary>Finalizes the library </summary>
		/// <remarks>This can be called before the library is unloaded to clean up all memory and threads.</remarks>
		public static void FinalizeLibrary(int flags) {
			ErrorCode result;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			result = Phidget22Imports.Phidget_finalize(flags);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#endregion

		/// <summary> </summary>
		/// <remarks>Opens the Phidget channel. The specific channel to be opened can be specified by setting any of the following properties:<ul><li>DeviceSerialNumber</li> <li>DeviceLabel</li> <li>Channel</li> <li>ServerName</li> <li>IsLocal</li><li>IsRemote</li><li>HubPort</li><li>HubSerialNumber</li><li>HubLabel</li><li>IsHubPortDevice</li></ul>Open will return immediately, with the open proceding asynchronously.Use the Attach event or Attached property to determine when the channel is ready to use.</remarks>
		public void Open() {
			ErrorCode result;
			if (managerPhidget) {
				managerPhidget = false;
				initializeEvents();
			}
			result = Phidget22Imports.Phidget_open(chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}

		/// <summary> </summary>
		/// <remarks>Opens the Phidget channel.The specific channel to be opened can be specified by setting any of the following properties:<ul><li>DeviceSerialNumber</li> <li>DeviceLabel</li> <li>Channel</li> <li>ServerName</li><li> IsLocal</li><li>IsRemote</li> <li>HubPort</li> <li>HubSerialNumber</li> <li>HubLabel</li><li>IsHubPortDevice</li></ul>Open will block until the channel is opened or a timeout occurs. A timeout value of 0 will wait forever.</remarks>
		public void Open(int timeout) {
			ErrorCode result;
			if (managerPhidget) {
				managerPhidget = false;
				initializeEvents();
			}
			result = Phidget22Imports.Phidget_openWaitForAttachment(chandle, timeout);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}

	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_release(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_retain(IntPtr phid);
	}
}