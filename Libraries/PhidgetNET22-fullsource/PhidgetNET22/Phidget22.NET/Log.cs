using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Log class definition </summary>
	public partial class Log {

		/// <summary> </summary>
		/// <remarks>Determies the number of log sources in the system, and the names of those sources.</remarks>
		public static string[] GetSources() {
			ErrorCode result;
			IntPtr[] sourcesPtr = new IntPtr[256];
			int len = sourcesPtr.Length;
			result = Phidget22Imports.PhidgetLog_getSources(sourcesPtr, ref len);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);

			string[] sources = new string[len];
			for (int i = 0; i < len; i++)
				sources[i] = UTF8Marshaler.Instance.MarshalNativeToManaged(sourcesPtr[i]);

			return sources;
		}
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_getSources(IntPtr[] sources, ref int count);

	}
}