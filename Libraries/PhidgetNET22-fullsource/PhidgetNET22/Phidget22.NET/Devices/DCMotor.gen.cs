using System;
using System.Text;
using Phidget22.Events;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Phidget22 {
	/// <summary> DCMotor class definition </summary>
	public partial class DCMotor : Phidget {
		#region Constructor/Destructor
		/// <summary> DCMotor Constructor </summary>
		public DCMotor() {
			ErrorCode result;
			result = Phidget22Imports.PhidgetDCMotor_create(out chandle);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			initializeEvents();
		}
		/// <summary> DCMotor Destructor </summary>
		/// <remarks> Will attempt to close the device. </remarks>
		~DCMotor() {
			if (managerPhidget)
				Phidget22Imports.Phidget_release(ref chandle);
			else {
				uninitializeEvents();
				Phidget22Imports.PhidgetDCMotor_delete(ref chandle);
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
				result = Phidget22Imports.PhidgetDCMotor_getAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setAcceleration(chandle, value);
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
				result = Phidget22Imports.PhidgetDCMotor_getMinAcceleration(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMaxAcceleration(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The back EMF value </summary>
		/// <remarks>The most recent <c>BackEMF</c> value that the controller has reported.
		/// </remarks>
		public double BackEMF {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getBackEMF(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The back EMF state </summary>
		/// <remarks>When <c>BackEMFSensingState</c> is enabled, the controller will measure and report the
		/// <c>BackEMF</c>.
		/// <list>
		/// <item>The motor will coast (freewheel) 5% of the time while the back EMF is being measured (800μs
		/// every 16ms). Therefore, at a <c>DutyCycle</c> of 100%, the motor will only be driven for 95%
		/// of the time.</item>
		/// </list>
		/// 
		/// </remarks>
		public bool BackEMFSensingState {
			get {
				ErrorCode result;
				bool val;
				result = Phidget22Imports.PhidgetDCMotor_getBackEMFSensingState(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setBackEMFSensingState(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The braking strength value </summary>
		/// <remarks>The most recent braking strength value that the controller has reported.
		/// </remarks>
		public double BrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getBrakingStrength(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMinBrakingStrength(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMaxBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The controller will limit the current through the motor to the <c>CurrentLimit</c> value.
		/// </remarks>
		public double CurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setCurrentLimit(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The minimum value that <c>CurrentLimit</c> can be set to.
		/// </remarks>
		public double MinCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getMinCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current value </summary>
		/// <remarks>The maximum value that <c>CurrentLimit</c> can be set to.
		/// </remarks>
		public double MaxCurrentLimit {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getMaxCurrentLimit(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current regulator gain value </summary>
		/// <remarks>Depending on power supply voltage and motor coil inductance, current through the motor can change
		/// relatively slowly or extremely rapidly. A physically larger DC Motor will typically have a lower
		/// inductance, requiring a higher current regulator gain. A higher power supply voltage will result in
		/// motor current changing more rapidly, requiring a higher current regulator gain. If the current
		/// regulator gain is too small, spikes in current will occur, causing large variations in torque, and
		/// possibly damaging the motor controller. If the current regulator gain is too high, the current will
		/// jitter, causing the motor to sound 'rough', especially when changing directions.
		/// <list>
		/// <item>Each DC Motor we sell specifies a suitable current regulator gain.</item>
		/// </list>
		/// 
		/// </remarks>
		public double CurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setCurrentRegulatorGain(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The current regulator gain value </summary>
		/// <remarks>The minimum value that <c>CurrentRegulatorGain</c> can be set to.
		/// </remarks>
		public double MinCurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getMinCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The current regulator gain value </summary>
		/// <remarks>The maximum value that <c>CurrentRegulatorGain</c> can be set to.
		/// </remarks>
		public double MaxCurrentRegulatorGain {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getMaxCurrentRegulatorGain(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}

		/// <summary> The data interval value </summary>
		/// <remarks>The <c>DataInterval</c> is the time that must elapse before the controller will fire another
		/// <c>VelocityUpdate</c>/<c>BrakingStrengthChange</c> event.
		/// <list>
		/// <item>The data interval is bounded by <c>MinDataInterval</c> and
		/// <c>MaxDataInterval</c>.</item>
		/// <item>Note: <c>BrakingStrengthChange</c> events will only fire if a change in braking has
		/// occurred.</item>
		/// </list>
		/// 
		/// </remarks>
		public int DataInterval {
			get {
				ErrorCode result;
				int val;
				result = Phidget22Imports.PhidgetDCMotor_getDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setDataInterval(chandle, value);
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
				result = Phidget22Imports.PhidgetDCMotor_getMinDataInterval(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMaxDataInterval(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
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
				result = Phidget22Imports.PhidgetDCMotor_getFanMode(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setFanMode(chandle, value);
				if (result != 0)
					throw PhidgetException.CreateByCode(result);
			}
		}

		/// <summary> The braking  value </summary>
		/// <remarks>When a motor is not being actively driven forward or reverse (i.e. <c>Velocity</c> = 0), you
		/// can choose if the motor will be allowed to freely turn, or will resist being turned.
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
		/// <item>Braking mode is enabled by setting the <c>Velocity</c> to <c>MinVelocity</c>
		/// (0.0)</item>
		/// </list>
		/// 
		/// </remarks>
		public double TargetBrakingStrength {
			get {
				ErrorCode result;
				double val;
				result = Phidget22Imports.PhidgetDCMotor_getTargetBrakingStrength(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setTargetBrakingStrength(chandle, value);
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
				result = Phidget22Imports.PhidgetDCMotor_getTargetVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
			set {
				ErrorCode result;
				result = Phidget22Imports.PhidgetDCMotor_setTargetVelocity(chandle, value);
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
				result = Phidget22Imports.PhidgetDCMotor_getVelocity(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMinVelocity(chandle, out val);
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
				result = Phidget22Imports.PhidgetDCMotor_getMaxVelocity(chandle, out val);
				if (result != 0) {
					throw PhidgetException.CreateByCode(result);
				}
				return val;
			}
		}
		#endregion

		#region Methods
		#endregion

		#region Events
		internal override void initializeEvents() {
			ErrorCode result;
			initializeBaseEvents();
			nativeBackEMFChangeEventCallback = new Phidget22Imports.DCMotorBackEMFChangeEvent(nativeBackEMFChangeEvent);
			result = Phidget22Imports.PhidgetDCMotor_setOnBackEMFChangeHandler(chandle, nativeBackEMFChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeBrakingStrengthChangeEventCallback = new Phidget22Imports.DCMotorBrakingStrengthChangeEvent(nativeBrakingStrengthChangeEvent);
			result = Phidget22Imports.PhidgetDCMotor_setOnBrakingStrengthChangeHandler(chandle, nativeBrakingStrengthChangeEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			nativeVelocityUpdateEventCallback = new Phidget22Imports.DCMotorVelocityUpdateEvent(nativeVelocityUpdateEvent);
			result = Phidget22Imports.PhidgetDCMotor_setOnVelocityUpdateHandler(chandle, nativeVelocityUpdateEventCallback, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		internal override void uninitializeEvents() {
			ErrorCode result;
			uninitializeBaseEvents();
			result = Phidget22Imports.PhidgetDCMotor_setOnBackEMFChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetDCMotor_setOnBrakingStrengthChangeHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			result = Phidget22Imports.PhidgetDCMotor_setOnVelocityUpdateHandler(chandle, null, IntPtr.Zero);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
		}
		/// <summary>  </summary>
		/// <remarks>The most recent back emf value will be reported in this event.
		/// </remarks>
		public event DCMotorBackEMFChangeEventHandler BackEMFChange;
		internal void OnBackEMFChange(DCMotorBackEMFChangeEventArgs e) {
			if (BackEMFChange != null) {
				foreach (DCMotorBackEMFChangeEventHandler BackEMFChangeHandler in BackEMFChange.GetInvocationList()) {
					#if DOTNET_FRAMEWORK || netcoreapp20 || netstandard20
					ISynchronizeInvoke syncInvoke = BackEMFChangeHandler.Target as ISynchronizeInvoke;
					if (Phidget.InvokeEventCallbacks && syncInvoke != null && syncInvoke.InvokeRequired) {
						try {
							syncInvoke.Invoke(BackEMFChangeHandler, new object[] { this, e });
						} catch (ObjectDisposedException) { } // Form is probably being closed.. ignore
					} else
					#endif
						BackEMFChangeHandler(this, e);
				}
			}
		}
		Phidget22Imports.DCMotorBackEMFChangeEvent nativeBackEMFChangeEventCallback;
		internal void nativeBackEMFChangeEvent(IntPtr phid, IntPtr ctx, double backEMF) {
			OnBackEMFChange(new DCMotorBackEMFChangeEventArgs(backEMF));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs when the motor braking strength changes.
		/// </remarks>
		public event DCMotorBrakingStrengthChangeEventHandler BrakingStrengthChange;
		internal void OnBrakingStrengthChange(DCMotorBrakingStrengthChangeEventArgs e) {
			if (BrakingStrengthChange != null) {
				foreach (DCMotorBrakingStrengthChangeEventHandler BrakingStrengthChangeHandler in BrakingStrengthChange.GetInvocationList()) {
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
		Phidget22Imports.DCMotorBrakingStrengthChangeEvent nativeBrakingStrengthChangeEventCallback;
		internal void nativeBrakingStrengthChangeEvent(IntPtr phid, IntPtr ctx, double brakingStrength) {
			OnBrakingStrengthChange(new DCMotorBrakingStrengthChangeEventArgs(brakingStrength));
		}
		/// <summary>  </summary>
		/// <remarks>Occurs at a rate defined by the <c>DataInterval</c>.
		/// </remarks>
		public event DCMotorVelocityUpdateEventHandler VelocityUpdate;
		internal void OnVelocityUpdate(DCMotorVelocityUpdateEventArgs e) {
			if (VelocityUpdate != null) {
				foreach (DCMotorVelocityUpdateEventHandler VelocityUpdateHandler in VelocityUpdate.GetInvocationList()) {
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
		Phidget22Imports.DCMotorVelocityUpdateEvent nativeVelocityUpdateEventCallback;
		internal void nativeVelocityUpdateEvent(IntPtr phid, IntPtr ctx, double velocity) {
			OnVelocityUpdate(new DCMotorVelocityUpdateEventArgs(velocity));
		}
		#endregion
	}
	internal partial class Phidget22Imports {
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_create(out IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_delete(ref IntPtr phid);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getAcceleration(IntPtr phid, out double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setAcceleration(IntPtr phid, double Acceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinAcceleration(IntPtr phid, out double MinAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxAcceleration(IntPtr phid, out double MaxAcceleration);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getBackEMF(IntPtr phid, out double BackEMF);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getBackEMFSensingState(IntPtr phid, out bool BackEMFSensingState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setBackEMFSensingState(IntPtr phid, bool BackEMFSensingState);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getBrakingStrength(IntPtr phid, out double BrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinBrakingStrength(IntPtr phid, out double MinBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxBrakingStrength(IntPtr phid, out double MaxBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getCurrentLimit(IntPtr phid, out double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setCurrentLimit(IntPtr phid, double CurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinCurrentLimit(IntPtr phid, out double MinCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxCurrentLimit(IntPtr phid, out double MaxCurrentLimit);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getCurrentRegulatorGain(IntPtr phid, out double CurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setCurrentRegulatorGain(IntPtr phid, double CurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinCurrentRegulatorGain(IntPtr phid, out double MinCurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxCurrentRegulatorGain(IntPtr phid, out double MaxCurrentRegulatorGain);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getDataInterval(IntPtr phid, out int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setDataInterval(IntPtr phid, int DataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinDataInterval(IntPtr phid, out int MinDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxDataInterval(IntPtr phid, out int MaxDataInterval);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getFanMode(IntPtr phid, out FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setFanMode(IntPtr phid, FanMode FanMode);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getTargetBrakingStrength(IntPtr phid, out double TargetBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setTargetBrakingStrength(IntPtr phid, double TargetBrakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getTargetVelocity(IntPtr phid, out double TargetVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setTargetVelocity(IntPtr phid, double TargetVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getVelocity(IntPtr phid, out double Velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMinVelocity(IntPtr phid, out double MinVelocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_getMaxVelocity(IntPtr phid, out double MaxVelocity);
		public delegate void DCMotorBackEMFChangeEvent(IntPtr phid, IntPtr ctx, double backEMF);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setOnBackEMFChangeHandler(IntPtr phid, DCMotorBackEMFChangeEvent fptr, IntPtr ctx);
		public delegate void DCMotorBrakingStrengthChangeEvent(IntPtr phid, IntPtr ctx, double brakingStrength);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setOnBrakingStrengthChangeHandler(IntPtr phid, DCMotorBrakingStrengthChangeEvent fptr, IntPtr ctx);
		public delegate void DCMotorVelocityUpdateEvent(IntPtr phid, IntPtr ctx, double velocity);
		[DllImport(DllName, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		public static extern ErrorCode PhidgetDCMotor_setOnVelocityUpdateHandler(IntPtr phid, DCMotorVelocityUpdateEvent fptr, IntPtr ctx);
	}
}

namespace Phidget22.Events {
	/// <summary> A DCMotor BackEMFChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DCMotorBackEMFChangeEventArg object contains data and information related to the Event.</param>
	public delegate void DCMotorBackEMFChangeEventHandler(object sender, DCMotorBackEMFChangeEventArgs e);
	/// <summary> DCMotor BackEMFChange Event data </summary>
	public class DCMotorBackEMFChangeEventArgs : EventArgs {
		/// <summary>The back EMF voltage from the motor
		/// </summary>
		public readonly double BackEMF;
		internal DCMotorBackEMFChangeEventArgs(double backEMF) {
			this.BackEMF = backEMF;
		}
	}

	/// <summary> A DCMotor BrakingStrengthChange Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DCMotorBrakingStrengthChangeEventArg object contains data and information related to the Event.</param>
	public delegate void DCMotorBrakingStrengthChangeEventHandler(object sender, DCMotorBrakingStrengthChangeEventArgs e);
	/// <summary> DCMotor BrakingStrengthChange Event data </summary>
	public class DCMotorBrakingStrengthChangeEventArgs : EventArgs {
		/// <summary>The most recent braking strength value will be reported in this event.
		/// <list>
		/// <item>This event will occur only when the value of braking strength has changed</item>
		/// </list>
		/// 
		/// </summary>
		public readonly double BrakingStrength;
		internal DCMotorBrakingStrengthChangeEventArgs(double brakingStrength) {
			this.BrakingStrength = brakingStrength;
		}
	}

	/// <summary> A DCMotor VelocityUpdate Event delegate </summary>
	/// <param name="sender">The object that triggered the event.</param>
	/// <param name="e">A DCMotorVelocityUpdateEventArg object contains data and information related to the Event.</param>
	public delegate void DCMotorVelocityUpdateEventHandler(object sender, DCMotorVelocityUpdateEventArgs e);
	/// <summary> DCMotor VelocityUpdate Event data </summary>
	public class DCMotorVelocityUpdateEventArgs : EventArgs {
		/// <summary>The most recent velocity value will be reported in this event.
		/// </summary>
		public readonly double Velocity;
		internal DCMotorVelocityUpdateEventArgs(double velocity) {
			this.Velocity = velocity;
		}
	}

}
