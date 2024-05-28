using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Dictionary class definition </summary>
	public partial class Dictionary : Phidget {
		#region Constructor/Destructor
		/// <summary> Dictionary Constructor </summary>
		public Dictionary() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDictionary_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Dictionary Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Dictionary() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDictionary_delete(ref chandle);
			}
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Adds a new dictionary that exports the control interface from the system
		/// </remarks>
		public static void EnableControlDictionary() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDictionary_enableControlDictionary();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Adds a new dictionary to the system.
		/// <p>The serial number must be greater than 1000.</p>
		/// 
		/// </remarks>
		public static void AddDictionary(int deviceSerialNumber, string label) {
			ErrorCode result;
			IntPtr labelPtr = UTF8Marshaler.Instance.MarshalManagedToNative(label);
			result = Phidget22Imports.PhidgetDictionary_addDictionary(deviceSerialNumber, labelPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(labelPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(labelPtr);
		}

		/// <summary> </summary>
		/// <remarks>Removes a dictionary from the system.
		/// </remarks>
		public static void RemoveDictionary(int deviceSerialNumber) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDictionary_removeDictionary(deviceSerialNumber);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Loads data from a file into the specified dictionary.
		/// <list>
		/// <item>The data is loaded from a file of the form key=value</item>
		/// <item>Blank lines are ignored</item>
		/// <item>Whitespace before and after the key and value is stripped</item>
		/// <item>Only the first = is observed</item>
		/// <item>Lines starting with # are ignored</item>
		/// </list>
		/// 
		/// </remarks>
		public static void LoadDictionary(int dictionarySerialNumber, string file) {
			ErrorCode result;
			IntPtr filePtr = UTF8Marshaler.Instance.MarshalManagedToNative(file);
			result = Phidget22Imports.PhidgetDictionary_loadDictionary(dictionarySerialNumber, filePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(filePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(filePtr);
		}

		/// <summary> </summary>
		/// <remarks>Adds a new dictionary that exports runtime statistics from the system
		/// </remarks>
		public static void EnableStatsDictionary() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDictionary_enableStatsDictionary();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Adds a new key value pair to the dictionary. It is an error if the key already exits.
		/// </remarks>
		public void Add(string key, string value) {
			ErrorCode result;
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			IntPtr valuePtr = UTF8Marshaler.Instance.MarshalManagedToNative(value);
			result = Phidget22Imports.PhidgetDictionary_add(chandle, keyPtr, valuePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(keyPtr);
				Marshal.FreeHGlobal(valuePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(keyPtr);
			Marshal.FreeHGlobal(valuePtr);
		}

		/// <summary> </summary>
		/// <remarks>Removes every key from the dictionary
		/// </remarks>
		public void RemoveAll() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDictionary_removeAll(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Removes the key from the dictionary
		/// </remarks>
		public void Remove(string key) {
			ErrorCode result;
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			result = Phidget22Imports.PhidgetDictionary_remove(chandle, keyPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(keyPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(keyPtr);
		}

		/// <summary> </summary>
		/// <remarks>Sets the value of a key, or creates the key value pair if the key does not already exist.
		/// </remarks>
		public void Set(string key, string value) {
			ErrorCode result;
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			IntPtr valuePtr = UTF8Marshaler.Instance.MarshalManagedToNative(value);
			result = Phidget22Imports.PhidgetDictionary_set(chandle, keyPtr, valuePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(keyPtr);
				Marshal.FreeHGlobal(valuePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(keyPtr);
			Marshal.FreeHGlobal(valuePtr);
		}

		/// <summary> </summary>
		/// <remarks>Updates a key value pair in the dictionary. It is an error if the key does not exist.
		/// </remarks>
		public void Update(string key, string value) {
			ErrorCode result;
			IntPtr keyPtr = UTF8Marshaler.Instance.MarshalManagedToNative(key);
			IntPtr valuePtr = UTF8Marshaler.Instance.MarshalManagedToNative(value);
			result = Phidget22Imports.PhidgetDictionary_update(chandle, keyPtr, valuePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(keyPtr);
				Marshal.FreeHGlobal(valuePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(keyPtr);
			Marshal.FreeHGlobal(valuePtr);
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeKeyAddEventCallback = new Phidget22Imports.DictionaryKeyAddEvent(nativeKeyAddEvent);
			result = Phidget22Imports.PhidgetDictionary_setOnAddHandler(chandle, nativeKeyAddEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeKeyRemoveEventCallback = new Phidget22Imports.DictionaryKeyRemoveEvent(nativeKeyRemoveEvent);
			result = Phidget22Imports.PhidgetDictionary_setOnRemoveHandler(chandle, nativeKeyRemoveEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeKeyValueUpdateEventCallback = new Phidget22Imports.DictionaryKeyValueUpdateEvent(nativeKeyValueUpdateEvent);
			result = Phidget22Imports.PhidgetDictionary_setOnUpdateHandler(chandle, nativeKeyValueUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetDictionary_setOnAddHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetDictionary_setOnRemoveHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetDictionary_setOnUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a new key value pair is added to the dictionary.
		/// </remarks>
		public event DictionaryKeyAddEventHandler KeyAdd;
		internal void OnKeyAdd(DictionaryKeyAddEventArgs e) {
			if (KeyAdd != null) {
				foreach (DictionaryKeyAddEventHandler KeyAddHandler in KeyAdd.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = KeyAddHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(KeyAddHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						KeyAddHandler(this, e);
				}
			}
		}
		Phidget22Imports.DictionaryKeyAddEvent nativeKeyAddEventCallback;
		internal void nativeKeyAddEvent(IntPtr phid, IntPtr ctx, IntPtr key, IntPtr value) {
			OnKeyAdd(new DictionaryKeyAddEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(key), UTF8Marshaler.Instance.MarshalNativeToManaged(value)));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a key is removed from the dictionary.
		/// </remarks>
		public event DictionaryKeyRemoveEventHandler KeyRemove;
		internal void OnKeyRemove(DictionaryKeyRemoveEventArgs e) {
			if (KeyRemove != null) {
				foreach (DictionaryKeyRemoveEventHandler KeyRemoveHandler in KeyRemove.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = KeyRemoveHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(KeyRemoveHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						KeyRemoveHandler(this, e);
				}
			}
		}
		Phidget22Imports.DictionaryKeyRemoveEvent nativeKeyRemoveEventCallback;
		internal void nativeKeyRemoveEvent(IntPtr phid, IntPtr ctx, IntPtr key) {
			OnKeyRemove(new DictionaryKeyRemoveEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(key)));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a change is made to a key value pair in the dictionary.
		/// </remarks>
		public event DictionaryKeyValueUpdateEventHandler KeyValueUpdate;
		internal void OnKeyValueUpdate(DictionaryKeyValueUpdateEventArgs e) {
			if (KeyValueUpdate != null) {
				foreach (DictionaryKeyValueUpdateEventHandler KeyValueUpdateHandler in KeyValueUpdate.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = KeyValueUpdateHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(KeyValueUpdateHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						KeyValueUpdateHandler(this, e);
				}
			}
		}
		Phidget22Imports.DictionaryKeyValueUpdateEvent nativeKeyValueUpdateEventCallback;
		internal void nativeKeyValueUpdateEvent(IntPtr phid, IntPtr ctx, IntPtr key, IntPtr value) {
			OnKeyValueUpdate(new DictionaryKeyValueUpdateEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(key), UTF8Marshaler.Instance.MarshalNativeToManaged(value)));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_enableControlDictionary();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_addDictionary(int deviceSerialNumber, IntPtr label);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_removeDictionary(int deviceSerialNumber);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_loadDictionary(int dictionarySerialNumber, IntPtr file);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_enableStatsDictionary();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_add(IntPtr phid, IntPtr key, IntPtr value);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_removeAll(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_get(IntPtr phid, IntPtr key, IntPtr value, IntPtr valueLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_remove(IntPtr phid, IntPtr key);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_set(IntPtr phid, IntPtr key, IntPtr value);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_update(IntPtr phid, IntPtr key, IntPtr value);
		public delegate void DictionaryKeyAddEvent(IntPtr phid, IntPtr ctx, IntPtr key, IntPtr value);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_setOnAddHandler(IntPtr phid, DictionaryKeyAddEvent fptr, IntPtr ctx);
		public delegate void DictionaryKeyRemoveEvent(IntPtr phid, IntPtr ctx, IntPtr key);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_setOnRemoveHandler(IntPtr phid, DictionaryKeyRemoveEvent fptr, IntPtr ctx);
		public delegate void DictionaryKeyValueUpdateEvent(IntPtr phid, IntPtr ctx, IntPtr key, IntPtr value);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDictionary_setOnUpdateHandler(IntPtr phid, DictionaryKeyValueUpdateEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Dictionary KeyAdd Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DictionaryKeyAddEventArg object contains data and information related to the Event.</param>
	public delegate void DictionaryKeyAddEventHandler(object sender, DictionaryKeyAddEventArgs e);
	/// <summary> Dictionary KeyAdd Event data </summary>
	public class DictionaryKeyAddEventArgs : EventArgs {
		/// <summary>The key that was added
		/// </summary>
		public readonly string Key;
		/// <summary>The value of the new key
		/// </summary>
		public readonly string Value;
		internal DictionaryKeyAddEventArgs(string key, string value) {
			this.Key = key;
			this.Value = value;
		}
	}

	/// <summary> A Dictionary KeyRemove Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DictionaryKeyRemoveEventArg object contains data and information related to the Event.</param>
	public delegate void DictionaryKeyRemoveEventHandler(object sender, DictionaryKeyRemoveEventArgs e);
	/// <summary> Dictionary KeyRemove Event data </summary>
	public class DictionaryKeyRemoveEventArgs : EventArgs {
		/// <summary>The key that was removed
		/// </summary>
		public readonly string Key;
		internal DictionaryKeyRemoveEventArgs(string key) {
			this.Key = key;
		}
	}

	/// <summary> A Dictionary KeyValueUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DictionaryKeyValueUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void DictionaryKeyValueUpdateEventHandler(object sender, DictionaryKeyValueUpdateEventArgs e);
	/// <summary> Dictionary KeyValueUpdate Event data </summary>
	public class DictionaryKeyValueUpdateEventArgs : EventArgs {
		/// <summary>The key whose value was updated
		/// </summary>
		public readonly string Key;
		/// <summary>The new value
		/// </summary>
		public readonly string Value;
		internal DictionaryKeyValueUpdateEventArgs(string key, string value) {
			this.Key = key;
			this.Value = value;
		}
	}

}
