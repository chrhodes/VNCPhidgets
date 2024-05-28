using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Log class definition </summary>
	public partial class Log {

		#region Properties
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Disables logging within the Phidget library.
		/// </remarks>
		public static void Disable() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_disable();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Enables logging within the Phidget library.
		/// </remarks>
		public static void Enable(LogLevel level, string destination) {
			ErrorCode result;
			IntPtr destinationPtr = UTF8Marshaler.Instance.MarshalManagedToNative(destination);
			result = Phidget22Imports.PhidgetLog_enable(level, destinationPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(destinationPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(destinationPtr);
		}

		/// <summary> </summary>
		/// <remarks>Gets the log level for the phidget22 source.
		/// </remarks>
		public static LogLevel GetLevel() {
			ErrorCode result;
			LogLevel level;
			result = Phidget22Imports.PhidgetLog_getLevel(out level);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			return level;
		}

		/// <summary> </summary>
		/// <remarks>Sets the log level for all sources not prefaced with _phidget22.
		/// </remarks>
		public static void SetLevel(LogLevel level) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_setLevel(level);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Manually rotate the log file. This will only have an effect if automatic rotation is disabled and
		/// the log file is larger than the specified maximum file size.
		/// </remarks>
		public static void Rotate() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_rotate();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Determines if the library is automatically rotating the log file
		/// </remarks>
		public static bool IsRotating() {
			ErrorCode result;
			bool isrotating;
			result = Phidget22Imports.PhidgetLog_isRotating(out isrotating);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			return isrotating;
		}

		/// <summary> </summary>
		/// <remarks>Gets the current log rotation parameters
		/// </remarks>
		public static LogRotating GetRotating() {
			ErrorCode result;
			long size;
			int keepCount;
			result = Phidget22Imports.PhidgetLog_getRotating(out size, out keepCount);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			LogRotating ret = new LogRotating();
			ret.Size = size;
			ret.KeepCount = keepCount;
			return ret;
		}

		/// <summary> </summary>
		/// <remarks>Sets log rotation parameters.
		/// </remarks>
		public static void SetRotating(long size, int keepCount) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_setRotating(size, keepCount);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Enables automatic rotation of the log file (the default).
		/// </remarks>
		public static void EnableRotating() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_enableRotating();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Disables automatic rotation of the log file.
		/// </remarks>
		public static void DisableRotating() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetLog_disableRotating();
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Adds a source to the Phidget logging system. This is useful for declaring a source and setting its
		/// log level before sending any messages.
		/// </remarks>
		public static void AddSource(string source, LogLevel level) {
			ErrorCode result;
			IntPtr sourcePtr = UTF8Marshaler.Instance.MarshalManagedToNative(source);
			result = Phidget22Imports.PhidgetLog_addSource(sourcePtr, level);
			if (result != 0) {
				Marshal.FreeHGlobal(sourcePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(sourcePtr);
		}

		/// <summary> </summary>
		/// <remarks>Gets the log level of the specified log source.
		/// </remarks>
		public static LogLevel GetSourceLevel(string source) {
			ErrorCode result;
			IntPtr sourcePtr = UTF8Marshaler.Instance.MarshalManagedToNative(source);
			LogLevel level;
			result = Phidget22Imports.PhidgetLog_getSourceLevel(sourcePtr, out level);
			if (result != 0) {
				Marshal.FreeHGlobal(sourcePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(sourcePtr);
			return level;
		}

		/// <summary> </summary>
		/// <remarks>Sets the log level of the specified log source.
		/// </remarks>
		public static void SetSourceLevel(string source, LogLevel level) {
			ErrorCode result;
			IntPtr sourcePtr = UTF8Marshaler.Instance.MarshalManagedToNative(source);
			result = Phidget22Imports.PhidgetLog_setSourceLevel(sourcePtr, level);
			if (result != 0) {
				Marshal.FreeHGlobal(sourcePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(sourcePtr);
		}

		/// <summary> </summary>
		/// <remarks>Writes a message to the Phidget library log.
		/// </remarks>
		public static void WriteLine(LogLevel level, string message) {
			ErrorCode result;
			IntPtr messagePtr = UTF8Marshaler.Instance.MarshalManagedToNative(message);
			result = Phidget22Imports.PhidgetLog_logs(level, messagePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(messagePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(messagePtr);
		}

		/// <summary> </summary>
		/// <remarks>Writes a message to the Phidget library log with a specified source.
		/// </remarks>
		public static void WriteLine(LogLevel level, string source, string message) {
			ErrorCode result;
			IntPtr sourcePtr = UTF8Marshaler.Instance.MarshalManagedToNative(source);
			IntPtr messagePtr = UTF8Marshaler.Instance.MarshalManagedToNative(message);
			result = Phidget22Imports.PhidgetLog_loges(level, sourcePtr, messagePtr);
			if (result != 0) {
				Marshal.FreeHGlobal(sourcePtr);
				Marshal.FreeHGlobal(messagePtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(sourcePtr);
			Marshal.FreeHGlobal(messagePtr);
		}
		#endregion

		#region Events
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_disable();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_enable(LogLevel level, IntPtr destination);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_getLevel(out LogLevel level);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_setLevel(LogLevel level);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_rotate();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_isRotating(out bool isrotating);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_getRotating(out long size, out int keepCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_setRotating(long size, int keepCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_enableRotating();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_disableRotating();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_addSource(IntPtr source, LogLevel level);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_getSourceLevel(IntPtr source, out LogLevel level);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_setSourceLevel(IntPtr source, LogLevel level);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_logs(LogLevel level, IntPtr message);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetLog_loges(LogLevel level, IntPtr source, IntPtr message);
	}
}

namespace Phidget22.Events {
}
