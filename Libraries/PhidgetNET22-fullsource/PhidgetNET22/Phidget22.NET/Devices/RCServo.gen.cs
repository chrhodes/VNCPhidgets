using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> RCServo class definition </summary>
	public partial class RCServo : Phidget {
		#region Constructor/Destructor
		/// <summary> RCServo Constructor </summary>
		public RCServo() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetRCServo_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> RCServo Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~RCServo() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetRCServo_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The acceleration value </summary>
		/// <remarks>When changing velocity, the RC servo motor will accelerate/decelerate at this rate.
		/// <list>
		/// <item>The acceleration is bounded by <c>MaxAcceleration</c> and
		/// <c>MinAcceleration</c>.</item>
		/// <item><c>SpeedRampingState</c> controls whether or not the acceleration value is actually
		/// applied when trying to reach a target position.</item>
		/// <item>There is a practical limit on how fast your RC servo motor can accelerate. This is based on the
		/// load and physical design of the motor.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Acceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setAcceleration(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The acceleration value </summary>
		/// <remarks>The minimum value that <c>Acceleration</c> can be set to
		/// <list>
		/// <item>This value depends on <c>MinPosition</c>/<c>MaxPosition</c> and
		/// <c>MinPulseWidth</c>/<c>MaxPulseWidth</c></item>
		/// <item style="list-style: none">.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The acceleration value </summary>
		/// <remarks>The maximum acceleration that <c>Acceleration</c> can be set to.
		/// <list>
		/// <item>This value depends on <c>MinPosition</c>/<c>MaxPosition</c> and
		/// <c>MinPulseWidth</c>/<c>MaxPulseWidth</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the controller will fire another
		/// <c>PositionChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetRCServo_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetRCServo_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetRCServo_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The engaged value </summary>
		/// <remarks>When engaged, a RC servo motor has the ability to be positioned. When disengaged, no commands are
		/// sent to the RC servo motor.
		/// <list>
		/// <item>There is no position feedback to the controller, so the RC servo motor will immediately snap to
		/// the <c>TargetPosition</c> after being engaged from a disengaged state.</item>
		/// <item>This property is useful for relaxing a servo once it has reached a given position.</item>
		/// <item>If you are concerned about tracking position accurately, you should not disengage the motor
		/// while <c>IsMoving</c> is true.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Engaged {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetRCServo_getEngaged(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setEngaged(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The moving value </summary>
		/// <remarks><c>IsMoving</c> returns true if the RC servo motor is currently in motion.
		/// <list>
		/// <item>The controller cannot know if the RC servo motor is physically moving. When &lt; code &gt;
		/// IsMoving is false, it simply means there are no commands in the pipeline to the RC servo
		/// motor.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool IsMoving {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetRCServo_getIsMoving(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The most recent position of the RC servo motor that the controller has reported.
		/// <list>
		/// <item>This value will always be between <c>MinPosition</c> and <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Position {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The minimum position that <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setMinPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The maximum position <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setMaxPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The pulse width value </summary>
		/// <remarks>The <c>MinPulseWidth</c> represents the minimum pulse width that your RC servo motor
		/// specifies.
		/// <list>
		/// <item>This value can be found in the data sheet of most RC servo motors.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinPulseWidth {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinPulseWidth(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setMinPulseWidth(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The pulse width value </summary>
		/// <remarks>The <c>MaxPulseWidth</c> represents the maximum pulse width that your RC servo motor
		/// specifies.
		/// <list>
		/// <item>This value can be found in the data sheet of most RC servo motors.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxPulseWidth {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxPulseWidth(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setMaxPulseWidth(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The pulse width value </summary>
		/// <remarks>The minimum pulse width that <c>MinPulseWidth</c> can be set to.
		/// </remarks>
		public double MinPulseWidthLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinPulseWidthLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The pulse width value </summary>
		/// <remarks>The maximum pulse width that <c>MaxPulseWidth</c> can be set to.
		/// </remarks>
		public double MaxPulseWidthLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxPulseWidthLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The speed ramping state value </summary>
		/// <remarks>When speed ramping state is enabled, the controller will take the <c>Acceleration</c> and
		/// <c>Velocity</c> properties into account when moving the RC servo motor, usually resulting in
		/// smooth motion. If speed ramping state is not enabled, the controller will simply set the RC servo
		/// motor to the requested position.
		/// </remarks>
		public bool SpeedRampingState {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetRCServo_getSpeedRampingState(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setSpeedRampingState(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>If the RC servo motor is configured and <c>TargetPosition</c> is set, the controller will
		/// continuously try to reach targeted position.
		/// <list>
		/// <item>The target position is bounded by <c>MinPosition</c> and <c>MaxPosition</c>.</item>
		/// <item>If the RC servo motor is not engaged, then the position cannot be read.</item>
		/// <item>The position can still be set while the RC servo motor is not engaged. Once engaged, the RC
		/// servo motor will snap to position, assuming it is not there already.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getTargetPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setTargetPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The torque value. </summary>
		/// <remarks>The <c>Torque</c> is a ratio of the maximum available torque.
		/// <list>
		/// <item>The torque is bounded by <c>MinTorque</c> and <c>MaxTorque</c></item>
		/// <item>Increasing the torque will increase the speed and power consumption of the RC servo motor.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Torque {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getTorque(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setTorque(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The torque value </summary>
		/// <remarks>The minimum value that <c>Torque</c> can be set to.
		/// <list>
		/// <item><c>Torque</c> is a ratio of the maximum available torque, therefore the minimum torque is
		/// a unitless constant.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinTorque {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinTorque(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The torque value </summary>
		/// <remarks>The maximum value that <c>Torque</c> can be set to.
		/// <list>
		/// <item><c>Torque</c> is a ratio of the maximum available torque, therefore the minimum torque is
		/// a unitless constant.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxTorque {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxTorque(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The velocity that the RC servo motor is being driven at.
		/// <list>
		/// <item>A negative value means the RC servo motor is moving towards a lower position.</item>
		/// <item>The velocity range of the RC servo motor will be from <c>-VelocityLimit</c> to
		/// <c>VelocityLimit</c>, depending on direction.</item>
		/// <item>This is not the actual physical velocity of the RC servo motor.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Velocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>When moving, the RC servo motor velocity will be limited by this value.
		/// <list>
		/// <item>The velocity limit is bounded by <c>MinVelocityLimit</c> and
		/// <c>MaxVelocityLimit</c>.</item>
		/// <item><c>SpeedRampingState</c> controls whether or not the velocity limit value is actually
		/// applied when trying to reach a target position.</item>
		/// <item>The velocity range of the RC servo motor will be from <c>-VelocityLimit</c> to
		/// <c>VelocityLimit</c>, depending on direction.</item>
		/// <item>Note that when this value is set to 0, the RC servo motor will not move.</item>
		/// <item>There is a practical limit on how fast your servo can rotate, based on the physical design of
		/// the motor.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public double VelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setVelocityLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The minimum velocity <c>VelocityLimit</c> can be set to.
		/// </remarks>
		public double MinVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMinVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The maximum velocity <c>VelocityLimit</c> can be set to. This value depends on
		/// <c>MinPosition</c>/<c>MaxPosition</c> and
		/// <c>MinPulseWidth</c>/<c>MaxPulseWidth</c>.
		/// </remarks>
		public double MaxVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetRCServo_getMaxVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The voltage value </summary>
		/// <remarks>The supply voltage for the RC servo motor.
		/// <list>
		/// <item>If your controller supports multiple RC servo motors, every motor will have the same supply
		/// voltage. It is not possible to set individual supply voltages.</item>
		/// </list>
		/// 
		/// </remarks>
		public RCServoVoltage Voltage {
			get {
				ErrorCode result;
				RCServoVoltage val;
				result = Phidget22Imports.PhidgetRCServo_getVoltage(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setVoltage(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Methods

		/// <summary>The position value </summary>
		/// <remarks>If the RC servo motor is configured and <c>TargetPosition</c> is set, the controller will
		/// continuously try to reach targeted position.
		/// <list>
		/// <item>The target position is bounded by <c>MinPosition</c> and <c>MaxPosition</c>.</item>
		/// <item>If the RC servo motor is not engaged, then the position cannot be read.</item>
		/// <item>The position can still be set while the RC servo motor is not engaged. Once engaged, the RC
		/// servo motor will snap to position, assuming it is not there already.</item>
		/// <item>The units for <c>Position</c>,<c>Velocity</c>, and <c>Acceleration</c> are
		/// configured by scaling the internal timing (set with <c>MinPulseWidth</c> and
		/// <c>MaxPulseWidth</c>) to a user specified range with <c>MinPosition</c> and
		/// <c>MaxPosition</c>.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetTargetPosition(double targetPosition, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetTargetPosition");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetRCServo_setTargetPosition_async(chandle, targetPosition, asyncResult.cCallbackDelegate, IntPtr.Zero);
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
			nativePositionChangeEventCallback = new Phidget22Imports.RCServoPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetRCServo_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeTargetPositionReachedEventCallback = new Phidget22Imports.RCServoTargetPositionReachedEvent(nativeTargetPositionReachedEvent);
			result = Phidget22Imports.PhidgetRCServo_setOnTargetPositionReachedHandler(chandle, nativeTargetPositionReachedEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVelocityChangeEventCallback = new Phidget22Imports.RCServoVelocityChangeEvent(nativeVelocityChangeEvent);
			result = Phidget22Imports.PhidgetRCServo_setOnVelocityChangeHandler(chandle, nativeVelocityChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetRCServo_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetRCServo_setOnTargetPositionReachedHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetRCServo_setOnVelocityChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>An event that occurs when the position changes on a RC servo motor.
		/// </remarks>
		public event RCServoPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(RCServoPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (RCServoPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
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
		Phidget22Imports.RCServoPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, double position) {
			OnPositionChange(new RCServoPositionChangeEventArgs(position));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the RC servo motor has reached the <c>TargetPosition</c>.
		/// <list>
		/// <item>The controller cannot know if the RC servo motor has physically reached the target position.
		/// When <c>TargetPosition</c> is reached, it simply means the controller pulse width output is
		/// matching its target.</item>
		/// </list>
		/// 
		/// </remarks>
		public event RCServoTargetPositionReachedEventHandler TargetPositionReached;
		internal void OnTargetPositionReached(RCServoTargetPositionReachedEventArgs e) {
			if (TargetPositionReached != null) {
				foreach (RCServoTargetPositionReachedEventHandler TargetPositionReachedHandler in TargetPositionReached.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = TargetPositionReachedHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(TargetPositionReachedHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						TargetPositionReachedHandler(this, e);
				}
			}
		}
		Phidget22Imports.RCServoTargetPositionReachedEvent nativeTargetPositionReachedEventCallback;
		internal void nativeTargetPositionReachedEvent(IntPtr phid, IntPtr ctx, double position) {
			OnTargetPositionReached(new RCServoTargetPositionReachedEventArgs(position));
		}
		/// <summary>  </summary>
		/// <remarks>An event that occurs when the velocity changes on a RC servo motor.
		/// </remarks>
		public event RCServoVelocityChangeEventHandler VelocityChange;
		internal void OnVelocityChange(RCServoVelocityChangeEventArgs e) {
			if (VelocityChange != null) {
				foreach (RCServoVelocityChangeEventHandler VelocityChangeHandler in VelocityChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = VelocityChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(VelocityChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						VelocityChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.RCServoVelocityChangeEvent nativeVelocityChangeEventCallback;
		internal void nativeVelocityChangeEvent(IntPtr phid, IntPtr ctx, double velocity) {
			OnVelocityChange(new RCServoVelocityChangeEventArgs(velocity));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setTargetPosition_async(IntPtr phid, double TargetPosition, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getAcceleration(IntPtr phid, out double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setAcceleration(IntPtr phid, double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinAcceleration(IntPtr phid, out double MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxAcceleration(IntPtr phid, out double MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getEngaged(IntPtr phid, out bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setEngaged(IntPtr phid, bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getIsMoving(IntPtr phid, out bool IsMoving);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getPosition(IntPtr phid, out double Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinPosition(IntPtr phid, out double MinPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setMinPosition(IntPtr phid, double MinPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxPosition(IntPtr phid, out double MaxPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setMaxPosition(IntPtr phid, double MaxPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinPulseWidth(IntPtr phid, out double MinPulseWidth);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setMinPulseWidth(IntPtr phid, double MinPulseWidth);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxPulseWidth(IntPtr phid, out double MaxPulseWidth);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setMaxPulseWidth(IntPtr phid, double MaxPulseWidth);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinPulseWidthLimit(IntPtr phid, out double MinPulseWidthLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxPulseWidthLimit(IntPtr phid, out double MaxPulseWidthLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getSpeedRampingState(IntPtr phid, out bool SpeedRampingState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setSpeedRampingState(IntPtr phid, bool SpeedRampingState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getTargetPosition(IntPtr phid, out double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setTargetPosition(IntPtr phid, double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getTorque(IntPtr phid, out double Torque);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setTorque(IntPtr phid, double Torque);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinTorque(IntPtr phid, out double MinTorque);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxTorque(IntPtr phid, out double MaxTorque);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getVelocity(IntPtr phid, out double Velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getVelocityLimit(IntPtr phid, out double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setVelocityLimit(IntPtr phid, double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMinVelocityLimit(IntPtr phid, out double MinVelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getMaxVelocityLimit(IntPtr phid, out double MaxVelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_getVoltage(IntPtr phid, out RCServoVoltage Voltage);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setVoltage(IntPtr phid, RCServoVoltage Voltage);
		public delegate void RCServoPositionChangeEvent(IntPtr phid, IntPtr ctx, double position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setOnPositionChangeHandler(IntPtr phid, RCServoPositionChangeEvent fptr, IntPtr ctx);
		public delegate void RCServoTargetPositionReachedEvent(IntPtr phid, IntPtr ctx, double position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setOnTargetPositionReachedHandler(IntPtr phid, RCServoTargetPositionReachedEvent fptr, IntPtr ctx);
		public delegate void RCServoVelocityChangeEvent(IntPtr phid, IntPtr ctx, double velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetRCServo_setOnVelocityChangeHandler(IntPtr phid, RCServoVelocityChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A RCServo PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A RCServoPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void RCServoPositionChangeEventHandler(object sender, RCServoPositionChangeEventArgs e);
	/// <summary> RCServo PositionChange Event data </summary>
	public class RCServoPositionChangeEventArgs : EventArgs {
		/// <summary>The position value
		/// </summary>
		public readonly double Position;
		internal RCServoPositionChangeEventArgs(double position) {
			this.Position = position;
		}
	}

	/// <summary> A RCServo TargetPositionReached Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A RCServoTargetPositionReachedEventArg object contains data and information related to the Event.</param>
	public delegate void RCServoTargetPositionReachedEventHandler(object sender, RCServoTargetPositionReachedEventArgs e);
	/// <summary> RCServo TargetPositionReached Event data </summary>
	public class RCServoTargetPositionReachedEventArgs : EventArgs {
		/// <summary>The position value
		/// </summary>
		public readonly double Position;
		internal RCServoTargetPositionReachedEventArgs(double position) {
			this.Position = position;
		}
	}

	/// <summary> A RCServo VelocityChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A RCServoVelocityChangeEventArg object contains data and information related to the Event.</param>
	public delegate void RCServoVelocityChangeEventHandler(object sender, RCServoVelocityChangeEventArgs e);
	/// <summary> RCServo VelocityChange Event data </summary>
	public class RCServoVelocityChangeEventArgs : EventArgs {
		/// <summary>The velocity value
		/// </summary>
		public readonly double Velocity;
		internal RCServoVelocityChangeEventArgs(double velocity) {
			this.Velocity = velocity;
		}
	}

}
