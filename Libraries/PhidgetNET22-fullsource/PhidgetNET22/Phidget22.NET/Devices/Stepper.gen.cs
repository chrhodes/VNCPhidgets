using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> Stepper class definition </summary>
	public partial class Stepper : Phidget {
		#region Constructor/Destructor
		/// <summary> Stepper Constructor </summary>
		public Stepper() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetStepper_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> Stepper Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~Stepper() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetStepper_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The acceleration value </summary>
		/// <remarks>The rate at which the controller can change the motor's <c>Velocity</c>.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Acceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setAcceleration(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The acceleration value </summary>
		/// <remarks>The minimum value that <c>Acceleration</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMinAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The acceleration value </summary>
		/// <remarks>The maximum value that <c>Acceleration</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMaxAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The control mode value </summary>
		/// <remarks>Use step mode when you want to set a <c>TargetPosition</c> for the Stepper motor. Use run
		/// mode when you simply want the Stepper motor to rotate continuously in a specific direction.
		/// </remarks>
		public StepperControlMode ControlMode {
			get {
				ErrorCode result;
				StepperControlMode val;
				result = Phidget22Imports.PhidgetStepper_getControlMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setControlMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current limit value </summary>
		/// <remarks>The current through the motor will be limited by the <c>CurrentLimit</c>.
		/// <list>
		/// <item>See your Stepper motor's data sheet for more information about what value the
		/// <c>CurrentLimit</c> should be.</item>
		/// </list>
		/// 
		/// </remarks>
		public double CurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setCurrentLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current limit </summary>
		/// <remarks>The minimum value that <c>CurrentLimit</c> and <c>HoldingCurrentLimit</c> can be set
		/// to.
		/// <list>
		/// <item>Reference your controller's User Guide for more information about how the
		/// <c>HoldingCurrentLimit</c> and <c>CurrentLimit</c> can be used in your
		/// application.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMinCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current limit </summary>
		/// <remarks>The maximum value that <c>CurrentLimit</c> and <c>HoldingCurrentLimit</c> can be set
		/// to.
		/// <list>
		/// <item>Reference your controller's User Guide for more information about how the
		/// <c>HoldingCurrentLimit</c> and <c>CurrentLimit</c> can be used in your
		/// application.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMaxCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the controller will fire another
		/// <c>PositionChange</c>/<c>VelocityChange</c> event.
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
				result = Phidget22Imports.PhidgetStepper_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetStepper_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetStepper_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The engaged state </summary>
		/// <remarks>When this property is true, the controller will supply power to the motor coils.
		/// <list>
		/// <item>The controller must be <c>Engaged</c> in order to move the Stepper motor, or have it hold
		/// position.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool Engaged {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetStepper_getEngaged(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setEngaged(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The <c>HoldingCurrentLimit</c> will activate when the <c>TargetPosition</c> has been
		/// reached. It will limit current through the motor.
		/// <list>
		/// <item>When the motor is not stopped, the current through the motor is limited by the
		/// <c>CurrentLimit</c>.</item>
		/// <item>If no <c>HoldingCurrentLimit</c> is specified, the <c>CurrentLimit</c> value will
		/// persist when the motor is stopped.</item>
		/// <item>Reference your controller's User Guide for more information about how the
		/// <c>HoldingCurrentLimit</c> and <c>CurrentLimit</c> can be used in your
		/// application.</item>
		/// </list>
		/// 
		/// </remarks>
		public double HoldingCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getHoldingCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setHoldingCurrentLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The moving state </summary>
		/// <remarks><c>IsMoving</c> returns true while the controller is sending commands to the motor. Note:
		/// there is no feedback to the controller, so it does not know whether the motor shaft is actually
		/// moving or not.
		/// </remarks>
		public bool IsMoving {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetStepper_getIsMoving(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The most recent position value that the controller has reported.
		/// <list>
		/// <item>This value will always be between <c>MinPosition</c> and <c>MaxPosition</c>.</item>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Position {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The minimum value that <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMinPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The maximum value that <c>TargetPosition</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMaxPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The rescale factor value </summary>
		/// <remarks>Applies a factor to the [user units] per step to all movement parameters to make the units in your
		/// application is more intuitive.
		/// <list>
		/// <item>For example, starting from position 0 and setting a new position with a rescale factor, the
		/// stepper will move <c>Position</c> / <c>RescaleFactor</c> steps.</item>
		/// <item>In this way, units for <c>Position</c>, <c>Velocity</c>, and
		/// <c>Acceleration</c> can be set by the user through the <c>RescaleFactor</c>. The
		/// <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double RescaleFactor {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getRescaleFactor(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setRescaleFactor(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>If the controller is configured and the <c>TargetPosition</c> is set, the Stepper motor will
		/// move towards the <c>TargetPosition</c> at the specified <c>Acceleration</c> and
		/// <c>Velocity</c>.
		/// <list>
		/// <item><c>TargetPosition</c> is only used when the <c>ControlMode</c> is set to step
		/// mode.</item>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getTargetPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setTargetPosition(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The most recent velocity value that the controller has reported.
		/// <list>
		/// <item>This value is bounded by <c>MinVelocityLimit</c> and <c>MaxVelocityLimit</c>.</item>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Velocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> Velocity limit </summary>
		/// <remarks>When moving, the Stepper motor velocity will be limited by this value.
		/// <list>
		/// <item>The <c>VelocityLimit</c> is bounded by <c>MinVelocityLimit</c> and
		/// <c>MaxVelocityLimit</c>.</item>
		/// <item>When in step mode, the <c>MinVelocityLimit</c> has a value of 0. This is because the sign
		/// (±) of the <c>TargetPosition</c> will indicate the direction.</item>
		/// <item>When in run mode, the <c>MinVelocityLimit</c> has a value of
		/// -<c>MaxVelocityLimit</c>. This is because there is no target position, so the direction is
		/// defined by the sign (±) of the <c>VelocityLimit</c>.</item>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double VelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setVelocityLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity limit value </summary>
		/// <remarks>The minimum value that <c>VelocityLimit</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMinVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The maximum value that <c>VelocityLimit</c> can be set to.
		/// <list>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxVelocityLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetStepper_getMaxVelocityLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Adds an offset (positive or negative) to the current position and target position.
		/// <list>
		/// <item>This is especially useful for zeroing position.</item>
		/// </list>
		/// 
		/// </remarks>
		public void AddPositionOffset(double positionOffset) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetStepper_addPositionOffset(chandle, positionOffset);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary>The position value </summary>
		/// <remarks>If the controller is configured and the <c>TargetPosition</c> is set, the Stepper motor will
		/// move towards the <c>TargetPosition</c> at the specified <c>Acceleration</c> and
		/// <c>Velocity</c>.
		/// <list>
		/// <item><c>TargetPosition</c> is only used when the <c>ControlMode</c> is set to step
		/// mode.</item>
		/// <item>Units for <c>Position</c>, <c>Velocity</c>, and <c>Acceleration</c> can be
		/// set by the user through the <c>RescaleFactor</c>. The <c>RescaleFactor</c> allows you
		/// to use more intuitive units such as rotations, or degrees.</item>
		/// </list>
		/// 
		/// </remarks>
		public IAsyncResult BeginSetTargetPosition(double targetPosition, AsyncCallback callback, object ctx) {
			AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, ctx, this, "SetTargetPosition");
			try {
				ErrorCode result;
				result = Phidget22Imports.PhidgetStepper_setTargetPosition_async(chandle, targetPosition, asyncResult.cCallbackDelegate, IntPtr.Zero);
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
			nativePositionChangeEventCallback = new Phidget22Imports.StepperPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetStepper_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeStoppedEventCallback = new Phidget22Imports.StepperStoppedEvent(nativeStoppedEvent);
			result = Phidget22Imports.PhidgetStepper_setOnStoppedHandler(chandle, nativeStoppedEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVelocityChangeEventCallback = new Phidget22Imports.StepperVelocityChangeEvent(nativeVelocityChangeEvent);
			result = Phidget22Imports.PhidgetStepper_setOnVelocityChangeHandler(chandle, nativeVelocityChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetStepper_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetStepper_setOnStoppedHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetStepper_setOnVelocityChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the controller updates the stepper motor position.
		/// <list>
		/// <item>This event will still fire even if the motor is blocked from physically moving or misses
		/// steps.</item>
		/// </list>
		/// 
		/// </remarks>
		public event StepperPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(StepperPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (StepperPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
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
		Phidget22Imports.StepperPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, double position) {
			OnPositionChange(new StepperPositionChangeEventArgs(position));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the motor controller stops.
		/// <list>
		/// <item>The motor may still be physically moving if the inertia is great enough to make it
		/// misstep.</item>
		/// </list>
		/// 
		/// </remarks>
		public event StepperStoppedEventHandler Stopped;
		internal void OnStopped(StepperStoppedEventArgs e) {
			if (Stopped != null) {
				foreach (StepperStoppedEventHandler StoppedHandler in Stopped.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = StoppedHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(StoppedHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						StoppedHandler(this, e);
				}
			}
		}
		Phidget22Imports.StepperStoppedEvent nativeStoppedEventCallback;
		internal void nativeStoppedEvent(IntPtr phid, IntPtr ctx) {
			OnStopped(new StepperStoppedEventArgs());
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the stepper motor velocity changes.
		/// </remarks>
		public event StepperVelocityChangeEventHandler VelocityChange;
		internal void OnVelocityChange(StepperVelocityChangeEventArgs e) {
			if (VelocityChange != null) {
				foreach (StepperVelocityChangeEventHandler VelocityChangeHandler in VelocityChange.GetInvocationList()) {
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
		Phidget22Imports.StepperVelocityChangeEvent nativeVelocityChangeEventCallback;
		internal void nativeVelocityChangeEvent(IntPtr phid, IntPtr ctx, double velocity) {
			OnVelocityChange(new StepperVelocityChangeEventArgs(velocity));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_addPositionOffset(IntPtr phid, double positionOffset);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setTargetPosition_async(IntPtr phid, double TargetPosition, AsyncCallbackEvent callback, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getAcceleration(IntPtr phid, out double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setAcceleration(IntPtr phid, double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMinAcceleration(IntPtr phid, out double MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMaxAcceleration(IntPtr phid, out double MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getControlMode(IntPtr phid, out StepperControlMode ControlMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setControlMode(IntPtr phid, StepperControlMode ControlMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getCurrentLimit(IntPtr phid, out double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setCurrentLimit(IntPtr phid, double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMinCurrentLimit(IntPtr phid, out double MinCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMaxCurrentLimit(IntPtr phid, out double MaxCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getEngaged(IntPtr phid, out bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setEngaged(IntPtr phid, bool Engaged);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getHoldingCurrentLimit(IntPtr phid, out double HoldingCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setHoldingCurrentLimit(IntPtr phid, double HoldingCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getIsMoving(IntPtr phid, out bool IsMoving);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getPosition(IntPtr phid, out double Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMinPosition(IntPtr phid, out double MinPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMaxPosition(IntPtr phid, out double MaxPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getRescaleFactor(IntPtr phid, out double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setRescaleFactor(IntPtr phid, double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getTargetPosition(IntPtr phid, out double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setTargetPosition(IntPtr phid, double TargetPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getVelocity(IntPtr phid, out double Velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getVelocityLimit(IntPtr phid, out double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setVelocityLimit(IntPtr phid, double VelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMinVelocityLimit(IntPtr phid, out double MinVelocityLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_getMaxVelocityLimit(IntPtr phid, out double MaxVelocityLimit);
		public delegate void StepperPositionChangeEvent(IntPtr phid, IntPtr ctx, double position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setOnPositionChangeHandler(IntPtr phid, StepperPositionChangeEvent fptr, IntPtr ctx);
		public delegate void StepperStoppedEvent(IntPtr phid, IntPtr ctx);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setOnStoppedHandler(IntPtr phid, StepperStoppedEvent fptr, IntPtr ctx);
		public delegate void StepperVelocityChangeEvent(IntPtr phid, IntPtr ctx, double velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetStepper_setOnVelocityChangeHandler(IntPtr phid, StepperVelocityChangeEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A Stepper PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A StepperPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void StepperPositionChangeEventHandler(object sender, StepperPositionChangeEventArgs e);
	/// <summary> Stepper PositionChange Event data </summary>
	public class StepperPositionChangeEventArgs : EventArgs {
		/// <summary>The current stepper position
		/// </summary>
		public readonly double Position;
		internal StepperPositionChangeEventArgs(double position) {
			this.Position = position;
		}
	}

	/// <summary> A Stepper Stopped Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A StepperStoppedEventArg object contains data and information related to the Event.</param>
	public delegate void StepperStoppedEventHandler(object sender, StepperStoppedEventArgs e);
	/// <summary> Stepper Stopped Event data </summary>
	public class StepperStoppedEventArgs : EventArgs {
		internal StepperStoppedEventArgs() {
		}
	}

	/// <summary> A Stepper VelocityChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A StepperVelocityChangeEventArg object contains data and information related to the Event.</param>
	public delegate void StepperVelocityChangeEventHandler(object sender, StepperVelocityChangeEventArgs e);
	/// <summary> Stepper VelocityChange Event data </summary>
	public class StepperVelocityChangeEventArgs : EventArgs {
		/// <summary>Velocity of the stepper. Sign indicates direction.
		/// </summary>
		public readonly double Velocity;
		internal StepperVelocityChangeEventArgs(double velocity) {
			this.Velocity = velocity;
		}
	}

}
