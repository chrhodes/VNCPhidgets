using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Phidget class definition </summary>
	public partial class Phidget {

		#region Constants

		/// <summary> Pass to ${PROPERTY:DeviceSerialNumber} to open any serial number. </summary>
		public const int AnySerialNumber = -1;

		/// <summary> Pass to ${PROPERTY:HubPort} to open any hub port. </summary>
		public const int AnyHubPort = -1;

		/// <summary> Pass to ${PROPERTY:Channel} to open any channel. </summary>
		public const int AnyChannel = -1;

		/// <summary> Pass to ${PROPERTY:DeviceLabel} to open any label. </summary>
		public const string AnyLabel = null;

		/// <summary> Pass to ${METHOD:openWaitForAttachment} for an infinite timeout. </summary>
		public const int InfiniteTimeout = 0;

		/// <summary> Pass to ${METHOD:openWaitForAttachment} for the default timeout. </summary>
		public const int DefaultTimeout = 500;
		#endregion

		#region Properties

		/// <summary> The Phidget library version. </summary>
		/// <remarks>Gets the version of the Phidget library being used by the program.
		/// </remarks>
		public static string LibraryVersion {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getLibraryVersion(out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> True if the channel is attached </summary>
		/// <remarks>Gets the attached status of channel. A Phidget is attached after it has been opened and the Phidget
		/// library finds and connects to the corresponding hardware device.
		/// <list>
		/// <item>Most API calls are only valid on attached Phidgets.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Attached {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.Phidget_getAttached(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The channel index </summary>
		/// <remarks>Gets the channel index of the channel on the device.
		/// </remarks>
		public int Channel {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getChannel(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setChannel(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> True if the handle is for a channel. </summary>
		/// <remarks>Returns true if the <c>PhidgetHandle</c> is for a channel. Mostly for use alongside getParent
		/// to distinguish channel handles from device handles.
		/// </remarks>
		public bool IsChannel {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.Phidget_getIsChannel(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The channel class </summary>
		/// <remarks>Gets the channel class of the channel.
		/// </remarks>
		public ChannelClass ChannelClass {
			get {
				ErrorCode result;
				ChannelClass val;
				result = Phidget22Imports.Phidget_getChannelClass(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The name of the channel's class </summary>
		/// <remarks>Gets the name of the channel class the channel belongs to.
		/// </remarks>
		public string ChannelClassName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getChannelClassName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The channel's name </summary>
		/// <remarks>Gets the channel's name. This name serves as a description of the specific nature of the channel.
		/// </remarks>
		public string ChannelName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getChannelName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The channel's subclass </summary>
		/// <remarks>Gets the subclass for this channel. Allows for identifying channels with specific characteristics
		/// without needing to know the exact device and channel index.
		/// </remarks>
		public ChannelSubclass ChannelSubclass {
			get {
				ErrorCode result;
				ChannelSubclass val;
				result = Phidget22Imports.Phidget_getChannelSubclass(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The class of the device the channel is a part of. </summary>
		/// <remarks>Gets the device class for the Phidget which this channel is a part of.
		/// </remarks>
		public DeviceClass DeviceClass {
			get {
				ErrorCode result;
				DeviceClass val;
				result = Phidget22Imports.Phidget_getDeviceClass(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The class name of the device the channel is a part of. </summary>
		/// <remarks>Gets the name of the device class for the Phidget which this channel is a part of.
		/// </remarks>
		public string DeviceClassName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getDeviceClassName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The firmware upgrade string </summary>
		/// <remarks>Gets the string which will match the filename of the firmware upgrade file
		/// </remarks>
		public string DeviceFirmwareUpgradeString {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getDeviceFirmwareUpgradeString(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The device id of the device the channel is a part of </summary>
		/// <remarks>Gets the DeviceID for the Phidget which this channel is a part of.
		/// </remarks>
		public DeviceID DeviceID {
			get {
				ErrorCode result;
				DeviceID val;
				result = Phidget22Imports.Phidget_getDeviceID(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The device label </summary>
		/// <remarks>Gets the label of the Phidget which this channel is a part of. A device label is a custom string
		/// used to more easily identify a Phidget. Labels are written to a Phidget using
		/// <c>WriteDeviceLabel()</c>, or by right-clicking the device and setting a label in the Phidget
		/// Control Panel for Windows.
		/// 
		/// See <a href='/docs/Using_Multiple_Phidgets#Using_the_Label' target='_blank'>using a label</a> for
		/// more information about how to use labels with Phidgets.
		/// </remarks>
		public string DeviceLabel {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getDeviceLabel(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
			set {
				ErrorCode result;
				IntPtr valuePtr = UTF8Marshaler.Instance.MarshalManagedToNative(value);
				result = Phidget22Imports.Phidget_setDeviceLabel(chandle, valuePtr);
				Marshal.FreeHGlobal(valuePtr);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The name of the device the channel is a part of </summary>
		/// <remarks>Gets the name of the Phidget which this channel is a part of.
		/// </remarks>
		public string DeviceName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getDeviceName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The device serial number </summary>
		/// <remarks>Gets the serial number of the Phidget which this channel is a part of.
		/// If the channel is part of a VINT device, this will be the serial number of the VINT Hub the device
		/// is attached to.
		/// </remarks>
		public int DeviceSerialNumber {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getDeviceSerialNumber(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setDeviceSerialNumber(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The SKU of the device the channel is a part of </summary>
		/// <remarks>Gets the SKU (part number) of the Phidget which this channel is a part of.
		/// </remarks>
		public string DeviceSKU {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getDeviceSKU(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The version of the device the channel is a part of </summary>
		/// <remarks>Gets the firmware version of the Phidget which this channel is a part of.
		/// </remarks>
		public int DeviceVersion {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getDeviceVersion(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The vint id of the channels device </summary>
		/// <remarks>Gets the vint id for the Phidget which this channel is a part of.
		/// </remarks>
		public int DeviceVINTID {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getDeviceVINTID(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The hub the channels device is attached to </summary>
		/// <remarks>Gets the hub that this channel is attached to.
		/// </remarks>
		public Phidget Hub {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getHub(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return PhidgetMarshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The hub port index </summary>
		/// <remarks>Gets the hub port index of the VINT Hub port that the channel is attached to.
		/// </remarks>
		public int HubPort {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getHubPort(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setHubPort(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The number of ports on the VINT Hub </summary>
		/// <remarks>Gets the number of VINT ports present on the VINT Hub that the channel is attached to.
		/// </remarks>
		public int HubPortCount {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.Phidget_getHubPortCount(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The hub port mode (True if the channel is a hub port itself) </summary>
		/// <remarks>Gets whether this channel is a VINT Hub port channel, or part of a VINT device attached to a hub
		/// port.
		/// </remarks>
		public bool IsHubPortDevice {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.Phidget_getIsHubPortDevice(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setIsHubPortDevice(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> True if the channel is attached to a local device </summary>
		/// <remarks>Returns true when this channel is attached directly on the local machine, or false otherwise.
		/// </remarks>
		public bool IsLocal {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.Phidget_getIsLocal(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setIsLocal(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The mesh mode </summary>
		/// <remarks>The mesh mode of a mesh device.
		/// </remarks>
		public MeshMode MeshMode {
			get {
				ErrorCode result;
				MeshMode val;
				result = Phidget22Imports.Phidget_getMeshMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setMeshMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The handle of the parent </summary>
		/// <remarks><p>Gets the handle of the parent device of the given Phidget handle.</p>
		/// <p>For example, this would refer to the device the channel is a part of, or the Hub that a device
		/// is plugged into.</p>
		/// This is useful when used alongside a <b>Phidget Manager</b> to create device trees like the one in
		/// the Phidget Control Panel.
		/// <list>
		/// <item>This can be used to travel up the device tree and get device information at each step.</item>
		/// <item>The root device will return a null handle</item>
		/// <item>Parent handles always refer to devices. See getIsChannel</item>
		/// </list>
		/// 
		/// </remarks>
		public Phidget Parent {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getParent(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return PhidgetMarshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> True if the channel is attached to a network device </summary>
		/// <remarks>Returns true when this channel is attached via a Phidget network server, or false otherwise.
		/// </remarks>
		public bool IsRemote {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.Phidget_getIsRemote(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.Phidget_setIsRemote(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The hostname of the channel's server </summary>
		/// <remarks>Gets the hostname of the Phidget network server for network attached Phidgets.
		/// Fails if the channel is not connected to a Phidget network server.
		/// </remarks>
		public string ServerHostname {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getServerHostname(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}

		/// <summary> The name of the Phidget network server the channel is from </summary>
		/// <remarks>Gets the name of the Phidget network server the channel is attached to, if any.
		/// Fails if the channel is not connected to a Phidget network server.
		/// </remarks>
		public string ServerName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getServerName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
			set {
				ErrorCode result;
				IntPtr valuePtr = UTF8Marshaler.Instance.MarshalManagedToNative(value);
				result = Phidget22Imports.Phidget_setServerName(chandle, valuePtr);
				Marshal.FreeHGlobal(valuePtr);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The address and port of the channel's server </summary>
		/// <remarks>Gets the peer name (address and port) of the Phidget server for network attached Phidgets,
		/// formatted as: <c>address:port</c>
		/// Fails if the channel is not connected to a Phidget network server.
		/// </remarks>
		public string ServerPeerName {
			get {
				ErrorCode result;
				IntPtr val;
				result = Phidget22Imports.Phidget_getServerPeerName(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void Calibrate(byte offset, byte[] data) {
			ErrorCode result;
			result = Phidget22Imports.Phidget_calibrate(chandle, offset, data);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void CalibrateGainOffset(int index, short offset, int gain) {
			ErrorCode result;
			result = Phidget22Imports.Phidget_calibrateGainOffset(chandle, index, offset, gain);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void CalibrateGainOffset2(int index, short offset, int gain1, int gain2) {
			ErrorCode result;
			result = Phidget22Imports.Phidget_calibrateGainOffset2(chandle, index, offset, gain1, gain2);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Closes a Phidget channel that has been opened.<c>Close()</c> will release the channel on the
		/// Phidget device, and should be called prior to delete.
		/// </remarks>
		public void Close() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_close(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Gets the number of channels of the specified channel class on the device. Pass PHIDCHCLASS_NOTHING
		/// to get the total number of channels.
		/// </remarks>
		public int GetDeviceChannelCount(ChannelClass cls) {
			ErrorCode result;
			int count;
			result = Phidget22Imports.Phidget_getDeviceChannelCount(chandle, cls, out count);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
			return count;
		}

		/// <summary> </summary>
		/// <remarks>Sets a device-specific config block on M3 Phidgets.
		/// </remarks>
		public void SetDeviceSpecificConfigTable(int index, byte[] data) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.Phidget_setDeviceSpecificConfigTable(chandle, index, data, dataLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Sets a device-wide config block on M3 Phidgets.
		/// </remarks>
		public void SetDeviceWideConfigTable(int index, byte[] data) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.Phidget_setDeviceWideConfigTable(chandle, index, data, dataLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void EnterCalibrationMode(int confirm) {
			ErrorCode result;
			result = Phidget22Imports.Phidget_enterCalibrationMode(chandle, confirm);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Erases all config data on M3 Phidgets.
		/// </remarks>
		public void EraseConfig() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_eraseConfig(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void ExitCalibrationMode() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_exitCalibrationMode(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Reboots the device into the regular firmware.
		/// </remarks>
		public void Reboot() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_reboot(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Reboots the device into firmware upgrade mode.
		/// </remarks>
		public void RebootFirmwareUpgrade(int upgradeTimeout) {
			ErrorCode result;
			result = Phidget22Imports.Phidget_rebootFirmwareUpgrade(chandle, upgradeTimeout);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Reboots the device into ISP (in circuit programming) mode - M3 based devices only.
		/// </remarks>
		public void RebootISP() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_rebootISP(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>TODO
		/// </remarks>
		public void WriteCalibrationData(int offset, byte[] data) {
			ErrorCode result;
			IntPtr dataLen = new IntPtr(data.Length);
			result = Phidget22Imports.Phidget_writeCalibrationData(chandle, offset, data, dataLen);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Writes a label to the device in the form of a string in the device flash memory. This label can
		/// then be used to identify the device, and will persist across power cycles.
		/// 
		/// The label can be at most 10 UTF-16 code units. Most unicode characters take up a single code unit,
		/// but some, such as emoji, can take several.
		/// 
		/// Some devices can not have their labels set from Windows. For these devices the label should be set
		/// from Linux or macOS.
		/// 
		/// See <a href='/docs/Using_Multiple_Phidgets#Using_the_Label' target='_blank'>using a label</a> for
		/// more information about how to use labels with Phidgets.
		/// </remarks>
		public void WriteDeviceLabel(string deviceLabel) {
			ErrorCode result;
			IntPtr deviceLabelPtr = UTF8Marshaler.Instance.MarshalManagedToNative(deviceLabel);
			result = Phidget22Imports.Phidget_writeDeviceLabel(chandle, deviceLabelPtr);
			if (result != 0) {
				Marshal.FreeHGlobal(deviceLabelPtr);
				throw PhidgetException.CreateByCode(result);
			}
			Marshal.FreeHGlobal(deviceLabelPtr);
		}

		/// <summary> </summary>
		/// <remarks>Write all config data to flash on M3 Phidgets.
		/// </remarks>
		public void WriteFlash() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_writeFlash(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> </summary>
		/// <remarks>Zeroes out all config data in memory without erasing the flash on M3 Phidgets.
		/// </remarks>
		public void ZeroConfig() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_zeroConfig(chandle);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal virtual void initializeEvents() { initializeBaseEvents(); }
		internal void initializeBaseEvents() {
			ErrorCode result;
			nativeAttachEventCallback = new Phidget22Imports.AttachEvent(nativeAttachEvent);
			result = Phidget22Imports.Phidget_setOnAttachHandler(chandle, nativeAttachEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeCalibrationDataEventCallback = new Phidget22Imports.CalibrationDataEvent(nativeCalibrationDataEvent);
			result = Phidget22Imports.Phidget_setOnCalibrationDataHandler(chandle, nativeCalibrationDataEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeDetachEventCallback = new Phidget22Imports.DetachEvent(nativeDetachEvent);
			result = Phidget22Imports.Phidget_setOnDetachHandler(chandle, nativeDetachEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeErrorEventCallback = new Phidget22Imports.ErrorEvent(nativeErrorEvent);
			result = Phidget22Imports.Phidget_setOnErrorHandler(chandle, nativeErrorEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativePropertyChangeEventCallback = new Phidget22Imports.PropertyChangeEvent(nativePropertyChangeEvent);
			result = Phidget22Imports.Phidget_setOnPropertyChangeHandler(chandle, nativePropertyChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal virtual void uninitializeEvents() { uninitializeBaseEvents(); }
		internal void uninitializeBaseEvents() {
			ErrorCode result;
			result = Phidget22Imports.Phidget_setOnAttachHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.Phidget_setOnCalibrationDataHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.Phidget_setOnDetachHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.Phidget_setOnErrorHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.Phidget_setOnPropertyChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the channel is attached to a physical channel on a Phidget.
		/// 
		/// <c>Attach</c> must be registered prior to calling <c>Open()</c>, and will be called
		/// when the Phidget library matches the channel with a physical channel on a Phidget.
		/// <c>Attach</c> may be called more than once if the channel is detached during its
		/// lifetime.
		/// 
		/// <c>Attach</c> is the recommended place to configuration properties of the channel such as the
		/// data interval or change trigger.
		/// </remarks>
		public event AttachEventHandler Attach;
		internal void OnAttach(AttachEventArgs e) {
			if (Attach != null) {
				foreach (AttachEventHandler AttachHandler in Attach.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = AttachHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(AttachHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						AttachHandler(this, e);
				}
			}
		}
		Phidget22Imports.AttachEvent nativeAttachEventCallback;
		internal void nativeAttachEvent(IntPtr phid, IntPtr ctx) {
			OnAttach(new AttachEventArgs());
		}
		/// <summary>  </summary>
		/// <remarks>TODO
		/// </remarks>
		public event CalibrationDataEventHandler CalibrationData;
		internal void OnCalibrationData(CalibrationDataEventArgs e) {
			if (CalibrationData != null) {
				foreach (CalibrationDataEventHandler CalibrationDataHandler in CalibrationData.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = CalibrationDataHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(CalibrationDataHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						CalibrationDataHandler(this, e);
				}
			}
		}
		Phidget22Imports.CalibrationDataEvent nativeCalibrationDataEventCallback;
		internal void nativeCalibrationDataEvent(IntPtr phid, IntPtr ctx, byte[] data, IntPtr dataLen) {
			OnCalibrationData(new CalibrationDataEventArgs(data));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the channel is detached from a Phidget device channel.<c>Detach</c> typically
		/// occurs when the Phidget device is removed from the system.
		/// </remarks>
		public event DetachEventHandler Detach;
		internal void OnDetach(DetachEventArgs e) {
			if (Detach != null) {
				foreach (DetachEventHandler DetachHandler in Detach.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = DetachHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(DetachHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						DetachHandler(this, e);
				}
			}
		}
		Phidget22Imports.DetachEvent nativeDetachEventCallback;
		internal void nativeDetachEvent(IntPtr phid, IntPtr ctx) {
			OnDetach(new DetachEventArgs());
		}
		/// <summary>  </summary>
		/// <remarks><c>Error</c> is called when an error condition has been detected.
		/// 
		/// See the documentation for your specific channel class to see what error events it might throw.
		/// </remarks>
		public event ErrorEventHandler Error;
		internal void OnError(ErrorEventArgs e) {
			if (Error != null) {
				foreach (ErrorEventHandler ErrorHandler in Error.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = ErrorHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(ErrorHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						ErrorHandler(this, e);
				}
			}
		}
		Phidget22Imports.ErrorEvent nativeErrorEventCallback;
		internal void nativeErrorEvent(IntPtr phid, IntPtr ctx, ErrorEventCode code, IntPtr description) {
			OnError(new ErrorEventArgs(code, UTF8Marshaler.Instance.MarshalNativeToManaged(description)));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when a property is changed externally from the user channel, usually from a network client
		/// attached to the same channel.
		/// </remarks>
		public event PropertyChangeEventHandler PropertyChange;
		internal void OnPropertyChange(PropertyChangeEventArgs e) {
			if (PropertyChange != null) {
				foreach (PropertyChangeEventHandler PropertyChangeHandler in PropertyChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PropertyChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PropertyChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PropertyChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.PropertyChangeEvent nativePropertyChangeEventCallback;
		internal void nativePropertyChangeEvent(IntPtr phid, IntPtr ctx, IntPtr propertyName) {
			OnPropertyChange(new PropertyChangeEventArgs(UTF8Marshaler.Instance.MarshalNativeToManaged(propertyName)));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getErrorDescription(ErrorCode errorCode, out IntPtr errorString);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_finalize(int flags);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_resetLibrary();
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_calibrate(IntPtr phid, byte offset, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] byte[] data);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_calibrateGainOffset(IntPtr phid, int index, short offset, int gain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_calibrateGainOffset2(IntPtr phid, int index, short offset, int gain1, int gain2);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_close(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceChannelCount(IntPtr phid, ChannelClass cls, out int count);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setDeviceSpecificConfigTable(IntPtr phid, int index, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setDeviceWideConfigTable(IntPtr phid, int index, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_enterCalibrationMode(IntPtr phid, int confirm);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_eraseConfig(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_exitCalibrationMode(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_open(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_openWaitForAttachment(IntPtr phid, int timeout);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_reboot(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_rebootFirmwareUpgrade(IntPtr phid, int upgradeTimeout);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_rebootISP(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_writeCalibrationData(IntPtr phid, int offset, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_writeDeviceLabel(IntPtr phid, IntPtr deviceLabel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_writeFlash(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_zeroConfig(IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getLibraryVersion(out IntPtr LibraryVersion);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getAttached(IntPtr phid, out bool Attached);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getChannel(IntPtr phid, out int Channel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setChannel(IntPtr phid, int Channel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getIsChannel(IntPtr phid, out bool IsChannel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getChannelClass(IntPtr phid, out ChannelClass ChannelClass);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getChannelClassName(IntPtr phid, out IntPtr ChannelClassName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getChannelName(IntPtr phid, out IntPtr ChannelName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getChannelSubclass(IntPtr phid, out ChannelSubclass ChannelSubclass);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceClass(IntPtr phid, out DeviceClass DeviceClass);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceClassName(IntPtr phid, out IntPtr DeviceClassName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceFirmwareUpgradeString(IntPtr phid, out IntPtr DeviceFirmwareUpgradeString);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceID(IntPtr phid, out DeviceID DeviceID);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceLabel(IntPtr phid, out IntPtr DeviceLabel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setDeviceLabel(IntPtr phid, IntPtr DeviceLabel);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceName(IntPtr phid, out IntPtr DeviceName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceSerialNumber(IntPtr phid, out int DeviceSerialNumber);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setDeviceSerialNumber(IntPtr phid, int DeviceSerialNumber);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceSKU(IntPtr phid, out IntPtr DeviceSKU);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceVersion(IntPtr phid, out int DeviceVersion);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getDeviceVINTID(IntPtr phid, out int DeviceVINTID);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getHub(IntPtr phid, out IntPtr Hub);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getHubPort(IntPtr phid, out int HubPort);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setHubPort(IntPtr phid, int HubPort);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getHubPortCount(IntPtr phid, out int HubPortCount);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getIsHubPortDevice(IntPtr phid, out bool IsHubPortDevice);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setIsHubPortDevice(IntPtr phid, bool IsHubPortDevice);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getIsLocal(IntPtr phid, out bool IsLocal);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setIsLocal(IntPtr phid, bool IsLocal);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getMeshMode(IntPtr phid, out MeshMode MeshMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setMeshMode(IntPtr phid, MeshMode MeshMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getParent(IntPtr phid, out IntPtr Parent);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getIsRemote(IntPtr phid, out bool IsRemote);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setIsRemote(IntPtr phid, bool IsRemote);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getServerHostname(IntPtr phid, out IntPtr ServerHostname);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getServerName(IntPtr phid, out IntPtr ServerName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setServerName(IntPtr phid, IntPtr ServerName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_getServerPeerName(IntPtr phid, out IntPtr ServerPeerName);
		public delegate void AttachEvent(IntPtr phid, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setOnAttachHandler(IntPtr phid, AttachEvent fptr, IntPtr ctx);
		public delegate void CalibrationDataEvent(IntPtr phid, IntPtr ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data, IntPtr dataLen);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setOnCalibrationDataHandler(IntPtr phid, CalibrationDataEvent fptr, IntPtr ctx);
		public delegate void DetachEvent(IntPtr phid, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setOnDetachHandler(IntPtr phid, DetachEvent fptr, IntPtr ctx);
		public delegate void ErrorEvent(IntPtr phid, IntPtr ctx, ErrorEventCode Code, IntPtr Description);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setOnErrorHandler(IntPtr phid, ErrorEvent fptr, IntPtr ctx);
		public delegate void PropertyChangeEvent(IntPtr phid, IntPtr ctx, IntPtr propertyName);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode Phidget_setOnPropertyChangeHandler(IntPtr phid, PropertyChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Phidget Attach Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A AttachEventArg object contains data and information related to the Event.</param>
	public delegate void AttachEventHandler(object sender, AttachEventArgs e);
	/// <summary> Phidget Attach Event data </summary>
	public class AttachEventArgs : EventArgs {
		internal AttachEventArgs() {
		}
	}

	/// <summary> A Phidget CalibrationData Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A CalibrationDataEventArg object contains data and information related to the Event.</param>
	public delegate void CalibrationDataEventHandler(object sender, CalibrationDataEventArgs e);
	/// <summary> Phidget CalibrationData Event data </summary>
	public class CalibrationDataEventArgs : EventArgs {
		/// <summary>TODO: Text Here
		/// </summary>
		public readonly byte[] Data;
		internal CalibrationDataEventArgs(byte[] data) {
			this.Data = data;
		}
	}

	/// <summary> A Phidget Detach Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DetachEventArg object contains data and information related to the Event.</param>
	public delegate void DetachEventHandler(object sender, DetachEventArgs e);
	/// <summary> Phidget Detach Event data </summary>
	public class DetachEventArgs : EventArgs {
		internal DetachEventArgs() {
		}
	}

	/// <summary> A Phidget Error Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A ErrorEventArg object contains data and information related to the Event.</param>
	public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
	/// <summary> Phidget Error Event data </summary>
	public class ErrorEventArgs : EventArgs {
		/// <summary>The error code
		/// </summary>
		public readonly ErrorEventCode Code;
		/// <summary>The error description
		/// </summary>
		public readonly string Description;
		internal ErrorEventArgs(ErrorEventCode code, string description) {
			this.Code = code;
			this.Description = description;
		}
	}

	/// <summary> A Phidget PropertyChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A PropertyChangeEventArg object contains data and information related to the Event.</param>
	public delegate void PropertyChangeEventHandler(object sender, PropertyChangeEventArgs e);
	/// <summary> Phidget PropertyChange Event data </summary>
	public class PropertyChangeEventArgs : EventArgs {
		/// <summary>The name of the property that has changed
		/// </summary>
		public readonly string PropertyName;
		internal PropertyChangeEventArgs(string propertyName) {
			this.PropertyName = propertyName;
		}
	}

}
