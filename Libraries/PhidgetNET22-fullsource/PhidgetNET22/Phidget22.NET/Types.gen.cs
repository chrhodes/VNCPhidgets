using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary>Encoder interface mode
	/// </summary>
	public enum EncoderIOMode : int {
		/// <summary> No additional pull-up or pull-down resistors will be applied to the input lines. </summary>
		[Description("Push-Pull")]
		PushPull = 1,
		/// <summary> 2.2kΩ pull-down resistors will be applied to the input lines. </summary>
		[Description("Line Driver 2.2K")]
		LineDriver_2K2 = 2,
		/// <summary> 10kΩ pull-down resistors will be applied to the input lines. </summary>
		[Description("Line Driver 10K")]
		LineDriver_10K = 3,
		/// <summary> 2.2kΩ pull-up resistors will be applied to the input lines. </summary>
		[Description("Open Collector 2.2K")]
		OpenCollector_2K2 = 4,
		/// <summary> 10kΩ pull-up resistors will be applied to the input lines. </summary>
		[Description("Open Collector 10K")]
		OpenCollector_10K = 5,
	}
	/// <summary>Error codes returned from all API calls via Exceptions.
	/// </summary>
	public enum ErrorCode : int {
		/// <summary> Success </summary>
		[Description("Success")]
		Success = 0,
		/// <summary> Not Permitted </summary>
		[Description("Not Permitted")]
		NotPermitted = 1,
		/// <summary> No Such Entity </summary>
		[Description("No Such Entity")]
		NoSuchEntity = 2,
		/// <summary> Timed Out </summary>
		[Description("Timed Out")]
		Timeout = 3,
		/// <summary> Keep Alive Failure </summary>
		[Description("Keep Alive Failure")]
		KeepAlive = 58,
		/// <summary> Op Interrupted </summary>
		[Description("Op Interrupted")]
		Interrupted = 4,
		/// <summary> IO Issue </summary>
		[Description("IO Issue")]
		IO = 5,
		/// <summary> Memory Issue </summary>
		[Description("Memory Issue")]
		NoMemory = 6,
		/// <summary> Access (Permission) Issue </summary>
		[Description("Access (Permission) Issue")]
		Access = 7,
		/// <summary> Address Issue </summary>
		[Description("Address Issue")]
		Fault = 8,
		/// <summary> Resource Busy </summary>
		[Description("Resource Busy")]
		Busy = 9,
		/// <summary> Object Exists </summary>
		[Description("Object Exists")]
		Exists = 10,
		/// <summary> Object is not a directory </summary>
		[Description("Object is not a directory")]
		IsNotDirectory = 11,
		/// <summary> Object is a directory </summary>
		[Description("Object is a directory")]
		IsDirectory = 12,
		/// <summary> Invalid </summary>
		[Description("Invalid")]
		Invalid = 13,
		/// <summary> Too many open files in system </summary>
		[Description("Too many open files in system")]
		TooManyFilesSystem = 14,
		/// <summary> Too many open files </summary>
		[Description("Too many open files")]
		TooManyFiles = 15,
		/// <summary> Not enough space </summary>
		[Description("Not enough space")]
		NoSpace = 16,
		/// <summary> File too Big </summary>
		[Description("File too Big")]
		FileTooBig = 17,
		/// <summary> Read Only Filesystem </summary>
		[Description("Read Only Filesystem")]
		ReadOnlyFilesystem = 18,
		/// <summary> Read Only Object </summary>
		[Description("Read Only Object")]
		ReadOnly = 19,
		/// <summary> Operation Not Supported </summary>
		[Description("Operation Not Supported")]
		Unsupported = 20,
		/// <summary> Invalid Argument </summary>
		[Description("Invalid Argument")]
		InvalidArgument = 21,
		/// <summary> Try again </summary>
		[Description("Try again")]
		TryAgain = 22,
		/// <summary> No Empty </summary>
		[Description("No Empty")]
		NotEmpty = 26,
		/// <summary> Unexpected Error </summary>
		[Description("Unexpected Error")]
		Unexpected = 28,
		/// <summary> Duplicate </summary>
		[Description("Duplicate")]
		Duplicate = 27,
		/// <summary> Bad Credential </summary>
		[Description("Bad Credential")]
		BadPassword = 37,
		/// <summary> Network Unavailable </summary>
		[Description("Network Unavailable")]
		NetworkUnavailable = 45,
		/// <summary> Connection Refused </summary>
		[Description("Connection Refused")]
		ConnectionRefused = 35,
		/// <summary> Connection Reset </summary>
		[Description("Connection Reset")]
		ConnectionReset = 46,
		/// <summary> No route to host </summary>
		[Description("No route to host")]
		HostUnreachable = 48,
		/// <summary> No Such Device </summary>
		[Description("No Such Device")]
		NoSuchDevice = 40,
		/// <summary> Wrong Device </summary>
		[Description("Wrong Device")]
		WrongDevice = 50,
		/// <summary> Broken Pipe </summary>
		[Description("Broken Pipe")]
		BrokenPipe = 41,
		/// <summary> Name Resolution Failure </summary>
		[Description("Name Resolution Failure")]
		NameResolutionFailure = 44,
		/// <summary> Unknown or Invalid Value </summary>
		[Description("Unknown or Invalid Value")]
		UnknownValue = 51,
		/// <summary> Device not Attached </summary>
		[Description("Device not Attached")]
		NotAttached = 52,
		/// <summary> Invalid or Unexpected Packet </summary>
		[Description("Invalid or Unexpected Packet")]
		InvalidPacket = 53,
		/// <summary> Argument List Too Long </summary>
		[Description("Argument List Too Long")]
		TooBig = 54,
		/// <summary> Bad Version </summary>
		[Description("Bad Version")]
		BadVersion = 55,
		/// <summary> Closed </summary>
		[Description("Closed")]
		Closed = 56,
		/// <summary> Not Configured </summary>
		[Description("Not Configured")]
		NotConfigured = 57,
		/// <summary> End of File </summary>
		[Description("End of File")]
		EndOfFile = 31,
	}
	/// <summary>The error code from an Error event
	/// </summary>
	public enum ErrorEventCode : int {
		/// <summary> Client and Server protocol versions don't match. </summary>
		[Description("Client and Server protocol versions don't match.")]
		BadVersion = 1,
		/// <summary> Phidget is in use. </summary>
		[Description("Phidget is in use.")]
		Busy = 2,
		/// <summary> Networking communication error. </summary>
		[Description("Networking communication error.")]
		Network = 3,
		/// <summary> An error occured dispatching a command or event. </summary>
		[Description("An error occured dispatching a command or event.")]
		Dispatch = 4,
		/// <summary> A general failure occured - see description for details. </summary>
		[Description("A general failure occured - see description for details.")]
		Failure = 5,
		/// <summary> An error state has cleared. </summary>
		[Description("An error state has cleared.")]
		Success = 4096,
		/// <summary> A sampling overrun happened in firmware. </summary>
		[Description("A sampling overrun happened in firmware.")]
		Overrun = 4098,
		/// <summary> One or more packets were lost. </summary>
		[Description("One or more packets were lost.")]
		PacketLost = 4099,
		/// <summary> A variable has wrapped around. </summary>
		[Description("A variable has wrapped around.")]
		WrapAround = 4100,
		/// <summary> Over-temperature condition detected. </summary>
		[Description("Over-temperature condition detected.")]
		OverTemperature = 4101,
		/// <summary> Over-current condition detected. </summary>
		[Description("Over-current condition detected.")]
		OverCurrent = 4102,
		/// <summary> Out of range condition detected. </summary>
		[Description("Out of range condition detected.")]
		OutOfRange = 4103,
		/// <summary> Power supply problem detected. </summary>
		[Description("Power supply problem detected.")]
		BadPower = 4104,
		/// <summary> Saturation condition detected. </summary>
		[Description("Saturation condition detected.")]
		Saturation = 4105,
		/// <summary> Over-voltage condition detected. </summary>
		[Description("Over-voltage condition detected.")]
		OverVoltage = 4107,
		/// <summary> Fail-safe condition detected. </summary>
		[Description("Fail-safe condition detected.")]
		FailsafeCondition = 4108,
		/// <summary> Voltage error detected. </summary>
		[Description("Voltage error detected.")]
		VoltageError = 4109,
		/// <summary> Energy dump condition detected. </summary>
		[Description("Energy dump condition detected.")]
		EnergyDumpCondition = 4110,
		/// <summary> Motor stall detected. </summary>
		[Description("Motor stall detected.")]
		MotorStallCondition = 4111,
	}
	/// <summary>Phidget device ID
	/// </summary>
	public enum DeviceID : int {
		/// <summary> Unknown device </summary>
		[Description("Unknown device")]
		None = 0,
		/// <summary> PhidgetInterfaceKit 4/8/8 </summary>
		[Description("PhidgetInterfaceKit 4/8/8")]
		PN_InterfaceKit488 = 1,
		/// <summary> PhidgetServo 1-Motor (1000) </summary>
		[Description("PhidgetServo 1-Motor (1000)")]
		PN_1000 = 2,
		/// <summary> PhidgetServo 4-Motor (1001) </summary>
		[Description("PhidgetServo 4-Motor (1001)")]
		PN_1001 = 3,
		/// <summary> PhidgetAnalog 4-Output (1002) </summary>
		[Description("PhidgetAnalog 4-Output (1002)")]
		PN_1002 = 4,
		/// <summary> PhidgetAccelerometer 2-Axis (1008) </summary>
		[Description("PhidgetAccelerometer 2-Axis (1008)")]
		PN_1008 = 5,
		/// <summary> PhidgetInterfaceKit 8/8/8 (1010, 1013, 1018, 1019) </summary>
		[Description("PhidgetInterfaceKit 8/8/8 (1010, 1013, 1018, 1019)")]
		PN_1010_1013_1018_1019 = 6,
		/// <summary> PhidgetInterfaceKit 2/2/2 (1011) </summary>
		[Description("PhidgetInterfaceKit 2/2/2 (1011)")]
		PN_1011 = 7,
		/// <summary> PhidgetInterfaceKit 0/16/16 (1012) </summary>
		[Description("PhidgetInterfaceKit 0/16/16 (1012)")]
		PN_1012 = 8,
		/// <summary> PhidgetInterfaceKit 0/0/4 (1014) </summary>
		[Description("PhidgetInterfaceKit 0/0/4 (1014)")]
		PN_1014 = 9,
		/// <summary> PhidgetLinearTouch (1015) </summary>
		[Description("PhidgetLinearTouch (1015)")]
		PN_1015 = 10,
		/// <summary> PhidgetCircularTouch (1016) </summary>
		[Description("PhidgetCircularTouch (1016)")]
		PN_1016 = 11,
		/// <summary> PhidgetInterfaceKit 0/0/8 (1017) </summary>
		[Description("PhidgetInterfaceKit 0/0/8 (1017)")]
		PN_1017 = 12,
		/// <summary> PhidgetRFID (1023) </summary>
		[Description("PhidgetRFID (1023)")]
		PN_1023 = 13,
		/// <summary> PhidgetRFID Read-Write (1024) </summary>
		[Description("PhidgetRFID Read-Write (1024)")]
		PN_1024 = 14,
		/// <summary> PhidgetLED-64 (1030) </summary>
		[Description("PhidgetLED-64 (1030)")]
		PN_1030 = 15,
		/// <summary> PhidgetLED-64 Advanced (1031) </summary>
		[Description("PhidgetLED-64 Advanced (1031)")]
		PN_1031 = 16,
		/// <summary> PhidgetLED-64 Advanced (1032) </summary>
		[Description("PhidgetLED-64 Advanced (1032)")]
		PN_1032 = 17,
		/// <summary> PhidgetGPS (1040) </summary>
		[Description("PhidgetGPS (1040)")]
		PN_1040 = 18,
		/// <summary> PhidgetSpatial 0/0/3 Basic (1041) </summary>
		[Description("PhidgetSpatial 0/0/3 Basic (1041)")]
		PN_1041 = 19,
		/// <summary> PhidgetSpatial 3/3/3 Basic (1042) </summary>
		[Description("PhidgetSpatial 3/3/3 Basic (1042)")]
		PN_1042 = 20,
		/// <summary> PhidgetSpatial Precision 0/0/3 High Resolution (1043) </summary>
		[Description("PhidgetSpatial Precision 0/0/3 High Resolution (1043)")]
		PN_1043 = 21,
		/// <summary> PhidgetSpatial Precision 3/3/3 High Resolution (1044) </summary>
		[Description("PhidgetSpatial Precision 3/3/3 High Resolution (1044)")]
		PN_1044 = 22,
		/// <summary> PhidgetTemperatureSensor IR (1045) </summary>
		[Description("PhidgetTemperatureSensor IR (1045)")]
		PN_1045 = 23,
		/// <summary> PhidgetBridge 4-Input (1046) </summary>
		[Description("PhidgetBridge 4-Input (1046)")]
		PN_1046 = 24,
		/// <summary> PhidgetEncoder HighSpeed 4-Input (1047) </summary>
		[Description("PhidgetEncoder HighSpeed 4-Input (1047)")]
		PN_1047 = 25,
		/// <summary> PhidgetTemperatureSensor 4-input (1048) </summary>
		[Description("PhidgetTemperatureSensor 4-input (1048)")]
		PN_1048 = 26,
		/// <summary> PhidgetSpatial 0/0/3 (1049) </summary>
		[Description("PhidgetSpatial 0/0/3 (1049)")]
		PN_1049 = 27,
		/// <summary> PhidgetTemperatureSensor 1-Input (1051) </summary>
		[Description("PhidgetTemperatureSensor 1-Input (1051)")]
		PN_1051 = 28,
		/// <summary> PhidgetEncoder Mechanical (1052) </summary>
		[Description("PhidgetEncoder Mechanical (1052)")]
		PN_1052 = 29,
		/// <summary> PhidgetAccelerometer 2-Axis (1053) </summary>
		[Description("PhidgetAccelerometer 2-Axis (1053)")]
		PN_1053 = 30,
		/// <summary> PhidgetFrequencyCounter (1054) </summary>
		[Description("PhidgetFrequencyCounter (1054)")]
		PN_1054 = 31,
		/// <summary> PhidgetIR (1055) </summary>
		[Description("PhidgetIR (1055)")]
		PN_1055 = 32,
		/// <summary> PhidgetSpatial 3/3/3 (1056) </summary>
		[Description("PhidgetSpatial 3/3/3 (1056)")]
		PN_1056 = 33,
		/// <summary> PhidgetEncoder HighSpeed (1057) </summary>
		[Description("PhidgetEncoder HighSpeed (1057)")]
		PN_1057 = 34,
		/// <summary> PhidgetPHSensor (1058) </summary>
		[Description("PhidgetPHSensor (1058)")]
		PN_1058 = 35,
		/// <summary> PhidgetAccelerometer 3-Axis (1059) </summary>
		[Description("PhidgetAccelerometer 3-Axis (1059)")]
		PN_1059 = 36,
		/// <summary> PhidgetMotorControl LV (1060) </summary>
		[Description("PhidgetMotorControl LV (1060)")]
		PN_1060 = 37,
		/// <summary> PhidgetAdvancedServo 8-Motor (1061) </summary>
		[Description("PhidgetAdvancedServo 8-Motor (1061)")]
		PN_1061 = 38,
		/// <summary> PhidgetStepper Unipolar 4-Motor (1062) </summary>
		[Description("PhidgetStepper Unipolar 4-Motor (1062)")]
		PN_1062 = 39,
		/// <summary> PhidgetStepper Bipolar 1-Motor (1063) </summary>
		[Description("PhidgetStepper Bipolar 1-Motor (1063)")]
		PN_1063 = 40,
		/// <summary> PhidgetMotorControl HC (1064) </summary>
		[Description("PhidgetMotorControl HC (1064)")]
		PN_1064 = 41,
		/// <summary> PhidgetMotorControl 1-Motor (1065) </summary>
		[Description("PhidgetMotorControl 1-Motor (1065)")]
		PN_1065 = 42,
		/// <summary> PhidgetAdvancedServo 1-Motor (1066) </summary>
		[Description("PhidgetAdvancedServo 1-Motor (1066)")]
		PN_1066 = 43,
		/// <summary> PhidgetStepper Bipolar HC (1067) </summary>
		[Description("PhidgetStepper Bipolar HC (1067)")]
		PN_1067 = 44,
		/// <summary> PhidgetTextLCD 20x2 with PhidgetInterfaceKit 8/8/8 (1201, 1202, 1203) </summary>
		[Description("PhidgetTextLCD 20x2 with PhidgetInterfaceKit 8/8/8 (1201, 1202, 1203)")]
		PN_1202_1203 = 45,
		/// <summary> PhidgetTextLCD Adapter (1204) </summary>
		[Description("PhidgetTextLCD Adapter (1204)")]
		PN_1204 = 46,
		/// <summary> PhidgetTextLCD 20x2 (1215/1216/1217/1218) </summary>
		[Description("PhidgetTextLCD 20x2 (1215/1216/1217/1218)")]
		PN_1215__1218 = 47,
		/// <summary> PhidgetTextLCD 20x2 with PhidgetInterfaceKit 0/8/8 (1219, 1220, 1221, 1222) </summary>
		[Description("PhidgetTextLCD 20x2 with PhidgetInterfaceKit 0/8/8 (1219, 1220, 1221, 1222)")]
		PN_1219__1222 = 48,
		/// <summary> pH Adapter </summary>
		[Description("pH Adapter")]
		PN_ADP1000 = 49,
		/// <summary> SPI Customer Interface </summary>
		[Description("SPI Customer Interface")]
		PN_ADP1001 = 50,
		/// <summary> Analog Input Module x8 </summary>
		[Description("Analog Input Module x8")]
		PN_DAQ1000 = 51,
		/// <summary> Digital Input 4 </summary>
		[Description("Digital Input 4")]
		PN_DAQ1200 = 52,
		/// <summary> Digital Input 4 Isolated </summary>
		[Description("Digital Input 4 Isolated")]
		PN_DAQ1300 = 53,
		/// <summary> Digital Input 16 </summary>
		[Description("Digital Input 16")]
		PN_DAQ1301 = 54,
		/// <summary> Versatile Input </summary>
		[Description("Versatile Input")]
		PN_DAQ1400 = 55,
		/// <summary> Bridge </summary>
		[Description("Bridge")]
		PN_DAQ1500 = 56,
		/// <summary> DC Motor Controller with PID </summary>
		[Description("DC Motor Controller with PID")]
		PN_DCC1000 = 57,
		/// <summary> 200mm Distance Sensor </summary>
		[Description("200mm Distance Sensor")]
		PN_DST1000 = 58,
		/// <summary> Sonar Distance Sensor </summary>
		[Description("Sonar Distance Sensor")]
		PN_DST1200 = 59,
		/// <summary> Encoder </summary>
		[Description("Encoder")]
		PN_ENC1000 = 60,
		/// <summary> Capacitive Touch Sensor </summary>
		[Description("Capacitive Touch Sensor")]
		PN_HIN1000 = 61,
		/// <summary> Capacitive Scroll </summary>
		[Description("Capacitive Scroll")]
		PN_HIN1001 = 62,
		/// <summary> Joystick </summary>
		[Description("Joystick")]
		PN_HIN1100 = 63,
		/// <summary> Phidget Hub with 6 ports </summary>
		[Description("Phidget Hub with 6 ports")]
		PN_HUB0000 = 64,
		/// <summary> Phidget Mesh Hub with 4 ports </summary>
		[Description("Phidget Mesh Hub with 4 ports")]
		PN_HUB0001 = 65,
		/// <summary> Phidget Mesh Dongle </summary>
		[Description("Phidget Mesh Dongle")]
		PN_HUB0002 = 66,
		/// <summary> Phidget SPI VINT Hub with 6 ports </summary>
		[Description("Phidget SPI VINT Hub with 6 ports")]
		PN_HUB0004 = 67,
		/// <summary> Phidget Lightning Hub with 6 ports </summary>
		[Description("Phidget Lightning Hub with 6 ports")]
		PN_HUB0005 = 68,
		/// <summary> Humidity Sensor </summary>
		[Description("Humidity Sensor")]
		PN_HUM1000 = 69,
		/// <summary> LCD </summary>
		[Description("LCD")]
		PN_LCD1100 = 70,
		/// <summary> LED Driver 32 </summary>
		[Description("LED Driver 32")]
		PN_LED1000 = 71,
		/// <summary> Light Sensor </summary>
		[Description("Light Sensor")]
		PN_LUX1000 = 72,
		/// <summary> Accelerometer 0/0/3 </summary>
		[Description("Accelerometer 0/0/3")]
		PN_MOT1100 = 73,
		/// <summary> Spatial 3/3/3 </summary>
		[Description("Spatial 3/3/3")]
		PN_MOT1101 = 74,
		/// <summary> Analog Output 0-5V </summary>
		[Description("Analog Output 0-5V")]
		PN_OUT1000 = 75,
		/// <summary> Analog Output (+/-)10V </summary>
		[Description("Analog Output (+/-)10V")]
		PN_OUT1001 = 76,
		/// <summary> Analog Output (+/-)10V - 16 bit </summary>
		[Description("Analog Output (+/-)10V - 16 bit")]
		PN_OUT1002 = 77,
		/// <summary> Digital Output 4 </summary>
		[Description("Digital Output 4")]
		PN_OUT1100 = 78,
		/// <summary> Barometer </summary>
		[Description("Barometer")]
		PN_PRE1000 = 79,
		/// <summary> 8-Servo Controller </summary>
		[Description("8-Servo Controller")]
		PN_RCC1000 = 80,
		/// <summary> Power Relay 4 </summary>
		[Description("Power Relay 4")]
		PN_REL1000 = 81,
		/// <summary> Digital Output 4 Isolated </summary>
		[Description("Digital Output 4 Isolated")]
		PN_REL1100 = 82,
		/// <summary> Digital Output 16 Isolated </summary>
		[Description("Digital Output 16 Isolated")]
		PN_REL1101 = 83,
		/// <summary> Power Supply Protector </summary>
		[Description("Power Supply Protector")]
		PN_SAF1000 = 84,
		/// <summary> Sound Pressure Level Sensor </summary>
		[Description("Sound Pressure Level Sensor")]
		PN_SND1000 = 85,
		/// <summary> Bipolar Stepper Motor Controller </summary>
		[Description("Bipolar Stepper Motor Controller")]
		PN_STC1000 = 86,
		/// <summary> Integrated Temperature Sensor </summary>
		[Description("Integrated Temperature Sensor")]
		PN_TMP1000 = 87,
		/// <summary> Thermocouple 1 </summary>
		[Description("Thermocouple 1")]
		PN_TMP1100 = 88,
		/// <summary> Thermocouple 4 </summary>
		[Description("Thermocouple 4")]
		PN_TMP1101 = 89,
		/// <summary> RTD </summary>
		[Description("RTD")]
		PN_TMP1200 = 90,
		/// <summary> Infrared Temperature Sensor </summary>
		[Description("Infrared Temperature Sensor")]
		PN_TMP1300 = 91,
		/// <summary> Voltage Sensor High Precision </summary>
		[Description("Voltage Sensor High Precision")]
		PN_VCP1000 = 92,
		/// <summary> Voltage Sensor Large </summary>
		[Description("Voltage Sensor Large")]
		PN_VCP1001 = 93,
		/// <summary> Voltage Sensor Small </summary>
		[Description("Voltage Sensor Small")]
		PN_VCP1002 = 94,
		/// <summary> Hub Port in Digital Input mode </summary>
		[Description("Hub Port in Digital Input mode")]
		DigitalInputPort = 95,
		/// <summary> Hub Port in Digital Output mode </summary>
		[Description("Hub Port in Digital Output mode")]
		DigitalOutputPort = 96,
		/// <summary> Hub Port in Voltage Input mode </summary>
		[Description("Hub Port in Voltage Input mode")]
		VoltageInputPort = 97,
		/// <summary> Hub Port in Voltage Ratio Input mode </summary>
		[Description("Hub Port in Voltage Ratio Input mode")]
		VoltageRatioInputPort = 98,
		/// <summary> Generic USB device </summary>
		[Description("Generic USB device")]
		GenericUSB = 99,
		/// <summary> Generic VINT device </summary>
		[Description("Generic VINT device")]
		GenericVINT = 100,
		/// <summary> USB device in firmware upgrade mode </summary>
		[Description("USB device in firmware upgrade mode")]
		FirmwareUpgradeUSB = 101,
		/// <summary> VINT Device in firmware upgrade mode, STM32F0 Proc. </summary>
		[Description("VINT Device in firmware upgrade mode, STM32F0 Proc.")]
		FirmwareUpgradeSTM32F0 = 102,
		/// <summary> VINT Device in firmware upgrade mode, STM8S Proc. </summary>
		[Description("VINT Device in firmware upgrade mode, STM8S Proc.")]
		FirmwareUpgradeSTM8S = 103,
		/// <summary> Phidget SPI device under firmware upgrade </summary>
		[Description("Phidget SPI device under firmware upgrade")]
		FirmwareUpgradeSPI = 104,
		/// <summary> 30A Current Sensor </summary>
		[Description("30A Current Sensor")]
		PN_VCP1100 = 105,
		/// <summary> BLDC Motor Controller </summary>
		[Description("BLDC Motor Controller")]
		PN_DCC1100 = 108,
		/// <summary> Dial Encoder </summary>
		[Description("Dial Encoder")]
		PN_HIN1101 = 109,
		/// <summary> Small DC Motor Controller </summary>
		[Description("Small DC Motor Controller")]
		PN_DCC1001 = 110,
		/// <summary> Dictionary </summary>
		[Description("Dictionary")]
		PN_DICTIONARY = 111,
		/// <summary> Bipolar Stepper Motor SmallController </summary>
		[Description("Bipolar Stepper Motor SmallController")]
		PN_STC1001 = 115,
		/// <summary> OS Testing Fixture </summary>
		[Description("OS Testing Fixture")]
		PN_USBSwitch = 116,
		/// <summary> 4A Small DC Motor Controller </summary>
		[Description("4A Small DC Motor Controller")]
		PN_DCC1002 = 117,
		/// <summary> 8A Bipolar Stepper Motor Controller </summary>
		[Description("8A Bipolar Stepper Motor Controller")]
		PN_STC1002 = 118,
		/// <summary> 4A Bipolar Stepper Motor SmallController </summary>
		[Description("4A Bipolar Stepper Motor SmallController")]
		PN_STC1003 = 119,
		/// <summary> 2 Channel DC Motor Controller </summary>
		[Description("2 Channel DC Motor Controller")]
		PN_DCC1003 = 120,
	}
	/// <summary>Phidget logging level
	/// </summary>
	public enum LogLevel : int {
		/// <summary> Critical </summary>
		[Description("Critical")]
		Critical = 1,
		/// <summary> Error </summary>
		[Description("Error")]
		Error = 2,
		/// <summary> Warning </summary>
		[Description("Warning")]
		Warning = 3,
		/// <summary> Info </summary>
		[Description("Info")]
		Info = 4,
		/// <summary> Debug </summary>
		[Description("Debug")]
		Debug = 5,
		/// <summary> Verbose </summary>
		[Description("Verbose")]
		Verbose = 6,
	}
	/// <summary>Phidget device class
	/// </summary>
	public enum DeviceClass : int {
		/// <summary> Any device </summary>
		[Description("PhidgetNone")]
		None = 0,
		/// <summary> PhidgetAccelerometer device </summary>
		[Description("PhidgetAccelerometer")]
		Accelerometer = 1,
		/// <summary> PhidgetAdvancedServo device </summary>
		[Description("PhidgetAdvancedServo")]
		AdvancedServo = 2,
		/// <summary> PhidgetAnalog device </summary>
		[Description("PhidgetAnalog")]
		Analog = 3,
		/// <summary> PhidgetBridge device </summary>
		[Description("PhidgetBridge")]
		Bridge = 4,
		/// <summary> PhidgetEncoder device </summary>
		[Description("PhidgetEncoder")]
		Encoder = 5,
		/// <summary> PhidgetFrequencyCounter device </summary>
		[Description("PhidgetFrequencyCounter")]
		FrequencyCounter = 6,
		/// <summary> PhidgetGPS device </summary>
		[Description("PhidgetGPS")]
		GPS = 7,
		/// <summary> Phidget VINT Hub device </summary>
		[Description("PhidgetHub")]
		Hub = 8,
		/// <summary> PhidgetInterfaceKit device </summary>
		[Description("PhidgetInterfaceKit")]
		InterfaceKit = 9,
		/// <summary> PhidgetIR device </summary>
		[Description("PhidgetIR")]
		IR = 10,
		/// <summary> PhidgetLED device </summary>
		[Description("PhidgetLED")]
		LED = 11,
		/// <summary> Phidget Mesh Dongle </summary>
		[Description("PhidgetMeshDongle")]
		MeshDongle = 12,
		/// <summary> PhidgetMotorControl device </summary>
		[Description("PhidgetMotorControl")]
		MotorControl = 13,
		/// <summary> PhidgetPHSensor device </summary>
		[Description("PhidgetPHSensor")]
		PHSensor = 14,
		/// <summary> PhidgetRFID device </summary>
		[Description("PhidgetRFID")]
		RFID = 15,
		/// <summary> PhidgetServo device </summary>
		[Description("PhidgetServo")]
		Servo = 16,
		/// <summary> PhidgetSpatial device </summary>
		[Description("PhidgetSpatial")]
		Spatial = 17,
		/// <summary> PhidgetStepper device </summary>
		[Description("PhidgetStepper")]
		Stepper = 18,
		/// <summary> PhidgetTemperatureSensor device </summary>
		[Description("PhidgetTemperatureSensor")]
		TemperatureSensor = 19,
		/// <summary> PhidgetTextLCD device </summary>
		[Description("PhidgetTextLCD")]
		TextLCD = 20,
		/// <summary> Phidget VINT device </summary>
		[Description("PhidgetVINT")]
		VINT = 21,
		/// <summary> Generic device </summary>
		[Description("PhidgetGeneric")]
		Generic = 22,
		/// <summary> Phidget device in Firmware Upgrade mode </summary>
		[Description("PhidgetFirmwareUpgrade")]
		FirmwareUpgrade = 23,
		/// <summary> Dictionary device </summary>
		[Description("PhidgetDictionary")]
		Dictionary = 24,
	}
	/// <summary>Phidget channel class
	/// </summary>
	public enum ChannelClass : int {
		/// <summary> Any channel </summary>
		[Description("PhidgetNone")]
		None = 0,
		/// <summary> Accelerometer channel </summary>
		[Description("PhidgetAccelerometer")]
		Accelerometer = 1,
		/// <summary> Current input channel </summary>
		[Description("PhidgetCurrentInput")]
		CurrentInput = 2,
		/// <summary> Data adapter channel </summary>
		[Description("PhidgetDataAdapter")]
		DataAdapter = 3,
		/// <summary> DC motor channel </summary>
		[Description("PhidgetDCMotor")]
		DCMotor = 4,
		/// <summary> Digital input channel </summary>
		[Description("PhidgetDigitalInput")]
		DigitalInput = 5,
		/// <summary> Digital output channel </summary>
		[Description("PhidgetDigitalOutput")]
		DigitalOutput = 6,
		/// <summary> Distance sensor channel </summary>
		[Description("PhidgetDistanceSensor")]
		DistanceSensor = 7,
		/// <summary> Encoder channel </summary>
		[Description("PhidgetEncoder")]
		Encoder = 8,
		/// <summary> Frequency counter channel </summary>
		[Description("PhidgetFrequencyCounter")]
		FrequencyCounter = 9,
		/// <summary> GPS channel </summary>
		[Description("PhidgetGPS")]
		GPS = 10,
		/// <summary> LCD channel </summary>
		[Description("PhidgetLCD")]
		LCD = 11,
		/// <summary> Gyroscope channel </summary>
		[Description("PhidgetGyroscope")]
		Gyroscope = 12,
		/// <summary> VINT Hub channel </summary>
		[Description("PhidgetHub")]
		Hub = 13,
		/// <summary> Capacitive Touch channel </summary>
		[Description("PhidgetCapacitiveTouch")]
		CapacitiveTouch = 14,
		/// <summary> Humidity sensor channel </summary>
		[Description("PhidgetHumiditySensor")]
		HumiditySensor = 15,
		/// <summary> IR channel </summary>
		[Description("PhidgetIR")]
		IR = 16,
		/// <summary> Light sensor channel </summary>
		[Description("PhidgetLightSensor")]
		LightSensor = 17,
		/// <summary> Magnetometer channel </summary>
		[Description("PhidgetMagnetometer")]
		Magnetometer = 18,
		/// <summary> Mesh dongle channel </summary>
		[Description("PhidgetMeshDongle")]
		MeshDongle = 19,
		/// <summary> pH sensor channel </summary>
		[Description("PhidgetPHSensor")]
		PHSensor = 37,
		/// <summary> Power guard channel </summary>
		[Description("PhidgetPowerGuard")]
		PowerGuard = 20,
		/// <summary> Pressure sensor channel </summary>
		[Description("PhidgetPressureSensor")]
		PressureSensor = 21,
		/// <summary> RC Servo channel </summary>
		[Description("PhidgetRCServo")]
		RCServo = 22,
		/// <summary> Resistance input channel </summary>
		[Description("PhidgetResistanceInput")]
		ResistanceInput = 23,
		/// <summary> RFID channel </summary>
		[Description("PhidgetRFID")]
		RFID = 24,
		/// <summary> Sound sensor channel </summary>
		[Description("PhidgetSoundSensor")]
		SoundSensor = 25,
		/// <summary> Spatial channel </summary>
		[Description("PhidgetSpatial")]
		Spatial = 26,
		/// <summary> Stepper channel </summary>
		[Description("PhidgetStepper")]
		Stepper = 27,
		/// <summary> Temperature sensor channel </summary>
		[Description("PhidgetTemperatureSensor")]
		TemperatureSensor = 28,
		/// <summary> Voltage input channel </summary>
		[Description("PhidgetVoltageInput")]
		VoltageInput = 29,
		/// <summary> Voltage output channel </summary>
		[Description("PhidgetVoltageOutput")]
		VoltageOutput = 30,
		/// <summary> Voltage ratio input channel </summary>
		[Description("PhidgetVoltageRatioInput")]
		VoltageRatioInput = 31,
		/// <summary> Firmware upgrade channel </summary>
		[Description("PhidgetFirmwareUpgrade")]
		FirmwareUpgrade = 32,
		/// <summary> Generic channel </summary>
		[Description("PhidgetGeneric")]
		Generic = 33,
		/// <summary> Motor position control channel. </summary>
		[Description("PhidgetMotorPositionController")]
		MotorPositionController = 34,
		/// <summary> BLDC motor channel </summary>
		[Description("PhidgetBLDCMotor")]
		BLDCMotor = 35,
		/// <summary> Dictionary </summary>
		[Description("PhidgetDictionary")]
		Dictionary = 36,
	}
	/// <summary>Phidget channel sub class
	/// </summary>
	public enum ChannelSubclass : int {
		/// <summary> No subclass </summary>
		[Description("No subclass")]
		None = 1,
		/// <summary> Digital output duty cycle </summary>
		[Description("Digital output duty cycle")]
		DigitalOutputDutyCycle = 16,
		/// <summary> Digital output LED driver </summary>
		[Description("Digital output LED driver")]
		DigitalOutputLEDDriver = 17,
		/// <summary> Temperature sensor RTD </summary>
		[Description("Temperature sensor RTD")]
		TemperatureSensorRTD = 32,
		/// <summary> Temperature sensor thermocouple </summary>
		[Description("Temperature sensor thermocouple")]
		TemperatureSensorThermocouple = 33,
		/// <summary> Voltage sensor port </summary>
		[Description("Voltage sensor port")]
		VoltageInputSensorPort = 48,
		/// <summary> Voltage ratio sensor port </summary>
		[Description("Voltage ratio sensor port")]
		VoltageRatioInputSensorPort = 64,
		/// <summary> Voltage ratio bridge input </summary>
		[Description("Voltage ratio bridge input")]
		VoltageRatioInputBridge = 65,
		/// <summary> Graphic LCD </summary>
		[Description("Graphic LCD")]
		LCDGraphic = 80,
		/// <summary> Text LCD </summary>
		[Description("Text LCD")]
		LCDText = 81,
		/// <summary> Encoder IO mode settable </summary>
		[Description("Encoder IO mode settable")]
		EncoderModeSettable = 96,
		/// <summary> RC Servo Embedded </summary>
		[Description("RC Servo Embedded")]
		RCServoEmbedded = 112,
	}
	/// <summary>Phidget mesh mode
	/// </summary>
	public enum MeshMode : int {
		/// <summary> Router mode </summary>
		[Description("Router mode")]
		Router = 1,
		/// <summary> Sleepy end device mode </summary>
		[Description("Sleepy end device mode")]
		SleepyEndDevice = 2,
	}
	/// <summary>The voltage level being provided to the sensor
	/// </summary>
	public enum PowerSupply : int {
		/// <summary> Switch the sensor power supply off </summary>
		[Description("Off")]
		Off = 1,
		/// <summary> The sensor is provided with 12 volts </summary>
		[Description("12 V")]
		Volts_12 = 2,
		/// <summary> The sensor is provided with 24 volts </summary>
		[Description("24 V")]
		Volts_24 = 3,
	}
	/// <summary>RTD wiring configuration
	/// </summary>
	public enum RTDWireSetup : int {
		/// <summary> Configures the device to make resistance calculations based on a 2-wire RTD setup. </summary>
		[Description("2 Wire")]
		Wires_2 = 1,
		/// <summary> Configures the device to make resistance calculations based on a 3-wire RTD setup. </summary>
		[Description("3 Wire")]
		Wires_3 = 2,
		/// <summary> Configures the device to make resistance calculations based on a 4-wire RTD setup. </summary>
		[Description("4 Wire")]
		Wires_4 = 3,
	}
	/// <summary>The selected polarity mode for the digital input
	/// </summary>
	public enum InputMode : int {
		/// <summary> For interfacing NPN digital sensors </summary>
		[Description("NPN")]
		NPN = 1,
		/// <summary> For interfacing PNP digital sensors </summary>
		[Description("PNP")]
		PNP = 2,
	}
	/// <summary>The operating condition of the fan. Choose between on, off, or automatic (based on temperature).
	/// </summary>
	public enum FanMode : int {
		/// <summary> Turns the fan off. </summary>
		[Description("Off")]
		Off = 1,
		/// <summary> Turns the fan on. </summary>
		[Description("On")]
		On = 2,
		/// <summary> The fan will be automatically controlled based on temperature. </summary>
		[Description("Automatic")]
		Auto = 3,
	}
	/// <summary>Analog sensor units. These correspond to the types of quantities that can be measured by Phidget
	/// analog sensors.
	/// </summary>
	public enum Unit : int {
		/// <summary> Unitless </summary>
		[Description("Unitless")]
		None = 0,
		/// <summary> Boolean </summary>
		[Description("Boolean")]
		Boolean = 1,
		/// <summary> Percent </summary>
		[Description("Percent")]
		Percent = 2,
		/// <summary> Decibel </summary>
		[Description("Decibel")]
		Decibel = 3,
		/// <summary> Millimeter </summary>
		[Description("Millimeter")]
		Millimeter = 4,
		/// <summary> Centimeter </summary>
		[Description("Centimeter")]
		Centimeter = 5,
		/// <summary> Meter </summary>
		[Description("Meter")]
		Meter = 6,
		/// <summary> Gram </summary>
		[Description("Gram")]
		Gram = 7,
		/// <summary> Kilogram </summary>
		[Description("Kilogram")]
		Kilogram = 8,
		/// <summary> Milliampere </summary>
		[Description("Milliampere")]
		Milliampere = 9,
		/// <summary> Ampere </summary>
		[Description("Ampere")]
		Ampere = 10,
		/// <summary> Kilopascal </summary>
		[Description("Kilopascal")]
		Kilopascal = 11,
		/// <summary> Volt </summary>
		[Description("Volt")]
		Volt = 12,
		/// <summary> Degree Celcius </summary>
		[Description("Degree Celcius")]
		DegreeCelcius = 13,
		/// <summary> Lux </summary>
		[Description("Lux")]
		Lux = 14,
		/// <summary> Gauss </summary>
		[Description("Gauss")]
		Gauss = 15,
		/// <summary> pH </summary>
		[Description("pH")]
		pH = 16,
		/// <summary> Watt </summary>
		[Description("Watt")]
		Watt = 17,
	}
	/// <summary>The forward voltage setting of the LED
	/// </summary>
	public enum LEDForwardVoltage : int {
		/// <summary> 1.7 V </summary>
		[Description("1.7 V")]
		Volts_1_7 = 1,
		/// <summary> 2.75 V </summary>
		[Description("2.75 V")]
		Volts_2_75 = 2,
		/// <summary> 3.2 V </summary>
		[Description("3.2 V")]
		Volts_3_2 = 3,
		/// <summary> 3.9 V </summary>
		[Description("3.9 V")]
		Volts_3_9 = 4,
		/// <summary> 4.0 V </summary>
		[Description("4.0 V")]
		Volts_4_0 = 5,
		/// <summary> 4.8 V </summary>
		[Description("4.8 V")]
		Volts_4_8 = 6,
		/// <summary> 5.0 V </summary>
		[Description("5.0 V")]
		Volts_5_0 = 7,
		/// <summary> 5.6 V </summary>
		[Description("5.6 V")]
		Volts_5_6 = 8,
	}
	/// <summary>Type of filter used on the frequency input
	/// </summary>
	public enum FrequencyFilterType : int {
		/// <summary> Frequency is calculated from the number of times the signal transitions from a negative voltage to a positive voltage and back again. </summary>
		[Description("Zero Crossing")]
		ZeroCrossing = 1,
		/// <summary> Frequency is calculated from the number of times the signal transitions from a logic false to a logic true and back again. </summary>
		[Description("Logic Level")]
		LogicLevel = 2,
	}
	/// <summary>The GPS time in UTC
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPSTime {
		/// <summary> Milliseconds </summary>
		public short Millisecond;
		/// <summary> Seconds </summary>
		public short Second;
		/// <summary> Minutes </summary>
		public short Minute;
		/// <summary> Hours </summary>
		public short Hour;
	}
	/// <summary>GPS Date in UTC
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPSDate {
		/// <summary> Day (1-31) </summary>
		public short Day;
		/// <summary> Month (1-12) </summary>
		public short Month;
		/// <summary> Year </summary>
		public short Year;
	}
	/// <summary>NMEA GGA Sentence
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPGGA {
		/// <summary> Latitude </summary>
		public double Latitude;
		/// <summary> Longitude </summary>
		public double Longitude;
		/// <summary> GPS quality indicator </summary>
		public short FixQuality;
		/// <summary> Number of satellites in use </summary>
		public short NumSatellites;
		/// <summary> Horizontal dilution of precision </summary>
		public double HorizontalDilution;
		/// <summary> Mean sea level altitude </summary>
		public double Altitude;
		/// <summary> Geoidal separation </summary>
		public double HeightOfGeoid;
	}
	/// <summary>NMEA GSA sentence
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPGSA {
		/// <summary> Manual/Automatic mode (A = auto, M = manual) </summary>
		public char Mode;
		/// <summary> Fix type (1 = no fix, 2 = 2D, 3 = 3D) </summary>
		public short FixType;
		/// <summary> Satellite IDs </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
		public short[] SatUsed;
		/// <summary> Position dilution of precision </summary>
		public double PositionDilution;
		/// <summary> Horizontal dilution of precision </summary>
		public double HorizontalDilution;
		/// <summary> Vertical dilution of precision </summary>
		public double VerticalDilution;
	}
	/// <summary>NMEA RMC sentence
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPRMC {
		/// <summary> Status of the data </summary>
		public char Status;
		/// <summary> Latitude </summary>
		public double Latitude;
		/// <summary> Longitude </summary>
		public double Longitude;
		/// <summary> Speed over ground in knots </summary>
		public double SpeedKnots;
		/// <summary> Heading over ground in degrees </summary>
		public double Heading;
		/// <summary> Magnetic variation </summary>
		public double MagneticVariation;
		/// <summary> Mode indicator </summary>
		public char Mode;
	}
	/// <summary>NMEA VTG sentence
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GPVTG {
		/// <summary> True heading over ground </summary>
		public double TrueHeading;
		/// <summary> Magnetic heading </summary>
		public double MagneticHeading;
		/// <summary> Speed over ground in knots </summary>
		public double SpeedKnots;
		/// <summary> Speed over ground in km/h </summary>
		public double Speed;
		/// <summary> Mode indicator </summary>
		public char Mode;
	}
	/// <summary>The NMEA Data structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct NMEAData {
		/// <summary> NMEA GGA Sentence </summary>
		public GPGGA GGA;
		/// <summary> NMEA GSA Sentence </summary>
		public GPGSA GSA;
		/// <summary> NMEA RMC Sentence </summary>
		public GPRMC RMC;
		/// <summary> NMEA VTG Sentence </summary>
		public GPVTG VTG;
	}
	/// <summary>The mode of the VINT port
	/// </summary>
	public enum HubPortMode : int {
		/// <summary> Communicate with a smart VINT device </summary>
		[Description("VINT")]
		VINT = 0,
		/// <summary> 5V Logic-level digital input </summary>
		[Description("Digital Input")]
		DigitalInput = 1,
		/// <summary> 3.3V digital output </summary>
		[Description("Digital Output")]
		DigitalOutput = 2,
		/// <summary> 0-5V voltage input for non-ratiometric sensors  </summary>
		[Description("Voltage Input")]
		VoltageInput = 3,
		/// <summary> 0-5V voltage input for ratiometric sensors </summary>
		[Description("Voltage Ratio Input")]
		VoltageRatioInput = 4,
	}
	/// <summary>Describes the encoding technique used for the code
	/// </summary>
	public enum IRCodeEncoding : int {
		/// <summary> Unknown - the default value </summary>
		[Description("Unknown")]
		Unknown = 1,
		/// <summary> Space encoding, or Pulse Distance Modulation </summary>
		[Description("Space")]
		Space = 2,
		/// <summary> Pulse encoding, or Pulse Width Modulation </summary>
		[Description("Pulse")]
		Pulse = 3,
		/// <summary> Bi-Phase, or Manchester encoding </summary>
		[Description("BiPhase")]
		BiPhase = 4,
		/// <summary> RC5 - a type of Bi-Phase encoding </summary>
		[Description("RC5")]
		RC5 = 5,
		/// <summary> RC6 - a type of Bi-Phase encoding </summary>
		[Description("RC6")]
		RC6 = 6,
	}
	/// <summary>The length type of the bitstream and gaps
	/// </summary>
	public enum IRCodeLength : int {
		/// <summary> Unknown - the default value </summary>
		[Description("Unknown")]
		Unknown = 1,
		/// <summary> Constant - the bitstream and gap length is constant </summary>
		[Description("Constant")]
		Constant = 2,
		/// <summary> Variable - the bitstream has a variable length with a constant gap </summary>
		[Description("Variable")]
		Variable = 3,
	}
	/// <summary>The PhidgetIR CodeInfo structure contains all information needed to transmit a code, apart from the
	/// actual code data.
	/// <list>
	/// <item>Some values can be set to null to select defaults.</item>
	/// </list>
	/// 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct IRCodeInfo {
		/// <summary> Number of bits in the code </summary>
		public int BitCount;
		/// <summary> Encoding technique used to encode the data </summary>
		public IRCodeEncoding Encoding;
		/// <summary> Constant or Variable length encoding </summary>
		public IRCodeLength Length;
		/// <summary> Gap time in microseconds </summary>
		public int Gap;
		/// <summary> Trail time in microseconds. Can be zero for no trail </summary>
		public int Trail;
		/// <summary> Header pulse and space. Can be zero for no header </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] Header;
		/// <summary> Pulse and Space times to represent a '1' bit, in microseconds </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] One;
		/// <summary> Pulse and Space times to represent a '0' bit, in microseconds </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] Zero;
		/// <summary> A series or pulse and space times to represent the repeat code. Start and end with pulses and null terminate. Set to 0 for none. </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
		public int[] Repeat;
		/// <summary> Minium number of times to repeat a code on transmit </summary>
		public int MinRepeat;
		/// <summary> Duty Cycle in percent (0.1-0.5). Defaults to 0.33 </summary>
		public double DutyCycle;
		/// <summary> Carrier frequency in Hz - defaults to 38kHz </summary>
		public int CarrierFrequency;
		/// <summary> Bit toggles, which are applied to the code after each transmit </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
		public string ToggleMask;
	}
	/// <summary>The type of font being used
	/// </summary>
	public enum LCDFont : int {
		/// <summary> User-defined font #1 </summary>
		[Description("User 1")]
		User1 = 1,
		/// <summary> User-defined font #2 </summary>
		[Description("User 2")]
		User2 = 2,
		/// <summary> 6px by 10px font </summary>
		[Description("6x10")]
		Dimensions_6x10 = 3,
		/// <summary> 5px by 8px font </summary>
		[Description("5x8")]
		Dimensions_5x8 = 4,
		/// <summary> 6px by 12px font </summary>
		[Description("6x12")]
		Dimensions_6x12 = 5,
	}
	/// <summary>Size of the attached LCD screen
	/// </summary>
	public enum LCDScreenSize : int {
		/// <summary> Screen size unknown </summary>
		[Description("No Screen")]
		NoScreen = 1,
		/// <summary> One row, eight column text screen </summary>
		[Description("1x8")]
		Dimensions_1x8 = 2,
		/// <summary> Two row, eight column text screen </summary>
		[Description("2x8")]
		Dimensions_2x8 = 3,
		/// <summary> One row, 16 column text screen </summary>
		[Description("1x16")]
		Dimensions_1x16 = 4,
		/// <summary> Two row, 16 column text screen </summary>
		[Description("2x16")]
		Dimensions_2x16 = 5,
		/// <summary> Four row, 16 column text screen </summary>
		[Description("4x16")]
		Dimensions_4x16 = 6,
		/// <summary> Two row, 20 column text screen </summary>
		[Description("2x20")]
		Dimensions_2x20 = 7,
		/// <summary> Four row, 20 column text screen. </summary>
		[Description("4x20")]
		Dimensions_4x20 = 8,
		/// <summary> Two row, 24 column text screen </summary>
		[Description("2x24")]
		Dimensions_2x24 = 9,
		/// <summary> One row, 40 column text screen </summary>
		[Description("1x40")]
		Dimensions_1x40 = 10,
		/// <summary> Two row, 40 column text screen </summary>
		[Description("2x40")]
		Dimensions_2x40 = 11,
		/// <summary> Four row, 40 column text screen </summary>
		[Description("4x40")]
		Dimensions_4x40 = 12,
		/// <summary> 64px by 128px graphic screen </summary>
		[Description("64x128")]
		Dimensions_64x128 = 13,
	}
	/// <summary>The on/off state of the pixel to be set
	/// </summary>
	public enum LCDPixelState : int {
		/// <summary> Pixel off state </summary>
		[Description("Off")]
		Off = 0,
		/// <summary> Pixel on state </summary>
		[Description("On")]
		On = 1,
		/// <summary> Invert the pixel state </summary>
		[Description("Invert")]
		Invert = 2,
	}
	/// <summary>The name, symbol, and Phidgets enumeration of the units of the sensor value calculated from the
	/// analog sensor's measurements.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct UnitInfo {
		/// <summary> Unit </summary>
		public Unit Unit;
		/// <summary> Name </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;
		/// <summary> Symbol </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Symbol;
	}
	/// <summary>Phidget Server Types
	/// </summary>
	public enum ServerType : int {
		/// <summary> Unknown or unspecified server type </summary>
		[Description("Unknown or unspecified server type")]
		None = 0,
		/// <summary> Phidget22 Server listener </summary>
		[Description("Phidget22 Server listener")]
		DeviceListener = 1,
		/// <summary> Phidget22 Server connection </summary>
		[Description("Phidget22 Server connection")]
		Device = 2,
		/// <summary> Phidget22 Server<br/>Server discovery with this server type allows discovery of servers hosting Phidget devices. Enabling server discovery with this server type allows automated connection to these servers, and the Phidgets connected to them. Enabling server discovery with this server type will also enable ServerAdded and ServerRemoved events for this server type. </summary>
		[Description("Phidget22 Server<br/>Server discovery with this server type allows discovery of servers hosting Phidget devices. Enabling server discovery with this server type allows automated connection to these servers, and the Phidgets connected to them. Enabling server discovery with this server type will also enable ServerAdded and ServerRemoved events for this server type.")]
		DeviceRemote = 3,
		/// <summary> Phidget22 Web Server </summary>
		[Description("Phidget22 Web Server")]
		WWWListener = 4,
		/// <summary> Phidget22 Web Server connection </summary>
		[Description("Phidget22 Web Server connection")]
		WWW = 5,
		/// <summary> Phidget22 Web server<br/>Server discovery with this server type detects the presence of Phidget web servers used to communicate with in-browser JavaScript. Enabling server discovery with this server type will enable ServerAdded and ServerRemoved events for this server type. </summary>
		[Description("Phidget22 Web server<br/>Server discovery with this server type detects the presence of Phidget web servers used to communicate with in-browser JavaScript. Enabling server discovery with this server type will enable ServerAdded and ServerRemoved events for this server type.")]
		WWWRemote = 6,
		/// <summary> Phidget SBC<br/>Server discovery with this server type detects the presence of Phidget SBCs on the network. Enabling server discovery with this server type will enable ServerAdded and ServerRemoved events for this server type. </summary>
		[Description("Phidget SBC<br/>Server discovery with this server type detects the presence of Phidget SBCs on the network. Enabling server discovery with this server type will enable ServerAdded and ServerRemoved events for this server type.")]
		SBC = 7,
	}
	/// <summary>Describes a known server. See Constants for supported flags.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhidgetServer {
		/// <summary> The name of the server </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;
		/// <summary> Name of the server type </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string TypeName;
		/// <summary> The server type </summary>
		public ServerType Type;
		/// <summary> Flags describing the server state </summary>
		public int Flags;
		/// <summary> The address of the server </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Address;
		/// <summary> The hostname of the server </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Hostname;
		/// <summary> The port number of the server </summary>
		public int Port;
	}
	/// <summary>Voltage supplied to all attached servos
	/// </summary>
	public enum RCServoVoltage : int {
		/// <summary> Run all servos on 5V DC </summary>
		[Description("5.0 V")]
		Volts_5_0 = 1,
		/// <summary> Run all servos on 6V DC </summary>
		[Description("6.0 V")]
		Volts_6_0 = 2,
		/// <summary> Run all servos on 7.4V DC </summary>
		[Description("7.4 V")]
		Volts_7_4 = 3,
	}
	/// <summary>The protocol used to communicate with the tags
	/// </summary>
	public enum RFIDProtocol : int {
		/// <summary> EM4100 </summary>
		[Description("EM4100")]
		EM4100 = 1,
		/// <summary> ISO11785 FDX B </summary>
		[Description("ISO11785 FDX B")]
		ISO11785_FDX_B = 2,
		/// <summary> PhidgetTAG </summary>
		[Description("PhidgetTAG")]
		PhidgetTAG = 3,
	}
	/// <summary>The measurement range of the sound sensor
	/// </summary>
	public enum SPLRange : int {
		/// <summary> Range 102dB </summary>
		[Description("102 dB")]
		dB_102 = 1,
		/// <summary> Range 82dB </summary>
		[Description("82 dB")]
		dB_82 = 2,
	}
	/// <summary>Method of motor control
	/// </summary>
	public enum StepperControlMode : int {
		/// <summary> Control the motor by setting a target position. </summary>
		[Description("Step")]
		Step = 0,
		/// <summary> Control the motor by selecting a target velocity (sign indicates direction). The motor will rotate continously in the chosen direction. </summary>
		[Description("Run")]
		Run = 1,
	}
	/// <summary>RTD sensor type
	/// </summary>
	public enum RTDType : int {
		/// <summary> Configures the RTD type as a PT100 with a 3850ppm curve. </summary>
		[Description("PT100 3850")]
		PT100_3850 = 1,
		/// <summary> Configures the RTD type as a PT1000 with a 3850ppm curve. </summary>
		[Description("PT1000 3850")]
		PT1000_3850 = 2,
		/// <summary> Configures the RTD type as a PT100 with a 3920ppm curve. </summary>
		[Description("PT100 3920")]
		PT100_3920 = 3,
		/// <summary> Configures the RTD type as a PT1000 with a 3920ppm curve. </summary>
		[Description("PT1000 3920")]
		PT1000_3920 = 4,
	}
	/// <summary>The type of thermocouple being used
	/// </summary>
	public enum ThermocoupleType : int {
		/// <summary> Configures the thermocouple input as a J-Type thermocouple. </summary>
		[Description("J-Type")]
		J = 1,
		/// <summary> Configures the thermocouple input as a K-Type thermocouple. </summary>
		[Description("K-Type")]
		K = 2,
		/// <summary> Configures the thermocouple input as a E-Type thermocouple. </summary>
		[Description("E-Type")]
		E = 3,
		/// <summary> Configures the thermocouple input as a T-Type thermocouple. </summary>
		[Description("T-Type")]
		T = 4,
	}
	/// <summary>Measurement range of the voltage input. Larger ranges have less resolution.
	/// </summary>
	public enum VoltageRange : int {
		/// <summary> Range ±10mV DC </summary>
		[Description("10 mV")]
		MilliVolts_10 = 1,
		/// <summary> Range ±40mV DC </summary>
		[Description("40 mV")]
		MilliVolts_40 = 2,
		/// <summary> Range ±200mV DC </summary>
		[Description("200 mV")]
		MilliVolts_200 = 3,
		/// <summary> Range ±312.5mV DC </summary>
		[Description("312.5 mV")]
		MilliVolts_312_5 = 4,
		/// <summary> Range ±400mV DC </summary>
		[Description("400 mV")]
		MilliVolts_400 = 5,
		/// <summary> Range ±1000mV DC </summary>
		[Description("1000 mV")]
		MilliVolts_1000 = 6,
		/// <summary> Range ±2V DC </summary>
		[Description("2 V")]
		Volts_2 = 7,
		/// <summary> Range ±5V DC </summary>
		[Description("5 V")]
		Volts_5 = 8,
		/// <summary> Range ±15V DC </summary>
		[Description("15 V")]
		Volts_15 = 9,
		/// <summary> Range ±40V DC </summary>
		[Description("40 V")]
		Volts_40 = 10,
		/// <summary> Auto-range mode changes based on the present voltage measurements. </summary>
		[Description("Auto")]
		Auto = 11,
	}
	/// <summary>Type of sensor attached to the voltage input
	/// </summary>
	public enum VoltageSensorType : int {
		/// <summary> Default. Configures the channel to be a generic voltage sensor. Unit is volts. </summary>
		[Description("Generic voltage sensor")]
		Voltage = 0,
		/// <summary> 1114 - Temperature Sensor </summary>
		[Description("1114 - Temperature Sensor")]
		PN_1114 = 11140,
		/// <summary> 1117 - Voltage Sensor </summary>
		[Description("1117 - Voltage Sensor")]
		PN_1117 = 11170,
		/// <summary> 1123 - Precision Voltage Sensor </summary>
		[Description("1123 - Precision Voltage Sensor")]
		PN_1123 = 11230,
		/// <summary> 1127 - Precision Light Sensor </summary>
		[Description("1127 - Precision Light Sensor")]
		PN_1127 = 11270,
		/// <summary> 1130 - pH Adapter </summary>
		[Description("1130 - pH Adapter")]
		PN_1130_pH = 11301,
		/// <summary> 1130 - ORP Adapter </summary>
		[Description("1130 - ORP Adapter")]
		PN_1130_ORP = 11302,
		/// <summary> 1132 - 4-20mA Adapter </summary>
		[Description("1132 - 4-20mA Adapter")]
		PN_1132 = 11320,
		/// <summary> 1133 - Sound Sensor </summary>
		[Description("1133 - Sound Sensor")]
		PN_1133 = 11330,
		/// <summary> 1135 - Precision Voltage Sensor </summary>
		[Description("1135 - Precision Voltage Sensor")]
		PN_1135 = 11350,
		/// <summary> 1142 - Light Sensor 1000 lux </summary>
		[Description("1142 - Light Sensor 1000 lux")]
		PN_1142 = 11420,
		/// <summary> 1143 - Light Sensor 70000 lux </summary>
		[Description("1143 - Light Sensor 70000 lux")]
		PN_1143 = 11430,
		/// <summary> 3500 - AC Current Sensor 10Amp </summary>
		[Description("3500 - AC Current Sensor 10Amp")]
		PN_3500 = 35000,
		/// <summary> 3501 - AC Current Sensor 25Amp </summary>
		[Description("3501 - AC Current Sensor 25Amp")]
		PN_3501 = 35010,
		/// <summary> 3502 - AC Current Sensor 50Amp </summary>
		[Description("3502 - AC Current Sensor 50Amp")]
		PN_3502 = 35020,
		/// <summary> 3503 - AC Current Sensor 100Amp </summary>
		[Description("3503 - AC Current Sensor 100Amp")]
		PN_3503 = 35030,
		/// <summary> 3507 - AC Voltage Sensor 0-250V (50Hz) </summary>
		[Description("3507 - AC Voltage Sensor 0-250V (50Hz)")]
		PN_3507 = 35070,
		/// <summary> 3508 - AC Voltage Sensor 0-250V (60Hz) </summary>
		[Description("3508 - AC Voltage Sensor 0-250V (60Hz)")]
		PN_3508 = 35080,
		/// <summary> 3509 - DC Voltage Sensor 0-200V </summary>
		[Description("3509 - DC Voltage Sensor 0-200V")]
		PN_3509 = 35090,
		/// <summary> 3510 - DC Voltage Sensor 0-75V </summary>
		[Description("3510 - DC Voltage Sensor 0-75V")]
		PN_3510 = 35100,
		/// <summary> 3511 - DC Current Sensor 0-10mA </summary>
		[Description("3511 - DC Current Sensor 0-10mA")]
		PN_3511 = 35110,
		/// <summary> 3512 - DC Current Sensor 0-100mA </summary>
		[Description("3512 - DC Current Sensor 0-100mA")]
		PN_3512 = 35120,
		/// <summary> 3513 - DC Current Sensor 0-1A </summary>
		[Description("3513 - DC Current Sensor 0-1A")]
		PN_3513 = 35130,
		/// <summary> 3514 - AC Active Power Sensor 0-250V*0-30A (50Hz) </summary>
		[Description("3514 - AC Active Power Sensor 0-250V*0-30A (50Hz)")]
		PN_3514 = 35140,
		/// <summary> 3515 - AC Active Power Sensor 0-250V*0-30A (60Hz) </summary>
		[Description("3515 - AC Active Power Sensor 0-250V*0-30A (60Hz)")]
		PN_3515 = 35150,
		/// <summary> 3516 - AC Active Power Sensor 0-250V*0-5A (50Hz) </summary>
		[Description("3516 - AC Active Power Sensor 0-250V*0-5A (50Hz)")]
		PN_3516 = 35160,
		/// <summary> 3517 - AC Active Power Sensor 0-250V*0-5A (60Hz) </summary>
		[Description("3517 - AC Active Power Sensor 0-250V*0-5A (60Hz)")]
		PN_3517 = 35170,
		/// <summary> 3518 - AC Active Power Sensor 0-110V*0-5A (60Hz) </summary>
		[Description("3518 - AC Active Power Sensor 0-110V*0-5A (60Hz)")]
		PN_3518 = 35180,
		/// <summary> 3519 - AC Active Power Sensor 0-110V*0-15A (60Hz) </summary>
		[Description("3519 - AC Active Power Sensor 0-110V*0-15A (60Hz)")]
		PN_3519 = 35190,
		/// <summary> 3584 - 0-50A DC Current Transducer </summary>
		[Description("3584 - 0-50A DC Current Transducer")]
		PN_3584 = 35840,
		/// <summary> 3585 - 0-100A DC Current Transducer </summary>
		[Description("3585 - 0-100A DC Current Transducer")]
		PN_3585 = 35850,
		/// <summary> 3586 - 0-250A DC Current Transducer </summary>
		[Description("3586 - 0-250A DC Current Transducer")]
		PN_3586 = 35860,
		/// <summary> 3587 - +-50A DC Current Transducer </summary>
		[Description("3587 - +-50A DC Current Transducer")]
		PN_3587 = 35870,
		/// <summary> 3588 - +-100A DC Current Transducer </summary>
		[Description("3588 - +-100A DC Current Transducer")]
		PN_3588 = 35880,
		/// <summary> 3589 - +-250A DC Current Transducer </summary>
		[Description("3589 - +-250A DC Current Transducer")]
		PN_3589 = 35890,
	}
	/// <summary>The selected output voltage range
	/// </summary>
	public enum VoltageOutputRange : int {
		/// <summary> ±10V DC </summary>
		[Description("±10V DC")]
		Volts_10 = 1,
		/// <summary> 0-5V DC </summary>
		[Description("0-5V DC")]
		Volts_5 = 2,
	}
	/// <summary>Bridge gain amplification setting. Higher gain results in better resolution, but narrower voltage
	/// range.
	/// </summary>
	public enum BridgeGain : int {
		/// <summary> 1x Amplificaion </summary>
		[Description("1x")]
		Gain_1x = 1,
		/// <summary> 2x Amplification </summary>
		[Description("2x")]
		Gain_2x = 2,
		/// <summary> 4x Amplification </summary>
		[Description("4x")]
		Gain_4x = 3,
		/// <summary> 8x Amplification </summary>
		[Description("8x")]
		Gain_8x = 4,
		/// <summary> 16x Amplification </summary>
		[Description("16x")]
		Gain_16x = 5,
		/// <summary> 32x Amplification </summary>
		[Description("32x")]
		Gain_32x = 6,
		/// <summary> 64x Amplification </summary>
		[Description("64x")]
		Gain_64x = 7,
		/// <summary> 128x Amplification </summary>
		[Description("128x")]
		Gain_128x = 8,
	}
	/// <summary>The type of sensor attached to the voltage ratio input
	/// </summary>
	public enum VoltageRatioSensorType : int {
		/// <summary> Default. Configures the channel to be a generic ratiometric sensor. Unit is volts/volt. </summary>
		[Description("Generic ratiometric sensor")]
		VoltageRatio = 0,
		/// <summary> 1101 - IR Distance Adapter, with Sharp Distance Sensor 2D120X (4-30cm) </summary>
		[Description("1101 - IR Distance Adapter, with Sharp Distance Sensor 2D120X (4-30cm)")]
		PN_1101_Sharp2D120X = 11011,
		/// <summary> 1101 - IR Distance Adapter, with Sharp Distance Sensor 2Y0A21 (10-80cm) </summary>
		[Description("1101 - IR Distance Adapter, with Sharp Distance Sensor 2Y0A21 (10-80cm)")]
		PN_1101_Sharp2Y0A21 = 11012,
		/// <summary> 1101 - IR Distance Adapter, with Sharp Distance Sensor 2Y0A02 (20-150cm) </summary>
		[Description("1101 - IR Distance Adapter, with Sharp Distance Sensor 2Y0A02 (20-150cm)")]
		PN_1101_Sharp2Y0A02 = 11013,
		/// <summary> 1102 - IR Reflective Sensor 5mm </summary>
		[Description("1102 - IR Reflective Sensor 5mm")]
		PN_1102 = 11020,
		/// <summary> 1103 - IR Reflective Sensor 10cm </summary>
		[Description("1103 - IR Reflective Sensor 10cm")]
		PN_1103 = 11030,
		/// <summary> 1104 - Vibration Sensor </summary>
		[Description("1104 - Vibration Sensor")]
		PN_1104 = 11040,
		/// <summary> 1105 - Light Sensor </summary>
		[Description("1105 - Light Sensor")]
		PN_1105 = 11050,
		/// <summary> 1106 - Force Sensor </summary>
		[Description("1106 - Force Sensor")]
		PN_1106 = 11060,
		/// <summary> 1107 - Humidity Sensor </summary>
		[Description("1107 - Humidity Sensor")]
		PN_1107 = 11070,
		/// <summary> 1108 - Magnetic Sensor </summary>
		[Description("1108 - Magnetic Sensor")]
		PN_1108 = 11080,
		/// <summary> 1109 - Rotation Sensor </summary>
		[Description("1109 - Rotation Sensor")]
		PN_1109 = 11090,
		/// <summary> 1110 - Touch Sensor </summary>
		[Description("1110 - Touch Sensor")]
		PN_1110 = 11100,
		/// <summary> 1111 - Motion Sensor </summary>
		[Description("1111 - Motion Sensor")]
		PN_1111 = 11110,
		/// <summary> 1112 - Slider 60 </summary>
		[Description("1112 - Slider 60")]
		PN_1112 = 11120,
		/// <summary> 1113 - Mini Joy Stick Sensor </summary>
		[Description("1113 - Mini Joy Stick Sensor")]
		PN_1113 = 11130,
		/// <summary> 1115 - Pressure Sensor </summary>
		[Description("1115 - Pressure Sensor")]
		PN_1115 = 11150,
		/// <summary> 1116 - Multi-turn Rotation Sensor </summary>
		[Description("1116 - Multi-turn Rotation Sensor")]
		PN_1116 = 11160,
		/// <summary> 1118 - 50Amp Current Sensor AC </summary>
		[Description("1118 - 50Amp Current Sensor AC")]
		PN_1118_AC = 11181,
		/// <summary> 1118 - 50Amp Current Sensor DC </summary>
		[Description("1118 - 50Amp Current Sensor DC")]
		PN_1118_DC = 11182,
		/// <summary> 1119 - 20Amp Current Sensor AC </summary>
		[Description("1119 - 20Amp Current Sensor AC")]
		PN_1119_AC = 11191,
		/// <summary> 1119 - 20Amp Current Sensor DC </summary>
		[Description("1119 - 20Amp Current Sensor DC")]
		PN_1119_DC = 11192,
		/// <summary> 1120 - FlexiForce Adapter </summary>
		[Description("1120 - FlexiForce Adapter")]
		PN_1120 = 11200,
		/// <summary> 1121 - Voltage Divider </summary>
		[Description("1121 - Voltage Divider")]
		PN_1121 = 11210,
		/// <summary> 1122 - 30 Amp Current Sensor AC </summary>
		[Description("1122 - 30 Amp Current Sensor AC")]
		PN_1122_AC = 11221,
		/// <summary> 1122 - 30 Amp Current Sensor DC </summary>
		[Description("1122 - 30 Amp Current Sensor DC")]
		PN_1122_DC = 11222,
		/// <summary> 1124 - Precision Temperature Sensor </summary>
		[Description("1124 - Precision Temperature Sensor")]
		PN_1124 = 11240,
		/// <summary> 1125 - Humidity Sensor </summary>
		[Description("1125 - Humidity Sensor")]
		PN_1125_Humidity = 11251,
		/// <summary> 1125 - Temperature Sensor </summary>
		[Description("1125 - Temperature Sensor")]
		PN_1125_Temperature = 11252,
		/// <summary> 1126 - Differential Air Pressure Sensor +- 25kPa </summary>
		[Description("1126 - Differential Air Pressure Sensor +- 25kPa")]
		PN_1126 = 11260,
		/// <summary> 1128 - MaxBotix EZ-1 Sonar Sensor </summary>
		[Description("1128 - MaxBotix EZ-1 Sonar Sensor")]
		PN_1128 = 11280,
		/// <summary> 1129 - Touch Sensor </summary>
		[Description("1129 - Touch Sensor")]
		PN_1129 = 11290,
		/// <summary> 1131 - Thin Force Sensor </summary>
		[Description("1131 - Thin Force Sensor")]
		PN_1131 = 11310,
		/// <summary> 1134 - Switchable Voltage Divider </summary>
		[Description("1134 - Switchable Voltage Divider")]
		PN_1134 = 11340,
		/// <summary> 1136 - Differential Air Pressure Sensor +-2 kPa </summary>
		[Description("1136 - Differential Air Pressure Sensor +-2 kPa")]
		PN_1136 = 11360,
		/// <summary> 1137 - Differential Air Pressure Sensor +-7 kPa </summary>
		[Description("1137 - Differential Air Pressure Sensor +-7 kPa")]
		PN_1137 = 11370,
		/// <summary> 1138 - Differential Air Pressure Sensor 50 kPa </summary>
		[Description("1138 - Differential Air Pressure Sensor 50 kPa")]
		PN_1138 = 11380,
		/// <summary> 1139 - Differential Air Pressure Sensor 100 kPa </summary>
		[Description("1139 - Differential Air Pressure Sensor 100 kPa")]
		PN_1139 = 11390,
		/// <summary> 1140 - Absolute Air Pressure Sensor 20-400 kPa </summary>
		[Description("1140 - Absolute Air Pressure Sensor 20-400 kPa")]
		PN_1140 = 11400,
		/// <summary> 1141 - Absolute Air Pressure Sensor 15-115 kPa </summary>
		[Description("1141 - Absolute Air Pressure Sensor 15-115 kPa")]
		PN_1141 = 11410,
		/// <summary> 1146 - IR Reflective Sensor 1-4mm </summary>
		[Description("1146 - IR Reflective Sensor 1-4mm")]
		PN_1146 = 11460,
		/// <summary> 3120 - Compression Load Cell (0-4.5 kg) </summary>
		[Description("3120 - Compression Load Cell (0-4.5 kg)")]
		PN_3120 = 31200,
		/// <summary> 3121 - Compression Load Cell (0-11.3 kg) </summary>
		[Description("3121 - Compression Load Cell (0-11.3 kg)")]
		PN_3121 = 31210,
		/// <summary> 3122 - Compression Load Cell (0-22.7 kg) </summary>
		[Description("3122 - Compression Load Cell (0-22.7 kg)")]
		PN_3122 = 31220,
		/// <summary> 3123 - Compression Load Cell (0-45.3 kg) </summary>
		[Description("3123 - Compression Load Cell (0-45.3 kg)")]
		PN_3123 = 31230,
		/// <summary> 3130 - Relative Humidity Sensor </summary>
		[Description("3130 - Relative Humidity Sensor")]
		PN_3130 = 31300,
		/// <summary> 3520 - Sharp Distance Sensor (4-30cm) </summary>
		[Description("3520 - Sharp Distance Sensor (4-30cm)")]
		PN_3520 = 35200,
		/// <summary> 3521 - Sharp Distance Sensor (10-80cm) </summary>
		[Description("3521 - Sharp Distance Sensor (10-80cm)")]
		PN_3521 = 35210,
		/// <summary> 3522 - Sharp Distance Sensor (20-150cm) </summary>
		[Description("3522 - Sharp Distance Sensor (20-150cm)")]
		PN_3522 = 35220,
	}
	/// <summary>LogRotating contains the returns values from GetRotating()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LogRotating {
		/// <summary> The file size above which the log file should be rotated. </summary>
		public long Size;
		/// <summary> The number of log files that will be kept after rotation. </summary>
		public int KeepCount;
	}
	/// <summary>RFIDTag contains the returns values from GetLastTag()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RFIDTag {
		/// <summary> The data stored on the most recently read tag </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string TagString;
		/// <summary> Protocol of the most recently read tag </summary>
		public RFIDProtocol Protocol;
	}
	/// <summary>IRCode contains the returns values from GetLastCode()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct IRCode {
		/// <summary> The last received code </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Code;
		/// <summary> length of the received code in bits </summary>
		public int BitCount;
	}
	/// <summary>IRLearnedCode contains the returns values from GetLastLearnedCode()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct IRLearnedCode {
		/// <summary> The last learned code </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string Code;
		/// <summary> contains the data for characterizing the code </summary>
		public IRCodeInfo CodeInfo;
	}
	/// <summary>LCDFontSize contains the returns values from GetFontSize()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LCDFontSize {
		/// <summary> The width of the font </summary>
		public int Width;
		/// <summary> The height of the font </summary>
		public int Height;
	}
	/// <summary>DistanceSensorSonarReflections contains the returns values from GetSonarReflections()
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DistanceSensorSonarReflections {
		/// <summary> The reflection values </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] Distances;
		/// <summary> The amplitude values </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] Amplitudes;
		/// <summary> The number of reflections </summary>
		public int Count;
	}
}
