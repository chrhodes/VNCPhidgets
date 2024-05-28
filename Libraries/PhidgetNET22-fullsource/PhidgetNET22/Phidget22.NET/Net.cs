using System;
using System.ComponentModel;
using Phidget22.Events;
using System.Runtime.InteropServices;

namespace Phidget22 {
	public partial class Net {
		static Net() {
			initializeStaticEvents();
		}
		#region keyvalue
		/// <summary> Gets a string from the server added kv </summary>
		public static string GetKeyValue_String(IntPtr kv, string key, string def) {
			//We are making a copy of the string before passing it on..
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			IntPtr defPtr = UTF8Marshaler.Instance.MarshalManagedToNative(def);
			IntPtr valPtr = Phidget22Imports.kvgetstrc(kv, keyPtr, defPtr);
			Marshal.FreeHGlobal(keyPtr);
			Marshal.FreeHGlobal(defPtr);
			string val = UTF8Marshaler.Instance.MarshalNativeToManaged(valPtr);
			return "" + val;
		}
		/// <summary> Gets an Int32 from the server added kv </summary>
		public static long GetKeyValue_Int64(IntPtr kv, string key, long def) {
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			long val = Phidget22Imports.kvgeti64(kv, keyPtr, def);
			Marshal.FreeHGlobal(keyPtr);
			return val;
		}
		/// <summary> Gets aa Int64 from the server added kv </summary>
		public static int GetKeyValue_Int32(IntPtr kv, string key, int def) {
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			int val = Phidget22Imports.kvgeti32(kv, keyPtr, def);
			Marshal.FreeHGlobal(keyPtr);
			return val;
		}
		/// <summary> Gets a bool from the server added kv </summary>
		public static bool GetKeyValue_Boolean(IntPtr kv, string key, bool def) {
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			bool val = Phidget22Imports.kvgetbool(kv, keyPtr, def);
			Marshal.FreeHGlobal(keyPtr);
			return val;
		}
		/// <summary> Checks if a key exists in the server added kv </summary>
		public static bool GetKeyValueExists(IntPtr kv, string key) {
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			bool val = Phidget22Imports.kvhasvalue(kv, keyPtr);
			Marshal.FreeHGlobal(keyPtr);
			return val;
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern IntPtr kvgetstrc(IntPtr kv, IntPtr key, IntPtr def);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern long kvgeti64(IntPtr kv, IntPtr key, long def);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern int kvgeti32(IntPtr kv, IntPtr key, int def);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern bool kvgetbool(IntPtr kv, IntPtr key, bool def);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern bool kvhasvalue(IntPtr kv, IntPtr key);
	}
}

