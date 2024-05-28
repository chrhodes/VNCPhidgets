using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fx.Lights
{
    interface ILight
    {
        string Id { get; set; }
        string Color { get; set; }
        void On();
        void Off();

        // Does the light know how to blink?
        // Does the light have on/off duration?

        // Or are these notions sub-class behaviors?
    }
}
