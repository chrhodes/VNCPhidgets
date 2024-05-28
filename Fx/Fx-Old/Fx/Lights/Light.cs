using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fx.Lights
{
    public class Light
    {

        private string _DisplayLightId;
        public string DisplayLightId
        {
            get { return _DisplayLightId; }
            set
            {
                _DisplayLightId = value;
            }
        }

        private bool _IsOn;
        public bool IsOn
        {
            get { return _IsOn; }
            set
            {
                _IsOn = value;
            }
        }

    }
}
