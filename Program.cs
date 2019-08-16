using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static void Main(string[] args) {
      Pi.Init<BootstrapWiringPi>();
      Console.WriteLine("Hello there~ :3");
      
      var reset = Pi.Gpio[BcmPin.Gpio04];
      reset.PinMode = GpioPinDriveMode.Output;
      var lcdDc = Pi.Gpio[BcmPin.Gpio22];
      lcdDc.PinMode = GpioPinDriveMode.Output;
      var backlight = Pi.Gpio[BcmPin.Gpio18];
      backlight.PinMode = GpioPinDriveMode.Output;

      var lcd = new Celones.Device.Pcd8544(Pi.Spi.Channel0, reset, lcdDc, (GpioPin)backlight);
      lcd.Init();

      double br = 0.0;
      while(true) {
        System.Threading.Thread.Sleep(50);
        lcd.Write(Celones.Device.Pcd8544.PayloadType.Data, 0x55);
        lcd.Brightness = br;
        br += 0.01;
        if(br > 0.99) {
          br = 0;
        }
      }
    }
  }
}
