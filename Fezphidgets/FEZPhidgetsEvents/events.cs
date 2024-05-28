using System;
using Microsoft.SPOT;

namespace FEZPhidgets.Events
{
    #region Common events
    public class PhidgetOutputChangeEventArgs
    {
        public PhidgetOutputChangeEventArgs(int eIndex, bool eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public bool Value { get; private set; }
    }

    public class PhidgetAttachEventArgs
    {
        public PhidgetAttachEventArgs(string eDeviceName, string eSerial) { DeviceName = eDeviceName; SerialNum = eSerial; }
        public string DeviceName { get; private set; }
        public string SerialNum { get; private set; }
    }

    public class PhidgetInputChangeEventArgs
    {
        public PhidgetInputChangeEventArgs(int eIndex, bool eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public bool Value { get; private set; }
    }

    public delegate void AttachEventHandler(object sender, PhidgetAttachEventArgs e);
    public delegate void OutputChangeEventHandler(object sender, PhidgetOutputChangeEventArgs e);
    public delegate void InputChangeEventHandler(object sender, PhidgetInputChangeEventArgs e);

    #endregion

    #region RFID events
    public class TagEventArgs
    {
        public TagEventArgs(string eTag) { Tag = eTag; }
        public string Tag { get; private set; }
    }
    
    public delegate void TagEventHandler(object sender, TagEventArgs e);
    #endregion

    #region 1018 Interface kit events
    public class SensorChangeEventArgs
    {
        public SensorChangeEventArgs(int eIndex, int eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public int Value { get; private set; }
    }

    public delegate void SensorChangeEventHandler(object sender, SensorChangeEventArgs e);
    
    #endregion

    #region Encoder1052 events
    public class EncoderPositionChangeEventArgs
    {
        public EncoderPositionChangeEventArgs(int ePosition, bool eDirection, sbyte eRelativeMove) { Position = ePosition; Direction = eDirection; RelativeMove = eRelativeMove; }
        public int Position { get; private set; }
        public bool Direction { get; private set; }
        public int RelativeMove { get; private set; }
    }
    public delegate void PositionChangeEventHandler(object sender, EncoderPositionChangeEventArgs e);

    #endregion
}
