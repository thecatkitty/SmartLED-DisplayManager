using System;
using System.Drawing;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using Portable.Xaml;

namespace Celones.DisplayManager {
  class Program {
    static LcdScreen Lcd;
    static Unosquare.RaspberryIO.Peripherals.Button ButtonA, ButtonB, ButtonC, ButtonD;

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

      // Menu demo
      ButtonA = new Unosquare.RaspberryIO.Peripherals.Button(Pins.ButtonA, GpioPinResistorPullMode.PullUp);
      ButtonB = new Unosquare.RaspberryIO.Peripherals.Button(Pins.ButtonB, GpioPinResistorPullMode.PullUp);
      ButtonC = new Unosquare.RaspberryIO.Peripherals.Button(Pins.ButtonC, GpioPinResistorPullMode.PullUp);
      ButtonD = new Unosquare.RaspberryIO.Peripherals.Button(Pins.ButtonD, GpioPinResistorPullMode.PullUp);

      var ui = new ONUI.UI(AssemblyDirectory + "/Assets/MainMenu.xaml");
      ui.Lcd = Lcd;
      ui.BackButton = ButtonA;
      ui.OkButton = ButtonB;
      ui.DownButton = ButtonC;
      ui.UpButton = ButtonD;
      ui.Show();

      System.Environment.Exit(0);
    }

    static void ButtonA_Pressed(object sender, EventArgs e) {
      Lcd.Graphics.Clear(ClearColor);
      Lcd.Update();
      ClearColor = ClearColor == Color.Black ? Color.White : Color.Black;
    }

    public static string AssemblyDirectory {
      get {
        string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return System.IO.Path.GetDirectoryName(path);
      }
    }
  }
}
