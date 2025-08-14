//using VNCPhidget22Explorer.Domain;

namespace VNCPhidget22Explorer.Core.Events
{
    public class SelectedCollectionChangedEventArgs
    {
        public string Name { get; set; }
        //public AvailableCollection Collection;
    }

    public class AvailableCollection
    {
        public string Name { get; set; }
        //public Organization Organization { get; set; }
    }
}
