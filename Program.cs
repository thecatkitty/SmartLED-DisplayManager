﻿using System;
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
      var gc = new Drawing.Graphics(lcd);

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
      
      gc.Canvas.Clear();
      var rand = new Random();
      var pen = new Drawing.Pen(1);
      for (int i = 0; i < 5; i++) {
        int x1 = rand.Next(gc.Canvas.Width - 1);
        int x2 = rand.Next(gc.Canvas.Width - 1);
        int y1 = rand.Next(gc.Canvas.Height - 1);
        int y2 = rand.Next(gc.Canvas.Height - 1);
        pen.Size = i + 1;
        gc.DrawLine(pen, new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y2));
        System.Threading.Thread.Sleep(750);
      }
    }
  }
}
