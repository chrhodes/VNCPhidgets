using System;
using System.Reflection;

namespace Phidget22 {
	public partial class Manager {
		internal IntPtr chandle;

		static internal Phidget CreateTypedManagerPhid(Phidget phid) {
			Phidget phidRet;

			if (phid.IsChannel) {
				// For channels, we retain. Release happens in finalizer
				Phidget22Imports.Phidget_retain(phid.chandle);

				Type phidType = Type.GetType("Phidget22." + phid.ChannelClass.ToString());
				phidRet = (Phidget)Activator.CreateInstance(phidType);
#if DOTNET_FRAMEWORK
				MethodInfo deleteMethod = typeof(Phidget22Imports).GetMethod("Phidget" + phid.ChannelClass.ToString() + "_delete", new Type[] { typeof(IntPtr).MakeByRefType() });
#else
				MethodInfo deleteMethod = typeof(Phidget22Imports).GetTypeInfo().GetDeclaredMethod("Phidget" + phid.ChannelClass.ToString() + "_delete");
#endif
				deleteMethod.Invoke(null, new object[] { phidRet.chandle });
			} else {
				// For devices, it's already retained.
				phidRet = phid;
			}
			
			phidRet.chandle = phid.chandle;
			phidRet.managerPhidget = true;

			return phidRet;
		}
	}
}
