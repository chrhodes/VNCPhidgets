using System;
using System.Collections.Generic;
using System.Text;

namespace Phidget22 {
	/// <summary>
	/// This class represents Phidget related exceptions.
	/// </summary>
	/// <remarks>
	/// All Phidget exceptions originate in the phidget22 C library. These exceptions can be thrown by most function in the library and
	/// cover such things as trying to access a Phidget before opening it, or before it is attached and ready to use,
	/// out of bounds Index and data values, and other less common problems.
	/// </remarks>
	public class PhidgetException : System.Exception {
		ErrorCode errCode = ErrorCode.Success;
		String desc = "Uninitialized Error";

		#region Constructor
		private PhidgetException(ErrorCode code)
			: base("PhidgetException 0x" + ((int)code).ToString("x8") + " (" + GetErrorDesc(code) + ")") {
			errCode = code;
			desc = GetErrorDesc(code);
		}
		#endregion

		/// <summary>
		/// Creates an exception of the specified type
		/// </summary>
		public static PhidgetException CreateByCode(ErrorCode code) {
			return new PhidgetException(code);
		}

		#region Properties
		/// <summary>
		/// Gets the error code.
		/// </summary>
		public ErrorCode ErrorCode {
			get {
				return errCode;
			}
		}

		/// <summary>
		/// Gets the error description.
		/// </summary>
		/// <remarks>
		/// This is an English phrase that describes the exception that occured.
		/// </remarks>
		public String Description {
			get {
				return desc;
			}
		}
		#endregion

		internal static String GetErrorDesc(ErrorCode code) {
			ErrorCode result;
			IntPtr val;
			result = Phidget22Imports.Phidget_getErrorDescription(code, out val);
			if (result != 0)
				throw PhidgetException.CreateByCode(result);
			return UTF8Marshaler.Instance.MarshalNativeToManaged(val);
		}
	}
}
