using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static void Main(string[] args) {
      Pi.Init<BootstrapWiringPi>();
      Console.WriteLine("Celones SmartLED Display Manager");
      using (var systemInfo = Unosquare.RaspberryIO.Computer.SystemInfo.Instance)
      {
        Console.WriteLine(string.Format(
          "Raspberry Pi model {0} ({1}), {2} operating system",
          systemInfo.BoardModel, systemInfo.MemorySize, systemInfo.OperatingSystem.SysName));
      }
      
      var reset = Pi.Gpio[BcmPin.Gpio04];
      var lcdDc = Pi.Gpio[BcmPin.Gpio22];
      var backlight = Pi.Gpio[BcmPin.Gpio18];

      var pcd8544 = new Celones.Device.Pcd8544(Pi.Spi.Channel0, reset, lcdDc);
      var lcd = new LcdScreen(pcd8544, (GpioPin)backlight);

      reset.PinMode = GpioPinDriveMode.Output;
      lcdDc.PinMode = GpioPinDriveMode.Output;
      backlight.PinMode = GpioPinDriveMode.Output;

      lcd.Init();
      lcd.Clear();

      for(double brightness = 0.0; brightness <= 1.0; brightness += 0.1) {
        lcd.Brightness = brightness;
        lcd.Contrast = brightness;
        System.Threading.Thread.Sleep(50);
      }
      
      lcd.Clear();
      int x = 0, y = 0, vx = 1, vy = 1;
      for(int i = 0; i < lcd.Width * lcd.Height; i++) {
        lcd[x, y] = 1.0 - lcd[x, y];
        x += vx;
        y += vy;
        if(x == lcd.Width - 1) vx = -1;
        if(y == lcd.Height - 1) vy = -1;
        if(x == 0) vx = 1;
        if(y == 0) vy = 1;
        System.Threading.Thread.Sleep(10);
      }
    }
  }
}
