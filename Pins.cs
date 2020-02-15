using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Celones.DisplayManager
{
    public static class Pins
    {
        public static IGpioPin LcdReset;
        public static IGpioPin LcdDc;
        public static IGpioPin LcdBacklight;
        public static IGpioPin ButtonA;
        public static IGpioPin ButtonB;
        public static IGpioPin ButtonC;
        public static IGpioPin ButtonD;

        public static void Init()
        {
            LcdReset = Pi.Gpio[BcmPin.Gpio04];
            LcdDc = Pi.Gpio[BcmPin.Gpio22];
            LcdBacklight = Pi.Gpio[BcmPin.Gpio18];
            ButtonA = Pi.Gpio[BcmPin.Gpio20];
            ButtonB = Pi.Gpio[BcmPin.Gpio26];
            ButtonC = Pi.Gpio[BcmPin.Gpio19];
            ButtonD = Pi.Gpio[BcmPin.Gpio00];

            LcdReset.PinMode = GpioPinDriveMode.Output;
            LcdDc.PinMode = GpioPinDriveMode.Output;
            LcdBacklight.PinMode = GpioPinDriveMode.Output;
            ButtonA.PinMode = GpioPinDriveMode.Input;
        }
    }
}
