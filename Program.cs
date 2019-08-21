using System;
using System.Drawing;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static LcdScreen Lcd;
    static Unosquare.RaspberryIO.Peripherals.Button ButtonA;

    static Color ClearColor;

    static void Main(string[] args) {
      // Hardware initialization
      Pi.Init<BootstrapWiringPi>();
      Pins.Init();
      Console.WriteLine("Celones SmartLED Display Manager");
      Console.WriteLine(Pi.Info.ToString());
      
      Lcd = new LcdScreen(new Celones.Device.Pcd8544(Pi.Spi.Channel0, Pins.LcdReset, Pins.LcdDc), (GpioPin)Pins.LcdBacklight);

      Lcd.Init();
      Lcd.Clear();
      for(double brightness = 0.0; brightness <= 1.0; brightness += 0.1) {
        Lcd.Brightness = brightness;
        Lcd.Contrast = brightness;
        System.Threading.Thread.Sleep(50);
      }
      
      // Graphics demo
      var pen = new Pen(Color.Black);
      var brush = new SolidBrush(Color.Black);
      Lcd.Clear();
      Lcd.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Lcd.Width - 1, Lcd.Height - 1));
      Lcd.Update();

      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        Lcd.Graphics.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10, 20));
        Lcd.Graphics.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10 - 5, 10));
        Lcd.Update();
        System.Threading.Thread.Sleep(750);
      }
      
      Lcd.Graphics.FillRectangle(brush, new Rectangle(0, Lcd.Height / 2, Lcd.Width, Lcd.Height / 2));

      pen.Color = Color.White;
      for (int i = 1; i <= 6; i++) {
        pen.Width = i;
        Lcd.Graphics.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10, 45));
        Lcd.Graphics.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10 - 5, 35));
        Lcd.Update();
        System.Threading.Thread.Sleep(750);
      }

      // Font demo
      var fonts = new System.Drawing.Text.InstalledFontCollection();
      fonts.Families
        .Where(family => family.IsStyleAvailable(FontStyle.Regular))
        .OrderBy(family => family.Name)
        .ToList().ForEach(family => {
          var font = new Font(family, 8);
          Lcd.Clear();
          Lcd.Graphics.DrawString(family.Name, font, brush, new RectangleF(0, 0, Lcd.Width, Lcd.Height));
          Lcd.Update();
          System.Threading.Thread.Sleep(1500);
        });

      // Buttons demo
      ClearColor = Color.White;
      ButtonA = new Unosquare.RaspberryIO.Peripherals.Button(Pins.ButtonA, GpioPinResistorPullMode.PullUp);
      ButtonA.Pressed += ButtonA_Pressed;
      System.Threading.Thread.Sleep(5000);
      System.Environment.Exit(0);
    }

    static void ButtonA_Pressed(object sender, EventArgs e) {
      Lcd.Graphics.Clear(ClearColor);
      Lcd.Update();
      ClearColor = ClearColor == Color.Black ? Color.White : Color.Black;
    }
  }
}
