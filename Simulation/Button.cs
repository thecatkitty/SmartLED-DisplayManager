using System;
using System.Windows.Forms;
using Celones.ONUI;

namespace Celones.DisplayManager.Simulation
{
    public class Button : System.Windows.Forms.Button, IHardwareButton
    {
        public event EventHandler<HardwareButtonEventArgs> Pressed;
        public event EventHandler<HardwareButtonEventArgs> Released;

        protected System.Diagnostics.Stopwatch _stopwatch;

        public Button() : base()
        {
            _stopwatch = new System.Diagnostics.Stopwatch();
            _stopwatch.Start();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();
            OnPressed(new HardwareButtonEventArgs()
            {
                EventType = HardwareButtonEventType.Press,
                ElapsedMilliseconds = elapsedMilliseconds
            });
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();
            OnReleased(new HardwareButtonEventArgs()
            {
                EventType = HardwareButtonEventType.Release,
                ElapsedMilliseconds = elapsedMilliseconds
            });
            base.OnMouseUp(mevent);
        }

        protected virtual void OnPressed(HardwareButtonEventArgs e)
        {
            Pressed?.Invoke(this, e);
        }

        protected virtual void OnReleased(HardwareButtonEventArgs e)
        {
            Released?.Invoke(this, e);
        }

        public void Press()
        {
            OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, 5, 5, 0));
        }

        public void Release()
        {
            OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, 5, 5, 0));
        }
    }
}
