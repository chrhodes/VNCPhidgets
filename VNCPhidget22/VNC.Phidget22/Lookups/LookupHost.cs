using System;

using VNC.Core.DomainServices;

namespace VNC.Phidget22.Lookups
{
    public class LookupHost : ILookupItem<Int32>
    {
        public Int32 Id { get; set; }
        public string? DisplayMember { get; set; }
    }
}
