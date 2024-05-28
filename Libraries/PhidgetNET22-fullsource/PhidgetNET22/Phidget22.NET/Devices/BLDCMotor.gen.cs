using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> BLDCMotor class definition </summary>
	public partial class BLDCMotor : Phidget {
		#region Constructor/Destructor
		/// <summary> BLDCMotor Constructor </summary>
		public BLDCMotor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetBLDCMotor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> BLDCMotor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~BLDCMotor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetBLDCMotor_delete(ref chandle);
			}
		}
		#endregion

		#region Properties

		/// <summary> The acceleration value </summary>
		/// <remarks>The rate at which the controller can change the motor's <c>Velocity</c>.
		/// <list>
		/// <item>The acceleration is bounded by <c>MinAccleration</c> and
		/// <c>MaxAcceleration</c></item>
		/// </list>
		/// 
		/// </remarks>
		public double Acceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setAcceleration(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The acceleration value. </summary>
		/// <remarks>The minimum value that <c>Acceleration</c> can be set to.
		/// </remarks>
		public double MinAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMinAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The acceleration value. </summary>
		/// <remarks>The maximum value that <c>Acceleration</c> can be set to.
		/// </remarks>
		public double MaxAcceleration {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The braking strength value </summary>
		/// <remarks>The most recent braking strength value that the controller has reported.
		/// </remarks>
		public double BrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The braking value </summary>
		/// <remarks>The minimum value that <c>TargetBrakingStrength</c> can be set to.
		/// </remarks>
		public double MinBrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMinBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The braking value </summary>
		/// <remarks>The maximum value that <c>TargetBrakingStrength</c> can be set to.
		/// </remarks>
		public double MaxBrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the controller will fire another
		/// update event.
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
				result = Phidget22Imports.PhidgetBLDCMotor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetBLDCMotor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The most recent position value that the controller has reported.
		/// <list>
		/// <item>Position values are calculated using Hall Effect sensors mounted on the motor, therefore, the
		/// resolution of position depends on the motor you are using.</item>
		/// <item>Units for <c>Position</c> can be set by the user through the <c>RescaleFactor</c>.
		/// The <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees. For more information on how to apply the <c>RescaleFactor</c> to your application,
		/// see your controller's User Guide.</item>
		/// </list>
		/// 
		/// </remarks>
		public double Position {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The lower bound of <c>Position</c>.
		/// </remarks>
		public double MinPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMinPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The position value </summary>
		/// <remarks>The upper bound of <c>Position</c>.
		/// </remarks>
		public double MaxPosition {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxPosition(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The rescale factor value </summary>
		/// <remarks>Change the units of your parameters so that your application is more intuitive.
		/// <list>
		/// <item>Units for <c>Position</c> can be set by the user through the <c>RescaleFactor</c>.
		/// The <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees. For more information on how to apply the <c>RescaleFactor</c> to your application,
		/// see your controller's User Guide.</item>
		/// </list>
		/// 
		/// </remarks>
		public double RescaleFactor {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getRescaleFactor(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setRescaleFactor(chandle, value);
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
		/// immediately reduce the <c>Velocity</c> (i.e. average voltage) to 0 and an error will be
		/// reported to your program.
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
				result = Phidget22Imports.PhidgetBLDCMotor_getStallVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setStallVelocity(chandle, value);
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
				result = Phidget22Imports.PhidgetBLDCMotor_getMinStallVelocity(chandle, out val);
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
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxStallVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The braking  value </summary>
		/// <remarks>When a motor is not being actively driven forward or reverse, you can choose if the motor will be
		/// allowed to freely turn, or will resist being turned.
		/// <list>
		/// <item>A low <c>TargetBrakingStrength</c> value corresponds to free wheeling, this will have the
		/// following effects:
		/// <list>
		/// <item>The motor will continue to rotate after the controller is no longer driving the motor (i.e.
		/// <c>Velocity</c> is 0), due to inertia.</item>
		/// <item>The motor shaft will provide little resistance to being turned when it is stopped.</item>
		/// </list>
		/// </item>
		/// <item>A higher <c>TargetBrakingStrength</c> value will resist being turned, this will have the
		/// following effects:
		/// <list>
		/// <item>The motor will more stop more quickly if it is in motion and braking has been requested. It
		/// will fight against the rotation of the shaft.</item>
		/// </list>
		/// </item>
		/// <item>Braking mode is enabled by setting the <c>Velocity</c> to <c>MinVelocity</c></item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetBrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getTargetBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setTargetBrakingStrength(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The average voltage across the motor is based on the <c>TargetVelocity</c> value.
		/// <list>
		/// <item>At a constant load, increasing the target velocity will increase the speed of the motor.</item>
		/// <item><c>TargetVelocity</c> is bounded by -1×<c>MaxVelocity</c> and
		/// <c>MaxVelocity</c>, where a sign change (±) is indicitave of a direction change.</item>
		/// <item>Setting <c>TargetVelocity</c> to <c>MinVelocity</c> will stop the motor. See
		/// <c>TargetBrakingStrength</c> for more information on stopping the motor.</item>
		/// <item>The units of <c>TargetVelocity</c> and <c>Acceleration</c> refer to 'duty cycle'.
		/// This is because the controller must rapidly switch the power on/off (i.e. change the duty cycle) in
		/// order to manipulate the voltage across the motor.</item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getTargetVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetBLDCMotor_setTargetVelocity(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The most recent velocity value that the controller has reported.
		/// </remarks>
		public double Velocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The minimum value that <c>TargetVelocity</c> can be set to
		/// <list>
		/// <item>Set the <c>TargetVelocity</c> to <c>MinVelocity</c> to stop the motor. See
		/// <c>TargetBrakingStrength</c> for more information on stopping the motor.</item>
		/// <item><c>TargetVelocity</c> is bounded by -1×<c>MaxVelocity</c> and
		/// <c>MaxVelocity</c>, where a sign change (±) is indicitave of a direction change.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MinVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMinVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The velocity value </summary>
		/// <remarks>The maximum value that <c>TargetVelocity</c> can be set to.
		/// <list>
		/// <item><c>TargetVelocity</c> is bounded by -1×<c>MaxVelocity</c> and
		/// <c>MaxVelocity</c>, where a sign change (±) is indicitave of a direction change.</item>
		/// </list>
		/// 
		/// </remarks>
		public double MaxVelocity {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetBLDCMotor_getMaxVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods

		/// <summary> </summary>
		/// <remarks>Adds an offset (positive or negative) to the current position.
		/// <list>
		/// <item>This can be especially useful for zeroing position.</item>
		/// </list>
		/// 
		/// </remarks>
		public void AddPositionOffset(double positionOffset) {
			ErrorCode result;
			result = Phidget22Imports.PhidgetBLDCMotor_addPositionOffset(chandle, positionOffset);
			if (result != 0) {
				throw PhidgetException.CreateByCode(result);
			}
		}
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeBrakingStrengthChangeEventCallback = new Phidget22Imports.BLDCMotorBrakingStrengthChangeEvent(nativeBrakingStrengthChangeEvent);
			result = Phidget22Imports.PhidgetBLDCMotor_setOnBrakingStrengthChangeHandler(chandle, nativeBrakingStrengthChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativePositionChangeEventCallback = new Phidget22Imports.BLDCMotorPositionChangeEvent(nativePositionChangeEvent);
			result = Phidget22Imports.PhidgetBLDCMotor_setOnPositionChangeHandler(chandle, nativePositionChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVelocityUpdateEventCallback = new Phidget22Imports.BLDCMotorVelocityUpdateEvent(nativeVelocityUpdateEvent);
			result = Phidget22Imports.PhidgetBLDCMotor_setOnVelocityUpdateHandler(chandle, nativeVelocityUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetBLDCMotor_setOnBrakingStrengthChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetBLDCMotor_setOnPositionChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetBLDCMotor_setOnVelocityUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent braking strength value will be reported in this event, which occurs when the
		/// <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>Regardless of the <c>DataInterval</c>, this event will occur only when the braking
		/// strength value has changed from the previous value reported.</item>
		/// <item>Braking mode is enabled by setting the <c>Velocity</c> to <c>MinVelocity</c></item>
		/// </list>
		/// 
		/// </remarks>
		public event BLDCMotorBrakingStrengthChangeEventHandler BrakingStrengthChange;
		internal void OnBrakingStrengthChange(BLDCMotorBrakingStrengthChangeEventArgs e) {
			if (BrakingStrengthChange != null) {
				foreach (BLDCMotorBrakingStrengthChangeEventHandler BrakingStrengthChangeHandler in BrakingStrengthChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = BrakingStrengthChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(BrakingStrengthChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						BrakingStrengthChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.BLDCMotorBrakingStrengthChangeEvent nativeBrakingStrengthChangeEventCallback;
		internal void nativeBrakingStrengthChangeEvent(IntPtr phid, IntPtr ctx, double brakingStrength) {
			OnBrakingStrengthChange(new BLDCMotorBrakingStrengthChangeEventArgs(brakingStrength));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent position value will be reported in this event, which occurs when the
		/// <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>Regardless of the <c>DataInterval</c>, this event will occur only when the position value
		/// has changed from the previous value reported.</item>
		/// <item>Position values are calculated using Hall Effect sensors mounted on the motor, therefore, the
		/// resolution of position depends on the motor you are using.</item>
		/// <item>Units for <c>Position</c> can be set by the user through the <c>RescaleFactor</c>.
		/// The <c>RescaleFactor</c> allows you to use more intuitive units such as rotations, or
		/// degrees. For more information on how to apply the <c>RescaleFactor</c> to your application,
		/// see your controller's User Guide.</item>
		/// </list>
		/// 
		/// </remarks>
		public event BLDCMotorPositionChangeEventHandler PositionChange;
		internal void OnPositionChange(BLDCMotorPositionChangeEventArgs e) {
			if (PositionChange != null) {
				foreach (BLDCMotorPositionChangeEventHandler PositionChangeHandler in PositionChange.GetInvocationList()) {
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
		Phidget22Imports.BLDCMotorPositionChangeEvent nativePositionChangeEventCallback;
		internal void nativePositionChangeEvent(IntPtr phid, IntPtr ctx, double position) {
			OnPositionChange(new BLDCMotorPositionChangeEventArgs(position));
		}
		/// <summary>  </summary>
		/// <remarks>The most recent velocity value will be reported in this event, which occurs when the
		/// <c>DataInterval</c> has elapsed.
		/// <list>
		/// <item>This event will <b>always</b> occur when the <c>DataInterval</c> elapses. You can depend
		/// on this event for constant timing when implementing control loops in code. This is the last event
		/// to fire, giving you up-to-date access to all properties.</item>
		/// </list>
		/// 
		/// </remarks>
		public event BLDCMotorVelocityUpdateEventHandler VelocityUpdate;
		internal void OnVelocityUpdate(BLDCMotorVelocityUpdateEventArgs e) {
			if (VelocityUpdate != null) {
				foreach (BLDCMotorVelocityUpdateEventHandler VelocityUpdateHandler in VelocityUpdate.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = VelocityUpdateHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(VelocityUpdateHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						VelocityUpdateHandler(this, e);
				}
			}
		}
		Phidget22Imports.BLDCMotorVelocityUpdateEvent nativeVelocityUpdateEventCallback;
		internal void nativeVelocityUpdateEvent(IntPtr phid, IntPtr ctx, double velocity) {
			OnVelocityUpdate(new BLDCMotorVelocityUpdateEventArgs(velocity));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_addPositionOffset(IntPtr phid, double positionOffset);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getAcceleration(IntPtr phid, out double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setAcceleration(IntPtr phid, double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinAcceleration(IntPtr phid, out double MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxAcceleration(IntPtr phid, out double MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getBrakingStrength(IntPtr phid, out double BrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinBrakingStrength(IntPtr phid, out double MinBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxBrakingStrength(IntPtr phid, out double MaxBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getPosition(IntPtr phid, out double Position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinPosition(IntPtr phid, out double MinPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxPosition(IntPtr phid, out double MaxPosition);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getRescaleFactor(IntPtr phid, out double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setRescaleFactor(IntPtr phid, double RescaleFactor);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getStallVelocity(IntPtr phid, out double StallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setStallVelocity(IntPtr phid, double StallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinStallVelocity(IntPtr phid, out double MinStallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxStallVelocity(IntPtr phid, out double MaxStallVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getTargetBrakingStrength(IntPtr phid, out double TargetBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setTargetBrakingStrength(IntPtr phid, double TargetBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getTargetVelocity(IntPtr phid, out double TargetVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setTargetVelocity(IntPtr phid, double TargetVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getVelocity(IntPtr phid, out double Velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMinVelocity(IntPtr phid, out double MinVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_getMaxVelocity(IntPtr phid, out double MaxVelocity);
		public delegate void BLDCMotorBrakingStrengthChangeEvent(IntPtr phid, IntPtr ctx, double brakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setOnBrakingStrengthChangeHandler(IntPtr phid, BLDCMotorBrakingStrengthChangeEvent fptr, IntPtr ctx);
		public delegate void BLDCMotorPositionChangeEvent(IntPtr phid, IntPtr ctx, double position);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setOnPositionChangeHandler(IntPtr phid, BLDCMotorPositionChangeEvent fptr, IntPtr ctx);
		public delegate void BLDCMotorVelocityUpdateEvent(IntPtr phid, IntPtr ctx, double velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetBLDCMotor_setOnVelocityUpdateHandler(IntPtr phid, BLDCMotorVelocityUpdateEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A BLDCMotor BrakingStrengthChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A BLDCMotorBrakingStrengthChangeEventArg object contains data and information related to the Event.</param>
	public delegate void BLDCMotorBrakingStrengthChangeEventHandler(object sender, BLDCMotorBrakingStrengthChangeEventArgs e);
	/// <summary> BLDCMotor BrakingStrengthChange Event data </summary>
	public class BLDCMotorBrakingStrengthChangeEventArgs : EventArgs {
		/// <summary>The braking strength value
		/// </summary>
		public readonly double BrakingStrength;
		internal BLDCMotorBrakingStrengthChangeEventArgs(double brakingStrength) {
			this.BrakingStrength = brakingStrength;
		}
	}

	/// <summary> A BLDCMotor PositionChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A BLDCMotorPositionChangeEventArg object contains data and information related to the Event.</param>
	public delegate void BLDCMotorPositionChangeEventHandler(object sender, BLDCMotorPositionChangeEventArgs e);
	/// <summary> BLDCMotor PositionChange Event data </summary>
	public class BLDCMotorPositionChangeEventArgs : EventArgs {
		/// <summary>The position value
		/// </summary>
		public readonly double Position;
		internal BLDCMotorPositionChangeEventArgs(double position) {
			this.Position = position;
		}
	}

	/// <summary> A BLDCMotor VelocityUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A BLDCMotorVelocityUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void BLDCMotorVelocityUpdateEventHandler(object sender, BLDCMotorVelocityUpdateEventArgs e);
	/// <summary> BLDCMotor VelocityUpdate Event data </summary>
	public class BLDCMotorVelocityUpdateEventArgs : EventArgs {
		/// <summary>The velocity value
		/// </summary>
		public readonly double Velocity;
		internal BLDCMotorVelocityUpdateEventArgs(double velocity) {
			this.Velocity = velocity;
		}
	}

}
