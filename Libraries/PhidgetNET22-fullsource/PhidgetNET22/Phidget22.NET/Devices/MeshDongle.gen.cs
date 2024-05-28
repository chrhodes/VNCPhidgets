using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> MeshDongle class definition </summary>
	public partial class MeshDongle : Phidget {
		#region Constructor/Destructor
		/// <summary> MeshDongle Constructor </summary>
		public MeshDongle() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMeshDongle_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> MeshDongle Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~MeshDongle() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetMeshDongle_delete(ref chandle);
			}
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMeshDongle_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMeshDongle_delete(ref IntPtr phid);
	}
}

namespace Phidget22.Events {
}
