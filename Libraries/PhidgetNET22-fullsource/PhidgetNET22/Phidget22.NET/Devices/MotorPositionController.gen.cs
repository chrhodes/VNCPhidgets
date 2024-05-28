using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> MotorPositionController class definition </summary>
	public partial class MotorPositionController : Phidget {
		#region Constructor/Destructor
		/// <summary> MotorPositionController Constructor </summary>
		public MotorPositionController() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMotorPositionController_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> MotorPositionController Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~MotorPositionController() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetMotorPositionController_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The acceleration value </summary>
		/// <remarks>The rate at which the controller can change the motor's velocity.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Acceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setAcceleration(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The acceleration value. </summary>
		/// <remarks>The minimum value that <c>Acceleration</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The acceleration value. </summary>
		/// <remarks>The maximum value that <c>Acceleration</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Motor current limit </summary>
		/// <remarks>The controller will limit the current through the motor to this value.
		/// </remarks>
		public double CurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setCurrentLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> Minimum current limit </summary>
		/// <remarks>The minimum current limit that can be set for the device.
		/// </remarks>
		public double MinCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Maximum current limit </summary>
		/// <remarks>The maximum current limit that can be set for the device.
		/// </remarks>
		public double MaxCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Current Regulator Gain </summary>
		/// <remarks>Depending on power supply voltage and motor coil inductance, current through the motor can change
		/// relatively slowly or extremely rapidly. A physically larger DC Motor will typically have a lower
		/// inductance, requiring a higher current regulator gain. A higher power supply voltage will result in
		/// motor current changing more rapidly, requiring a higher current regulator gain. If the current
		/// regulator gain is too small, spikes in current will occur, causing large variations in torque, and
		/// possibly damaging the motor controller. If the current regulator gain is too high, the current will
		/// jitter, causing the motor to sound 'rough', especially when changing directions. Each DC Motor we
		/// sell specifies a suitable current regulator gain.
		/// </remarks>
		public double CurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setCurrentRegulatorGain(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> Minimum current regulator gain </summary>
		/// <remarks>The minimum current regulator gain for the device.
		/// </remarks>
		public double MinCurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Maximum current regulator gain </summary>
		/// <remarks>The maximum current regulator gain for the device.
		/// </remarks>
		public double MaxCurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the controller will fire another
		/// <c>CurrentChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>The timing between <c>CurrentChange</c> events can also affected by the
		/// <c>CurrentChangeTrigger</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetMotorPositionController_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setDataInterval(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The minimum value that <c>DataInterval</c> can be set to.
		/// </remarks>
		public int MinDataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The maximum value that <c>DataInterval</c> can be set to.
		/// </remarks>
		public int MaxDataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The dead band value </summary>
		/// <remarks>Depending on your system, it may not be possible to bring the position error
		/// (<c>TargetPosition</c> - <c>Position</c>) to zero. A small error can lead to the motor
		/// continually 'hunting' for a target position, which can cause unwanted effects. By setting a
		/// non-zero <c>DeadBand</c>, the position controller will relax control of the motor within the
		/// deadband, preventing the 'hunting' behavior.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double DeadBand {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getDeadBand(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setDeadBand(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The duty cycle value. </summary>
		/// <remarks>The most recent duty cycle value that the controller has reported.
		/// <list>
		/// <item>This value will be between -1 and 1 where a sign change (±) is indicitave of a direction
		/// change.</item>
		/// <item>Note that <c>DutyCycle</c> is merely an indication of the average voltage across the
		/// motor. At a constant load, an increase in <c>DutyCycle</c> indicates an increase in motor
		/// speed.</item>
		/// <item>The units of <c>DutyCycle</c> refer to 'duty cycle'. This is because the controller must
		/// rapidly switch the power on/off (i.e. change the duty cycle) in order to manipulate the voltage
		/// across the motor.</item>
		/// </list>
		/// 
		/// </remarks>
		public double DutyCycle {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getDutyCycle(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The engaged value. </summary>
		/// <remarks>When engaged, a motor has the ability to be positioned. When disengaged, no commands are sent to
		/// the motor.
		/// <list>
		/// <item>This function is useful for completely relaxing a motor once it has reached the target
		/// position.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Engaged {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetMotorPositionController_getEngaged(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setEngaged(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The fan mode </summary>
		/// <remarks>The <c>FanMode</c> dictates the operating condition of the fan.
		/// <list>
		/// <item>Choose between on, off, or automatic (based on temperature).</item>
		/// <item>If the <c>FanMode</c> is set to automatic, the fan will turn on when the temperature
		/// reaches 70°C and it will remain on until the temperature falls below 55°C.</item>
		/// <item>If the <c>FanMode</c> is off, the controller will still turn on the fan if the
		/// temperature reaches 85°C and it will remain on until it falls below 70°C.</item>
		/// </list>
		/// 
		/// </remarks>
		public FanMode FanMode {
			get {
				ErrorCode result;
				FanMode val;
				result = Phidget22Imports.PhidgetMotorPositionController_getFanMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setFanMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The IO mode value. </summary>
		/// <remarks>The encoder interface mode. Match the mode to the type of encoder you have attached.
		/// <list>
		/// <item>It is recommended to only change this when the encoder disabled in order to avoid unexpected
		/// results.</item>
		/// </list>
		/// 
		/// </remarks>
		public EncoderIOMode IOMode {
			get {
				ErrorCode result;
				EncoderIOMode val;
				result = Phidget22Imports.PhidgetMotorPositionController_getIOMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setIOMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The Kd value. </summary>
		/// <remarks>Derivative gain constant. A higher <c>Kd</c> will help reduce oscillations.
		/// </remarks>
		public double Kd {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getKd(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setKd(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The Ki value. </summary>
		/// <remarks>Integral gain constant. The integral term will help eliminate steady-state error.
		/// </remarks>
		public double Ki {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getKi(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setKi(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The Kp value. </summary>
		/// <remarks>Proportional gain constant. A small <c>Kp</c> value will result in a less responsive
		/// controller, however, if <c>Kp</c> is too high, the system can become unstable.
		/// </remarks>
		public double Kp {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getKp(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setKp(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The most recent position value that the controller has reported.
		/// <list>
		/// <item>This value will always be between <c>MinPosition</c> and <c>MaxPosition</c>.</item>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Position {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The minimum value that <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The maximum value that <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The rescale factor value </summary>
		/// <remarks>Change the units of your parameters so that your application is more intuitive.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double RescaleFactor {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getRescaleFactor(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setRescaleFactor(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The stall velocity value. </summary>
		/// <remarks>Before reading this description, it is important to note the difference between the units of
		/// <c>StallVelocity</c> and <c>Velocity</c>.
		/// <list>
		/// <item><c>Velocity</c> is a number between -1 and 1 with units of 'duty cycle'. It simply
		/// represents the average voltage across the motor.</item>
		/// <item><c>StallVelocity</c> represents a real velocity (e.g. m/s, RPM, etc.) and the units are
		/// determined by the <c>RescaleFactor</c>. With a <c>RescaleFactor</c> of 1, the default
		/// units would be in commutations per second.</item>
		/// </list>
		/// If the load on your motor is large, your motor may begin rotating more slowly, or even fully stall.
		/// Depending on the voltage across your motor, this may result in a large amount of current through
		/// both the controller and the motor. In order to prevent damage in these situations, you can use the
		/// <c>StallVelocity</c> property.
		/// 
		/// The <c>StallVelocity</c> should be set to the lowest velocity you would expect from your
		/// motor. The controller will then monitor the motor's velocity, as well as the <c>Velocity</c>,
		/// and prevent a 'dangerous stall' from occuring. If the controller detects a dangerous stall, it will
		/// immediately disengage the motor (i.e. <c>Engaged</c> will be set to false) and an error will
		/// be reported to your program.
		/// <list>
		/// <item>A 'dangerous stall' will occur faster when the <c>Velocity</c> is higher (i.e. when the
		/// average voltage across the motor is higher)</item>
		/// <item>A 'dangerous stall' will occur faster as (<c>StallVelocity</c> - motor velocity) becomes
		/// larger .</item>
		/// </list>
		/// Setting <c>StallVelocity</c> to 0 will turn off stall protection functionality.
		/// </remarks>
		public double StallVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getStallVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setStallVelocity(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The lower bound of <c>StallVelocity</c>.
		/// </remarks>
		public double MinStallVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinStallVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The upper bound of <c>StallVelocity</c>.
		/// </remarks>
		public double MaxStallVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxStallVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>If the controller is configured and the <c>TargetPosition</c> is set, the motor will try to
		/// reach the <c>TargetPostiion</c>.
		/// <list>
		/// <item>If the <c>DeadBand</c> is non-zero, the final position of the motor may not match the
		/// <c>TargetPosition</c></item>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getTargetPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setTargetPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value. </summary>
		/// <remarks>When moving, the motor velocity will be limited by this value.
		/// <list>
		/// <item><c>VelocityLimit</c> is bounded by <c>MinVelocityLimit</c> and
		/// <c>MaxVelocityLimit</c>.</item>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double VelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setVelocityLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The minimum value that <c>VelocityLimit</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMinVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The maximum value that <c>VelocityLimit</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetMotorPositionController_getMaxVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Adds an offset (positive or negative) to the current position. Useful for zeroing position.
		/// </remarks>
		public void AddPositionOffset(double positionOffset) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetMotorPositionController_addPositionOffset(chandle, positionOffset);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary>The position value </summary>
		/// <remarks>If the controller is configured and the <c>TargetPosition</c> is set, the motor will try to
		/// reach the <c>TargetPostiion</c>.
		/// <list>
		/// <item>If the <c>DeadBand</c> is non-zero, the final position of the motor may not match the
		/// <c>TargetPosition</c></item>
		/// <item>Units for <c>Position</c>, <c>VelocityLimit</c>, <c>Acceleration</c>, and
		/// <c>DeadBand</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetTargetPosition(double targetPosition, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetTargetPosition");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetMotorPositionController_setTargetPosition_async(chandle, targetPosition, asyncResult.cCallbackDelegate, IntPtr.Zero);
				if (result != 0)
					asyncResult.Complete(PhidgetException.CreateByCode(result), true);
			} catch (Exception ex) {
				asyncResult.Complete(ex, true);
			}
			return asyncResult;
		}
		/// <summary> Checks the result obtained from the corresponding async Begin function </summary>
		/// <param name="result">IAsyncResult object returned from BeginSetTargetPosition</param>
		public void EndSetTargetPosition(IAsyncResult result) {
			AsyncResultNoResult.End(result, this, "SetTargetPosition");
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeDutyCycleUpdateEventCallback = new Phidget22Imports.MotorPositionControllerDutyCycleUpdateEvent(nativeDutyCycleUpdateEvent);
			result = Phidget22Imports.PhidgetMotorPositionController_setOnDutyCycleUpdateHandler(chandle, nativeDutyCycleUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativePositionChangeEventCallback = new Phidget22Imports.MotorPositionControllerPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetMotorPositionController_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetMotorPositionController_setOnDutyCycleUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetMotorPositionController_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent duty cycle value will be reported in this event, which occurs when the
		/// <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>This event will <b>always</b> occur when the <c>DataInterval</c> elapses. You can depend
		/// on this event for constant timing when implementing control loops in code. This is the last event
		/// to fire, giving you up-to-date access to all properties.</item>
		/// </list>
		/// 
		/// </remarks>
		public event MotorPositionControllerDutyCycleUpdateEventHandler DutyCycleUpdate;
		internal void OnDutyCycleUpdate(MotorPositionControllerDutyCycleUpdateEventArgs e) {
			if (DutyCycleUpdate != null) {
				foreach (MotorPositionControllerDutyCycleUpdateEventHandler DutyCycleUpdateHandler in DutyCycleUpdate.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = DutyCycleUpdateHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(DutyCycleUpdateHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						DutyCycleUpdateHandler(this, e);
				}
			}
		}
		Phidget22Imports.MotorPositionControllerDutyCycleUpdateEvent nativeDutyCycleUpdateEventCallback;
		internal void nativeDutyCycleUpdateEvent(IntPtr phid, IntPtr ctx, double dutyCycle) {
			OnDutyCycleUpdate(new MotorPositionControllerDutyCycleUpdateEventArgs(dutyCycle));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent position value will be reported in this event, which occurs when the
		/// <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>Regardless of the <c>DataInterval</c>, this event will occur only when the position value
		/// has changed from the previous value reported.</item>
		/// <item>Units for <c>Position</c> can be set by the user through the <c>RescaleFactor</c>.
		/// The <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public event MotorPositionControllerPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(MotorPositionControllerPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (MotorPositionControllerPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = PositionChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(PositionChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						PositionChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.MotorPositionControllerPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, double position) {
			OnPositionChange(new MotorPositionControllerPositionChangeEventArgs(position));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_addPositionOffset(IntPtr phid, double positionOffset);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setTargetPosition_async(IntPtr phid, double TargetPosition, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getAcceleration(IntPtr phid, out double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setAcceleration(IntPtr phid, double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinAcceleration(IntPtr phid, out double MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxAcceleration(IntPtr phid, out double MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getCurrentLimit(IntPtr phid, out double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setCurrentLimit(IntPtr phid, double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinCurrentLimit(IntPtr phid, out double MinCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxCurrentLimit(IntPtr phid, out double MaxCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getCurrentRegulatorGain(IntPtr phid, out double CurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setCurrentRegulatorGain(IntPtr phid, double CurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinCurrentRegulatorGain(IntPtr phid, out double MinCurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxCurrentRegulatorGain(IntPtr phid, out double MaxCurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getDeadBand(IntPtr phid, out double DeadBand);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setDeadBand(IntPtr phid, double DeadBand);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getDutyCycle(IntPtr phid, out double DutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getEngaged(IntPtr phid, out bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setEngaged(IntPtr phid, bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getFanMode(IntPtr phid, out FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setFanMode(IntPtr phid, FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getIOMode(IntPtr phid, out EncoderIOMode IOMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setIOMode(IntPtr phid, EncoderIOMode IOMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getKd(IntPtr phid, out double Kd);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setKd(IntPtr phid, double Kd);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getKi(IntPtr phid, out double Ki);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setKi(IntPtr phid, double Ki);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getKp(IntPtr phid, out double Kp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setKp(IntPtr phid, double Kp);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getPosition(IntPtr phid, out double Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinPosition(IntPtr phid, out double MinPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxPosition(IntPtr phid, out double MaxPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getRescaleFactor(IntPtr phid, out double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setRescaleFactor(IntPtr phid, double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getStallVelocity(IntPtr phid, out double StallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setStallVelocity(IntPtr phid, double StallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinStallVelocity(IntPtr phid, out double MinStallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxStallVelocity(IntPtr phid, out double MaxStallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getTargetPosition(IntPtr phid, out double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setTargetPosition(IntPtr phid, double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getVelocityLimit(IntPtr phid, out double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setVelocityLimit(IntPtr phid, double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMinVelocityLimit(IntPtr phid, out double MinVelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_getMaxVelocityLimit(IntPtr phid, out double MaxVelocityLimit);
		public delegate void MotorPositionControllerDutyCycleUpdateEvent(IntPtr phid, IntPtr ctx, double dutyCycle);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setOnDutyCycleUpdateHandler(IntPtr phid, MotorPositionControllerDutyCycleUpdateEvent fptr, IntPtr ctx);
		public delegate void MotorPositionControllerPositionChangeEvent(IntPtr phid, IntPtr ctx, double position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetMotorPositionController_setOnPositionChangeHandler(IntPtr phid, MotorPositionControllerPositionChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A MotorPositionController DutyCycleUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A MotorPositionControllerDutyCycleUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void MotorPositionControllerDutyCycleUpdateEventHandler(object sender, MotorPositionControllerDutyCycleUpdateEventArgs e);
	/// <summary> MotorPositionController DutyCycleUpdate Event data </summary>
	public class MotorPositionControllerDutyCycleUpdateEventArgs : EventArgs {
		/// <summary>The duty cycle value
		/// </summary>
		public readonly double DutyCycle;
		internal MotorPositionControllerDutyCycleUpdateEventArgs(double dutyCycle) {
			this.DutyCycle = dutyCycle;
		}
	}

	/// <summary> A MotorPositionController PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A MotorPositionControllerPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void MotorPositionControllerPositionChangeEventHandler(object sender, MotorPositionControllerPositionChangeEventArgs e);
	/// <summary> MotorPositionController PositionChange Event data </summary>
	public class MotorPositionControllerPositionChangeEventArgs : EventArgs {
		/// <summary>The position value
		/// </summary>
		public readonly double Position;
		internal MotorPositionControllerPositionChangeEventArgs(double position) {
			this.Position = position;
		}
	}

}
