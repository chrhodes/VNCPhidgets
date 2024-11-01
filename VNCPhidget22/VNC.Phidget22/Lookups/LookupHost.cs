using VNC.Core.DomainServices;

namespace VNC.Phidget22.Lookups
{
    public class LookupHost : ILookupItem<int>
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
