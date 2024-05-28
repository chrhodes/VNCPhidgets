using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace DebugLightGridApplication
{
    [ServiceContract(Namespace="http://crhodes.com")]
    public interface IDisplayLight
    {
        [OperationContract()]
        void On(int displayLightId);

        [OperationContract()]
        void Off(int displayLightId);
    }
}