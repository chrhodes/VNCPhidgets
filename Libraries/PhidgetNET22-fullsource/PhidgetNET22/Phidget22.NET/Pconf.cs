using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Phidget22;
using System.IO;
using System.Collections;

namespace Phidget22 {
	/// <summary> Phidget Config file interface </summary>
	public class Pconf {

		internal IntPtr pc;
		/// <summary> Create new Pconf object </summary>
		public Pconf(PconfType type = PconfType.PC) {
			ErrorCode result;
			this.type = type;
			result = Phidget22Imports.pconf_create(out pc);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Create new Pconf object from a file </summary>
		public Pconf(FileInfo file, PconfType type = PconfType.PC) {
			ErrorCode result;
			this.type = type;
			result = Phidget22Imports.pconf_create(out pc);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			this.fileInt = file;
		}
		/// <summary> Pconf file format </summary>
		public enum PconfType {
			/// <summary> Json </summary>
			JSON,
			/// <summary> Pconf </summary>
			PC
		};

		internal Boolean fileLocked = false;
		internal FileInfo fileInt = null;
		internal PconfType type;

		/// <summary> The file </summary>
		public FileInfo File {
			get {
				return fileInt;
			}
		}
		/// <summary> Whether the file is write locked </summary>
		public Boolean Locked {
			get {
				return fileLocked;
			}
		}
		/// <summary>Loads pconf from a string </summary>
		public void Load(string str, PconfType type = PconfType.PC) {
			ErrorCode result;
			this.type = type;
			fileInt = null;
			fileLocked = false;
			IntPtr strPtr = UTF8Marshaler.Instance.MarshalManagedToNative(str);
			if (type == PconfType.JSON) {
				result = Phidget22Imports.pconf_parsejson(out pc, strPtr, new UIntPtr((uint)str.Length));
				Marshal.FreeHGlobal(strPtr);
			} else if (type == PconfType.PC) {
				IntPtr errPtr = Marshal.AllocHGlobal(1024);
				result = Phidget22Imports.pconf_parsepcs(out pc, errPtr, new UIntPtr(1024), "%s", strPtr);
				Marshal.FreeHGlobal(strPtr);
				if (result != 0) {
					string err = UTF8Marshaler.Instance.MarshalNativeToManaged(errPtr);
					Marshal.FreeHGlobal(errPtr);
					throw new Exception(err);
				}
				Marshal.FreeHGlobal(errPtr);
			} else {
				result = ErrorCode.Unexpected;
			}
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>Loads pconf from a file </summary>
		public void Load(FileInfo file, PconfType type = PconfType.PC, Boolean lockFile = false) {
			ErrorCode result;
			this.type = type;
			this.fileInt = file;
			fileLocked = false;
			if (type == PconfType.JSON) {
				throw PhidgetException.CreateByCode(ErrorCode.Unsupported);
			} else if (type == PconfType.PC) {
				IntPtr errPtr = Marshal.AllocHGlobal(1024);
				IntPtr filePtr = UTF8Marshaler.Instance.MarshalManagedToNative(file.FullName);
				if (lockFile) {
					result = Phidget22Imports.pconf_parsepc_locked(out pc, errPtr, new UIntPtr(1024), "%s", filePtr);
					if (result == ErrorCode.Success)
						fileLocked = true;
				} else {
					result = Phidget22Imports.pconf_parsepc(out pc, errPtr, new UIntPtr(1024), "%s", filePtr);
				}
				Marshal.FreeHGlobal(filePtr);
				if (result != 0) {
					string err = UTF8Marshaler.Instance.MarshalNativeToManaged(errPtr);
					Marshal.FreeHGlobal(errPtr);
					throw new Exception(err);
				}
				Marshal.FreeHGlobal(errPtr);
			} else {
				result = ErrorCode.Unexpected;
			}
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Reload a pconf from file </summary>
		public void Reload() {
			if (fileLocked)
				return; // No need
			if (fileInt == null)
				throw PhidgetException.CreateByCode(ErrorCode.Unexpected);
			Load(fileInt, type, false);
		}
		/// <summary> Load a pconf from file </summary>
		public void Load() {
			if (fileInt == null)
				throw PhidgetException.CreateByCode(ErrorCode.Unexpected);
			Load(fileInt, type, false);
		}
		/// <summary> Unload /release pconf resources </summary>
		public void Unload() {
			if (IntPtr.Zero == pc)
				return;
			if (fileLocked) {
				ErrorCode result;
				result = Phidget22Imports.pconf_unlock_locked(ref pc);
				if (result == ErrorCode.Success)
					fileLocked = false;
			} else {
				Phidget22Imports.pconf_release(ref pc);
			}
			fileInt = null;
		}
		/// <summary> finalizer </summary>
		~Pconf() {
			Unload();
		}
		/// <summary> Render pconf as Json </summary>
		public string RenderJSON() {

			ErrorCode result;
			IntPtr jsonPtr = Marshal.AllocHGlobal(65536);
			UIntPtr jsonsz = new UIntPtr(65536);
			result = Phidget22Imports.pconf_renderjson(pc, jsonPtr, jsonsz);
			if (result != 0) {
				Marshal.FreeHGlobal(jsonPtr);
				throw PhidgetException.CreateByCode(result);
			}
			String json = UTF8Marshaler.Instance.MarshalNativeToManaged(jsonPtr);
			Marshal.FreeHGlobal(jsonPtr);
			return json;
		}
		/// <summary> Render pconf as Pconf </summary>
		public string RenderPC() {

			ErrorCode result;
			IntPtr pcPtr = Marshal.AllocHGlobal(65536);
			UIntPtr pcsz = new UIntPtr(65536);
			result = Phidget22Imports.pconf_renderpc(pc, pcPtr, pcsz);
			if (result != 0) {
				Marshal.FreeHGlobal(pcPtr);
				throw PhidgetException.CreateByCode(result);
			}
			String pcStr = UTF8Marshaler.Instance.MarshalNativeToManaged(pcPtr);
			Marshal.FreeHGlobal(pcPtr);
			return pcStr;
		}
		// NOTE: static so that I can null the PConf object after writing it.
		/// <summary> Render locked pconf back out to file </summary>
		public static void RenderPCLocked(ref Pconf conf) {

			ErrorCode result;
			if (!conf.fileLocked)
				throw PhidgetException.CreateByCode(ErrorCode.InvalidArgument);
			result = Phidget22Imports.pconf_renderpc_locked(ref conf.pc);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			conf.fileLocked = false;
			conf = null;
		}
		/// <summary> Unlock pconf loaded from file </summary>
		public static void Unlock(ref Pconf conf) {
			ErrorCode result;
			if (conf.fileLocked) {
				result = Phidget22Imports.pconf_unlock_locked(ref conf.pc);
				if (result == ErrorCode.Success)
					conf.fileLocked = false;
			} else {
				result = ErrorCode.InvalidArgument;
			}

			if (result != 0)
				throw PhidgetException.CreateByCode(result);

			conf = null;
		}
		/// <summary> Remove entry </summary>
		public void Remove(string path) {
			ErrorCode result;
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_remove(pc, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> get entry name </summary>
		public string GetEntryName(string path, int offset) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			string name = UTF8Marshaler.Instance.MarshalNativeToManaged(Phidget22Imports.pconf_getentryname(pc, offset, "%s", pathPtr));
			Marshal.FreeHGlobal(pathPtr);
			return name;
		}
		/// <summary> Get number of entries at path </summary>
		public int GetCount(string path) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			int count = Phidget22Imports.pconf_getcount(pc, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return count;
		}
		/// <summary> Add new block </summary>
		public void AddBlock(string path) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addblock(pc, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Add array </summary>
		public void AddArray(string path) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addarray(pc, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Add entry </summary>
		public void Add(string path, string val, bool detectType = false) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr valPtr = UTF8Marshaler.Instance.MarshalManagedToNative(val);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			if (detectType)
				result = Phidget22Imports.pconf_set(pc, valPtr, "%s", pathPtr);
			else
				result = Phidget22Imports.pconf_addstr(pc, valPtr, "%s", pathPtr);
			Marshal.FreeHGlobal(valPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Add entry </summary>
		public void Add(string path, double val) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addnum(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Add entry </summary>
		public void Add(string path, bool val) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addbool(pc, (val ? 1 : 0), "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Add entry </summary>
		public void Add(string path, int val) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addi(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#if false // Not CLS Compliant
		/// <summary> Add entry </summary>
		public void Add(string path, uint val) {
			ErrorCode result;
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			result = Phidget22Imports.pconf_addu(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#endif
		/// <summary> Add entry </summary>
		public void Add(string path, Int64 val) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addi(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#if false // Not CLS Compliant
		/// <summary> Add entry </summary>
		public void Add(string path, UInt64 val) {
			ErrorCode result;
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addu(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#endif

		/// <summary> Set entry </summary>
		public void Set(string path, string val, bool detectType = false) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr valPtr = UTF8Marshaler.Instance.MarshalManagedToNative(val);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			if (detectType)
				result = Phidget22Imports.pconf_set(pc, valPtr, "%s", pathPtr);
			else
				result = Phidget22Imports.pconf_addstr(pc, valPtr, "%s", pathPtr);
			Marshal.FreeHGlobal(valPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Set entry </summary>
		public void Set(string path, double val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addnum(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Set entry </summary>
		public void Set(string path, bool val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addbool(pc, (val ? 1 : 0), "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary> Set entry </summary>
		public void Set(string path, int val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addi(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#if false // Not CLS Compliant
		/// <summary> Set entry </summary>
		public void Set(string path, uint val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addu(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#endif
		/// <summary> Set entry </summary>
		public void Set(string path, Int64 val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addi(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#if false // Not CLS Compliant
		/// <summary> Set entry </summary>
		public void Set(string path, UInt64 val) {
			ErrorCode result;
			if (Exists(path))
				Remove(path);
			Phidget22Imports.pconf_setcreatemissing(pc, 1);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_addu(pc, val, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Phidget22Imports.pconf_setcreatemissing(pc, 0);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
#endif

		/// <summary> Get entry </summary>
		public string Get(string path) {
			ErrorCode result;
			IntPtr bufPtr = Marshal.AllocHGlobal(65536);
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			result = Phidget22Imports.pconf_tostring(pc, bufPtr, new UIntPtr(65536), "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(bufPtr);
				throw PhidgetException.CreateByCode(result);
			}
			String buf = UTF8Marshaler.Instance.MarshalNativeToManaged(bufPtr);
			Marshal.FreeHGlobal(bufPtr);
			return buf;
		}
		/// <summary> Get entry </summary>
		public int Get(string path, int def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			int val = Phidget22Imports.pconf_get32(pc, def, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
#if false // Not CLS Compliant
		/// <summary> Get entry </summary>
		public uint Get(string path, uint def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			uint val = Phidget22Imports.pconf_getu32(pc, def, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
#endif
		/// <summary> Get entry </summary>
		public Int64 Get(string path, Int64 def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			long val = Phidget22Imports.pconf_get64(pc, def, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
#if false // Not CLS Compliant
		/// <summary> Get entry </summary>
		public UInt64 Get(string path, UInt64 def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			ulong val = Phidget22Imports.pconf_getu64(pc, def, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
#endif
		/// <summary> Get entry </summary>
		public string Get(string path, string def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			IntPtr defPtr = UTF8Marshaler.Instance.MarshalManagedToNative(def);
			IntPtr res = Phidget22Imports.pconf_getstr(pc, defPtr, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			Marshal.FreeHGlobal(defPtr);
			return UTF8Marshaler.Instance.MarshalNativeToManaged(res);
		}
		/// <summary> Get entry </summary>
		public bool Get(string path, bool def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			bool val = (Phidget22Imports.pconf_getbool(pc, (def ? 1 : 0), "%s", pathPtr) == 0 ? false : true);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
		/// <summary> Get entry </summary>
		public double Get(string path, double def) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			double val = Phidget22Imports.pconf_getdbl(pc, def, "%s", pathPtr);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}

		/// <summary> Get entry exists </summary>
		public bool Exists(string path) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			bool val = (Phidget22Imports.pconf_exists(pc, "%s", pathPtr) == 0 ? false : true);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
		/// <summary> Get path is block </summary>
		public bool IsBlock(string path) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			bool val = (Phidget22Imports.pconf_isblock(pc, "%s", pathPtr) == 0 ? false : true);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
		/// <summary> Get path is array </summary>
		public bool IsArray(string path) {
			IntPtr pathPtr = UTF8Marshaler.Instance.MarshalManagedToNative(path);
			bool val = (Phidget22Imports.pconf_isarray(pc, "%s", pathPtr) == 0 ? false : true);
			Marshal.FreeHGlobal(pathPtr);
			return val;
		}
	}
	internal partial class Phidget22Imports {

		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_create(out IntPtr conf);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_parsejson(out IntPtr conf, IntPtr json, UIntPtr jsonsz);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_renderjson(IntPtr conf, IntPtr json, UIntPtr jsonsz);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_renderpc(IntPtr conf, IntPtr pc, UIntPtr pcsz);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_release(ref IntPtr conf);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_merge(IntPtr dest_conf, ref IntPtr source_conf, IntPtr name, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_setcreatemissing(IntPtr conf, int enable);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_remove(IntPtr conf, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern IntPtr pconf_getentryname(IntPtr conf, int off, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern Int32 pconf_getcount(IntPtr conf, string fmt, IntPtr path);

		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addblock(IntPtr conf, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addarray(IntPtr conf, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addstr(IntPtr conf, IntPtr val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addnum(IntPtr conf, double val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addi(IntPtr conf, Int64 val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addu(IntPtr conf, UInt64 val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_addbool(IntPtr conf, int val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_add(IntPtr conf, IntPtr val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_update(IntPtr conf, IntPtr val, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_set(IntPtr conf, IntPtr val, string fmt, IntPtr path);


		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_tostring(IntPtr conf, IntPtr buf, UIntPtr bufsz, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern Int32 pconf_get32(IntPtr conf, Int32 def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern UInt32 pconf_getu32(IntPtr conf, UInt32 def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern Int64 pconf_get64(IntPtr conf, Int64 def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern UInt64 pconf_getu64(IntPtr conf, UInt64 def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern Double pconf_getdbl(IntPtr conf, double def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern IntPtr pconf_getstr(IntPtr conf, IntPtr def, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern int pconf_getbool(IntPtr conf, int def, string fmt, IntPtr path);

		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern int pconf_exists(IntPtr conf, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern int pconf_isblock(IntPtr conf, string fmt, IntPtr path);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern int pconf_isarray(IntPtr conf, string fmt, IntPtr path);

		[DllImport("phidget22extra.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_parsepcs(out IntPtr conf, IntPtr err, UIntPtr errsz, string fmt, IntPtr str);
		[DllImport("phidget22extra.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_parsepc(out IntPtr conf, IntPtr err, UIntPtr errsz, string fmt, IntPtr file);

		[DllImport("phidget22extra.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_parsepc_locked(out IntPtr conf, IntPtr err, UIntPtr errsz, string fmt, IntPtr file);
		[DllImport("phidget22extra.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_renderpc_locked(ref IntPtr conf);
		[DllImport("phidget22extra.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode pconf_unlock_locked(ref IntPtr conf);
	}
}