using System;
using Unosquare.RaspberryIO.Abstractions;

namespace Celones.DisplayManager
{
    class Button : ONUI.IHardwareButton
    {
        public event EventHandler<ONUI.HardwareButtonEventArgs> Pressed;
        public event EventHandler<ONUI.HardwareButtonEventArgs> Released;

        protected System.Diagnostics.Stopwatch _stopwatch;
        protected Unosquare.RaspberryIO.Peripherals.Button _button;

        public Button(IGpioPin gpioPin)
        {
            _stopwatch = new System.Diagnostics.Stopwatch();
            _stopwatch.Start();

            _button = new Unosquare.RaspberryIO.Peripherals.Button(gpioPin, GpioPinResistorPullMode.PullUp);
            _button.Pressed += button_Pressed;
            _button.Released += button_Released;
        }

        private void button_Pressed(object sender, EventArgs e)
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();
            OnReleased(new ONUI.HardwareButtonEventArgs()
            {
                EventType = ONUI.HardwareButtonEventType.Press,
                ElapsedMilliseconds = elapsedMilliseconds
            });
        }

        private void button_Released(object sender, EventArgs e)
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();
            OnPressed(new ONUI.HardwareButtonEventArgs()
            {
                EventType = ONUI.HardwareButtonEventType.Release,
                ElapsedMilliseconds = elapsedMilliseconds
            });
        }

        protected virtual void OnPressed(ONUI.HardwareButtonEventArgs e)
        {
            Pressed?.Invoke(this, e);
        }

        protected virtual void OnReleased(ONUI.HardwareButtonEventArgs e)
        {
            Released?.Invoke(this, e);
        }
    }
}
