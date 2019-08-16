using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static void Main(string[] args) {
      Pi.Init<BootstrapWiringPi>();
      Console.WriteLine("Celones SmartLED Display Manager");
      
      var reset = Pi.Gpio[BcmPin.Gpio04];
      reset.PinMode = GpioPinDriveMode.Output;
      var lcdDc = Pi.Gpio[BcmPin.Gpio22];
      lcdDc.PinMode = GpioPinDriveMode.Output;
      var backlight = Pi.Gpio[BcmPin.Gpio18];
      backlight.PinMode = GpioPinDriveMode.Output;

      var lcd = new Celones.Device.Pcd8544(Pi.Spi.Channel0, reset, lcdDc, (GpioPin)backlight);
      lcd.Init();
      lcd.Clear();
      
      double brightness = 0.0;
      for(int i = 0; i < lcd.ScreenWidth * lcd.ScreenHeight / 8; i++) {
        lcd.Write(Celones.Device.Pcd8544.PayloadType.Data, 0x55);
        if(brightness < 1.0) {
          lcd.Brightness = brightness;
          lcd.Contrast = brightness;
          brightness += 0.01;
        }
        System.Threading.Thread.Sleep(50);
      }
    }
  }
}
