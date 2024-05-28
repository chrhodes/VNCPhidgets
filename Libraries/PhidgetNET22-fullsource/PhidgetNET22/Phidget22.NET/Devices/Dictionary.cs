using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Dictionary class definition </summary>
	public partial class Dictionary : Phidget {

		/// <summary>Gets the value </summary>
		/// <remarks>Gets the value associated with the give key from the dictionary</remarks>
		public string Get(string key) {
			ErrorCode result;
			IntPtr valuePtr = Marshal.AllocHGlobal(65536);
			IntPtr len = new IntPtr(65536);
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			result = Phidget22Imports.PhidgetDictionary_get(chandle, keyPtr, valuePtr, len);
			Marshal.FreeHGlobal(keyPtr);
			if (result == ErrorCode.NoSuchEntity) {
				Marshal.FreeHGlobal(valuePtr);
				return null;
			}
			if (result != 0) {
				Marshal.FreeHGlobal(valuePtr);
				throw PhidgetException.CreateByCode(result);
			}
			String value = UTF8Marshaler.Instance.MarshalNativeToManaged(valuePtr);
			Marshal.FreeHGlobal(valuePtr);
			return value;
		}

		/// <summary>Gets the value </summary>
		/// <remarks>Gets the value associated with the give key from the dictionary</remarks>
		public string Get(string key, string def) {
			ErrorCode result;
			IntPtr valuePtr = Marshal.AllocHGlobal(65536);
			IntPtr len = new IntPtr(65536);
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			result = Phidget22Imports.PhidgetDictionary_get(chandle, keyPtr, valuePtr, len);
			Marshal.FreeHGlobal(keyPtr);
			if (result == ErrorCode.NoSuchEntity) {
				Marshal.FreeHGlobal(valuePtr);
				return def;
			}
			if (result != 0) {
				Marshal.FreeHGlobal(valuePtr);
				throw PhidgetException.CreateByCode(result);
			}
			String value = UTF8Marshaler.Instance.MarshalNativeToManaged(valuePtr);
			Marshal.FreeHGlobal(valuePtr);
			return value.ToString();
		}
		/// <summary>Gets a list of keys.</summary>
		/// <remarks>The list starts at they key following the provided key (an empty string means start at the first key).
		///
		/// The list may not contain all of the keys in the dictionary, and it is implementation specific how many will be returned.
		/// It is the responsibility of the user to call scan() again to get any remaining keys.
		///
		/// When all of the keys have been scanned, an empty list is returned.
		///
		/// Keys added during the scan may be missed, and keys deleted during the scan may be included.</remarks>
		public string[] Scan(string start) {
			ErrorCode result;
			IntPtr keyslistPtr = Marshal.AllocHGlobal(65536);
			IntPtr len = new IntPtr(65536);
			IntPtr startPtr = UTF8Marshaler.Instance.MarshalManagedToNative(start);
			result = Phidget22Imports.PhidgetDictionary_scan(chandle, startPtr, keyslistPtr, len);
			Marshal.FreeHGlobal(startPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(keyslistPtr);
				throw PhidgetException.CreateByCode(result);
			}
			String keyslist = UTF8Marshaler.Instance.MarshalNativeToManaged(keyslistPtr);
			Marshal.FreeHGlobal(keyslistPtr);
			return keyslist.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_scan(IntPtr phid, IntPtr start, IntPtr keyslist, IntPtr len);
	}
}
