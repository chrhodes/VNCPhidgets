using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Diagnostics;
#if DOTNET_FRAMEWORK
using System.Security.Permissions;
#endif

namespace Phidget22 {
	internal partial class Phidget22Imports {

		private const string DllName = "phidget22";
		/// <summary>
		/// Static constructor
		/// </summary>
#if DOTNET_FRAMEWORK
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
#endif
		static Phidget22Imports() {

			if (IsWindows())
				WindowsLibraryLoader.Instance.LoadLibrary(DllName);
		}

		/// <summary>
		/// Returns whether the OS is Windows or not
		/// </summary>
		/// <returns></returns>
		public static bool IsWindows() {

			return !IsUnix();
		}

		/// <summary>
		/// Returns whether the OS is *nix or not
		/// </summary>
		/// <returns></returns>
		public static bool IsUnix() {
#if DOTNET_FRAMEWORK
		var p = Environment.OSVersion.Platform;
		return (p == PlatformID.Unix ||
			p == PlatformID.MacOSX ||
			(int)p == 128);
#else
			return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
				RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
		}

		public delegate void AsyncCallbackEvent(IntPtr phid, IntPtr obj, ErrorCode returnCode);
	}

	/// <summary>
	/// Handles loading embedded dlls into memory.
	/// </summary>
	/// <remarks>This code is based on https://github.com/charlesw/tesseract - taken from OpenCvSharp</remarks>
	internal sealed class WindowsLibraryLoader {
		#region Singleton pattern

		/// <summary>
		///
		/// </summary>
		public static WindowsLibraryLoader Instance { get; } = new WindowsLibraryLoader();

		#endregion

		/// <summary>
		/// The default base directory name to copy the assemblies too.
		/// </summary>
		private const string ProcessorArchitecture = "PROCESSOR_ARCHITECTURE";
		private const string DllFileExtension = ".dll";
		private const string DllDirectory = "dll";

		private readonly List<string> loadedAssemblies = new List<string>();

		/// <summary>
		/// Map processor
		/// </summary>
		private readonly Dictionary<string, string> processorArchitecturePlatforms =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
				{
					{"x86", "x86"},
					{"AMD64", "x64"},
					{"IA64", "Itanium"},
					{"ARM", "WinCE"}
				};

		/// <summary>
		/// Used as a sanity check for the returned processor architecture to double check the returned value.
		/// </summary>
		private readonly Dictionary<string, int> processorArchitectureAddressWidthPlatforms =
			new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
				{
					{"x86", 4},
					{"AMD64", 8},
					{"IA64", 8},
					{"ARM", 4}
				};

		private readonly object syncLock = new object();

		/// <summary>
		///
		/// </summary>
		/// <param name="dllName"></param>
		/// <returns></returns>
		public bool IsLibraryLoaded(string dllName) {

			lock (syncLock)
				return loadedAssemblies.Contains(dllName);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public bool IsCurrentPlatformSupported() {
#if DOTNET_FRAMEWORK
			return Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows;
#else
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dllName"></param>
		public void LoadLibrary(string dllName) {

			if (!IsCurrentPlatformSupported())
				return;

			try {
				lock (syncLock) {
					if (loadedAssemblies.Contains(dllName))
						return;

					var processArch = GetProcessArchitecture();
					IntPtr dllHandle;
					string baseDirectory;

					// Try loading from executing assembly domain
#if !netstandard13
#if DOTNET_FRAMEWORK
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
#else
					Assembly executingAssembly = GetType().GetTypeInfo().Assembly;
#endif
					baseDirectory = Path.GetDirectoryName(executingAssembly.Location);
					dllHandle = LoadLibraryInternal(dllName, baseDirectory, processArch);
					if (dllHandle != IntPtr.Zero)
						return;
#endif

					// Fallback to current app domain
#if DOTNET_FRAMEWORK
					baseDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
					dllHandle = LoadLibraryInternal(dllName, baseDirectory, processArch);
					if (dllHandle != IntPtr.Zero)
						return;
#endif

					// Gets the pathname of the base directory that the assembly resolver uses to probe for assemblies.
					// https://github.com/dotnet/corefx/issues/2221
#if !net20 && !net40 && !net45
					baseDirectory = AppContext.BaseDirectory;
					dllHandle = LoadLibraryInternal(dllName, baseDirectory, processArch);
					if (dllHandle != IntPtr.Zero) return;
#endif

					// Finally try the working directory
					baseDirectory = Path.GetFullPath(System.IO.Directory.GetCurrentDirectory());
					dllHandle = LoadLibraryInternal(dllName, baseDirectory, processArch);
					if (dllHandle != IntPtr.Zero)
						return;

					// ASP.NET hack, requires an active context
#if false // TODO?
					if (System.Web.HttpContext.Current != null)
					{
						var server = System.Web.HttpContext.Current.Server;
						baseDirectory = Path.GetFullPath(server.MapPath("bin"));
						dllHandle = LoadLibraryInternal(dllName, baseDirectory, processArch);
						if (dllHandle != IntPtr.Zero)
							return;
					}
#endif

					// Finally, don't specify any path - try to load using standard search paths
					dllHandle = LoadLibraryRaw(dllName);
					if (dllHandle != IntPtr.Zero)
						return;

					StringBuilder errorMessage = new StringBuilder();
					errorMessage.AppendFormat("Failed to find dll \"{0}\", for processor architecture {1}.", dllName, processArch.Architecture);
					if (processArch.HasWarnings) {
						// include process detection warnings
						errorMessage.AppendFormat("\r\nWarnings: \r\n{0}", processArch.WarningText());
					}
					throw new Exception(errorMessage.ToString());
				}
			} catch (Exception e) {
				Debug.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// Get's the current process architecture while keeping track of any assumptions or possible errors.
		/// </summary>
		/// <returns></returns>
		private ProcessArchitectureInfo GetProcessArchitecture() {
			string processArchitecture = Environment.GetEnvironmentVariable(ProcessorArchitecture);
			var processInfo = new ProcessArchitectureInfo();

			if (!String.IsNullOrEmpty(processArchitecture)) {
				// Sanity check
				processInfo.Architecture = processArchitecture;
			} else {
				processInfo.AddWarning("Failed to detect processor architecture, falling back to x86.");
				processInfo.Architecture = (IntPtr.Size == 8) ? "x64" : "x86";
			}

			var addressWidth = processorArchitectureAddressWidthPlatforms[processInfo.Architecture];
			if (addressWidth != IntPtr.Size) {
				if (String.Equals(processInfo.Architecture, "AMD64", StringComparison.OrdinalIgnoreCase) && IntPtr.Size == 4) {
					// fall back to x86 if detected x64 but has an address width of 32 bits.
					processInfo.Architecture = "x86";
					processInfo.AddWarning("Expected the detected processing architecture of {0} to have an address width of {1} Bytes but was {2} Bytes, falling back to x86.", processInfo.Architecture, addressWidth, IntPtr.Size);
				} else {
					// no fallback possible
					processInfo.AddWarning("Expected the detected processing architecture of {0} to have an address width of {1} Bytes but was {2} Bytes.", processInfo.Architecture, addressWidth, IntPtr.Size);
				}
			}

			return processInfo;
		}

		private IntPtr LoadLibraryInternal(string dllName, string baseDirectory, ProcessArchitectureInfo processArchInfo) {

			var platformName = GetPlatformName(processArchInfo.Architecture);
			var expectedDllDirectory = Path.Combine(Path.Combine(baseDirectory, DllDirectory), platformName);
			return LoadLibraryRaw(dllName, expectedDllDirectory);
		}

		private IntPtr LoadLibraryRaw(string dllName, string baseDirectory = null) {
			IntPtr libraryHandle = IntPtr.Zero;
			var fileName = FixUpDllFileName((baseDirectory == null ? dllName : Path.Combine(baseDirectory, dllName)));

			// Show where we're trying to load the file from
			Debug.WriteLine(String.Format("Trying to load native library \"{0}\"...", fileName));

			if (File.Exists(fileName)) {
				// Attempt to load dll
				try {
					libraryHandle = Win32LoadLibrary(fileName);
					if (libraryHandle != IntPtr.Zero) {
						// library has been loaded
						Debug.WriteLine(String.Format("Successfully loaded native library \"{0}\".", fileName));
						loadedAssemblies.Add(dllName);
					} else {
						Debug.WriteLine(String.Format("Failed to load native library \"{0}\".\r\nCheck windows event log.", fileName));
					}
				} catch (Exception e) {
					var lastError = Marshal.GetLastWin32Error();
					Debug.WriteLine(String.Format("Failed to load native library \"{0}\".\r\nLast Error:{1}\r\nCheck inner exception and\\or windows event log.\r\nInner Exception: {2}", fileName, lastError, e));
				}
			} else {
				Debug.WriteLine(String.Format(CultureInfo.CurrentCulture, "The native library \"{0}\" does not exist.", fileName));
			}

			return libraryHandle;
		}

		/// <summary>
		/// Determines if the dynamic link library file name requires a suffix
		/// and adds it if necessary.
		/// </summary>
		private string FixUpDllFileName(string fileName) {

			if (!String.IsNullOrEmpty(fileName)) {
#if DOTNET_FRAMEWORK
				PlatformID platformId = Environment.OSVersion.Platform;
				if ((platformId == PlatformID.Win32S) ||
					(platformId == PlatformID.Win32Windows) ||
					(platformId == PlatformID.Win32NT) ||
					(platformId == PlatformID.WinCE)) {
#else
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
#endif
					if (!fileName.EndsWith(DllFileExtension, StringComparison.OrdinalIgnoreCase))
						return fileName + DllFileExtension;
				}
			}

			return fileName;
		}

		/// <summary>
		/// Given the processor architecture, returns the name of the platform.
		/// </summary>
		private string GetPlatformName(string processorArchitecture) {

			if (String.IsNullOrEmpty(processorArchitecture))
				return null;

			string platformName;
			if (processorArchitecturePlatforms.TryGetValue(processorArchitecture, out platformName))
				return platformName;

			return null;
		}

		/// <summary>
		///
		/// </summary>
		private class ProcessArchitectureInfo {
			public ProcessArchitectureInfo() {
				Warnings = new List<string>();
			}

			public string Architecture { get; set; }
			private List<string> Warnings { get; set; }

			public bool HasWarnings {
				get { return Warnings.Count > 0; }
			}

			public void AddWarning(string format, params object[] args) {
				Warnings.Add(String.Format(format, args));
			}

			public string WarningText() {
				return String.Join("\r\n", Warnings.ToArray());
			}
		}

#if DOTNET_FRAMEWORK
		private const CharSet DefaultCharSet = CharSet.Auto;
#else
		private const CharSet DefaultCharSet = CharSet.Unicode;
#endif

		[DllImport("kernel32", EntryPoint = "LoadLibrary", CallingConvention = CallingConvention.Winapi,
			SetLastError = true, CharSet = DefaultCharSet, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		private static extern IntPtr Win32LoadLibrary(string dllPath);
	}

	#region Custom Marshallers

	internal class PhidgetMarshaler {
		private static PhidgetMarshaler instance;

		private PhidgetMarshaler() { }

		public static PhidgetMarshaler Instance {
			get {
				if (instance == null) {
					instance = new PhidgetMarshaler();
				}
				return instance;
			}
		}

		public IntPtr MarshalManagedToNative(Phidget ManagedObj) {
			if (ManagedObj == null)
				return IntPtr.Zero;
			return ((Phidget)ManagedObj).chandle;
		}

		public Phidget MarshalNativeToManaged(IntPtr pNativeData) {
			if (pNativeData == IntPtr.Zero)
				return null;

			Phidget phid = new Phidget();
			phid.chandle = pNativeData;

			Phidget typedPhid = Manager.CreateTypedManagerPhid(phid);
			return typedPhid;
		}
	}
	internal class UTF8Marshaler {
		private static UTF8Marshaler instance;

		private UTF8Marshaler() { }

		public static UTF8Marshaler Instance {
			get {
				if (instance == null) {
					instance = new UTF8Marshaler();
				}
				return instance;
			}
		}

		public IntPtr MarshalManagedToNative(string ManagedObj) {
			if (ManagedObj == null)
				return IntPtr.Zero;
			byte[] array = Encoding.UTF8.GetBytes((string)ManagedObj);
			int size = Marshal.SizeOf(typeof(byte)) * (array.Length + 1);

			IntPtr ptr = Marshal.AllocHGlobal(size);

			Marshal.Copy(array, 0, ptr, array.Length);
			Marshal.WriteByte(ptr, array.Length, 0);

			return ptr;
		}

		public String MarshalNativeToManaged(IntPtr pNativeData) {
			if (pNativeData == IntPtr.Zero)
				return null;

			int size = 0;
			while (Marshal.ReadByte(pNativeData, size) > 0)
				size++;

			byte[] array = new byte[size];
			Marshal.Copy(pNativeData, array, 0, size);

			String utf8str = Encoding.UTF8.GetString(array);

			return utf8str;
		}
	}

	internal class UnitInfoMarshaler {
		private static UnitInfoMarshaler instance;

		private UnitInfoMarshaler() { }

		public static UnitInfoMarshaler Instance {
			get {
				if (instance == null) {
					instance = new UnitInfoMarshaler();
				}
				return instance;
			}
		}

		public IntPtr MarshalManagedToNative(UnitInfo ManagedObj) {

			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UnitInfo)));

			Marshal.StructureToPtr(ManagedObj, ptr, false);
			return ptr;
		}

		public UnitInfo MarshalNativeToManaged(IntPtr pNativeData) {
			UnitInfo u = new UnitInfo();

			if (pNativeData == IntPtr.Zero)
				return u;

			u.Unit = (Unit)Marshal.ReadInt32(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(UnitInfo), "Unit").ToInt32()));
			u.Name = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(UnitInfo), "Name").ToInt32())));
			u.Symbol = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(UnitInfo), "Symbol").ToInt32())));

			return u;
		}
	}

	internal class PhidgetServerMarshaler {
		private static PhidgetServerMarshaler instance;

		private PhidgetServerMarshaler() { }

		public static PhidgetServerMarshaler Instance {
			get {
				if (instance == null) {
					instance = new PhidgetServerMarshaler();
				}
				return instance;
			}
		}

		public int GetNativeDataSize() {
			return Marshal.SizeOf(typeof(PhidgetServer));
		}

		public IntPtr MarshalManagedToNative(PhidgetServer ManagedObj) {
			IntPtr ptr = Marshal.AllocHGlobal(GetNativeDataSize());

			Marshal.StructureToPtr(ManagedObj, ptr, false);
			return ptr;
		}

		public PhidgetServer MarshalNativeToManaged(IntPtr pNativeData) {
			PhidgetServer server = new PhidgetServer();

			if (pNativeData == IntPtr.Zero)
				return server;

			server.Name = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Name").ToInt32())));
			server.TypeName = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "TypeName").ToInt32())));
			server.Type = (ServerType)Marshal.ReadInt32(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Type").ToInt32()));
			server.Flags = Marshal.ReadInt32(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Flags").ToInt32()));
			server.Address = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Address").ToInt32())));
			server.Hostname = UTF8Marshaler.Instance.MarshalNativeToManaged(Marshal.ReadIntPtr(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Hostname").ToInt32())));
			server.Port = Marshal.ReadInt32(new IntPtr(pNativeData.ToInt64() + Marshal.OffsetOf(typeof(PhidgetServer), "Port").ToInt32()));

			return server;
		}
	}

	#endregion
}
