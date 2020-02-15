using System;

namespace Celones.ONUI
{
    interface IHardwareButton
    {
        event EventHandler<HardwareButtonEventArgs> Pressed;
        event EventHandler<HardwareButtonEventArgs> Released;
    }

    public enum HardwareButtonEventType
    {
        Press,
        Release
    }

    public class HardwareButtonEventArgs : EventArgs
    {
        public HardwareButtonEventType EventType { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
