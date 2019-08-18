﻿using System;
using System.Drawing;
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
      
      var pen = new Pen(Color.Black);
      gc.Canvas.Clear();
      gc.DrawRectangle(pen, new Rectangle(0, 0, gc.Canvas.Width - 1, gc.Canvas.Height - 1));

      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        gc.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10, 20));
        gc.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10 - 5, 10));
        System.Threading.Thread.Sleep(750);
      }
      
      gc.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, gc.Canvas.Height / 2, gc.Canvas.Width, gc.Canvas.Height / 2));
      pen.Color = Color.White;
      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        gc.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10, 45));
        gc.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10 - 5, 35));
        System.Threading.Thread.Sleep(750);
      }
    }
  }
}
