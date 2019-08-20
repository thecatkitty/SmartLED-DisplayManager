using System;
using System.Drawing;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static void Main(string[] args) {
      // Hardware initialization
      Pi.Init<BootstrapWiringPi>();
      Console.WriteLine("Celones SmartLED Display Manager");
      Console.WriteLine(Pi.Info.ToString());
      
      var reset = Pi.Gpio[BcmPin.Gpio04];
      var lcdDc = Pi.Gpio[BcmPin.Gpio22];
      var backlight = Pi.Gpio[BcmPin.Gpio18];
      var butt1 = Pi.Gpio[BcmPin.Gpio20];

      var pcd8544 = new Celones.Device.Pcd8544(Pi.Spi.Channel0, reset, lcdDc);
      var lcd = new LcdScreen(pcd8544, (GpioPin)backlight);
      var gc = new Drawing.Graphics(lcd);

      reset.PinMode = GpioPinDriveMode.Output;
      lcdDc.PinMode = GpioPinDriveMode.Output;
      backlight.PinMode = GpioPinDriveMode.Output;
      butt1.PinMode = GpioPinDriveMode.Input;

      lcd.Init();
      lcd.Clear();
      for(double brightness = 0.0; brightness <= 1.0; brightness += 0.1) {
        lcd.Brightness = brightness;
        lcd.Contrast = brightness;
        System.Threading.Thread.Sleep(50);
      }
      
      // Graphics demo
      var pen = new Pen(Color.Black);
      var brush = new SolidBrush(Color.Black);
      gc.Canvas.Clear();
      gc.DrawRectangle(pen, new Rectangle(0, 0, gc.Canvas.Width - 1, gc.Canvas.Height - 1));

      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        gc.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10, 20));
        gc.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10 - 5, 10));
        System.Threading.Thread.Sleep(750);
      }
      
      gc.FillRectangle(brush, new Rectangle(0, gc.Canvas.Height / 2, gc.Canvas.Width, gc.Canvas.Height / 2));
      pen.Color = Color.White;
      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        gc.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10, 45));
        gc.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10 - 5, 35));
        System.Threading.Thread.Sleep(750);
      }

      // Font demo
      var ifc = new System.Drawing.Text.InstalledFontCollection();
      for (int i = 0; i < ifc.Families.Length; i++) {
        var fontFamily = ifc.Families[i];
        if (fontFamily.IsStyleAvailable(FontStyle.Regular)) {
          var font = new Font(fontFamily, 8);
          gc.Canvas.Clear();
          gc.GdiPlus.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
          gc.GdiPlus.DrawString(fontFamily.Name, font, brush, new PointF(0, 12));
          gc.Canvas.Invalidate(new Rectangle(0, 0, gc.Canvas.Width, gc.Canvas.Height));
          System.Threading.Thread.Sleep(1500);
        }
      }

      // Buttons demo
      var color = Color.White;
      var button = new Unosquare.RaspberryIO.Peripherals.Button(butt1, GpioPinResistorPullMode.PullUp); // 20, 26, 19, 0
      button.Pressed += (object sender, EventArgs e) => {
        gc.GdiPlus.Clear(color);
        gc.Canvas.Invalidate(new Rectangle(0, 0, gc.Canvas.Width, gc.Canvas.Height));
        color = color == Color.Black ? Color.White : Color.Black;
      };
      System.Threading.Thread.Sleep(5000);
    }
  }
}
