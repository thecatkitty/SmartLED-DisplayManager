using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Celones.DisplayManager
{
    class Program
    {
        static LcdScreen Lcd;
        static ONUI.IHardwareButton ButtonA, ButtonB, ButtonC, ButtonD;

        static Color ClearColor;

        static void Main(string[] args)
        {
            Console.WriteLine("Celones SmartLED Display Manager");

            // Simulation initialization
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                var ctl = new Simulation.Nokia5110Lcd();
                var sim = new Simulation.ControlPanel();

                Console.WriteLine("Simulator for Windows");
                Thread thread = new Thread((object form) => System.Windows.Forms.Application.Run((System.Windows.Forms.Form)form));
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = false;
                thread.Start(sim);

                Lcd = new LcdScreen(new Device.Pcd8544(ctl, ctl, ctl));
                Lcd.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                ButtonA = sim.aButton;
                ButtonB = sim.bButton;
                ButtonC = sim.cButton;
                ButtonD = sim.dButton;

                sim.screenImage.Image = ctl.Image;
                sim.screenImage.BackColor = Color.CadetBlue;
            }

            // Hardware initialization
            else
            {
                Pi.Init<BootstrapWiringPi>();
                Pins.Init();
                Console.WriteLine(Pi.Info.ToString());

                Lcd = new LcdScreen(new Device.Pcd8544(Pi.Spi.Channel0, Pins.LcdReset, Pins.LcdDc), (GpioPin)Pins.LcdBacklight);
                ButtonA = new Button(Pins.ButtonA);
                ButtonB = new Button(Pins.ButtonB);
                ButtonC = new Button(Pins.ButtonC);
                ButtonD = new Button(Pins.ButtonD);
            }

            ButtonA.Pressed += ButtonA_Pressed;

            Lcd.Init();
            Lcd.Clear();
            for (double brightness = 0.0; brightness <= 1.0; brightness += 0.1)
            {
                Lcd.Brightness = brightness;
                Lcd.Contrast = brightness;
                Thread.Sleep(50);
            }

            // Graphics demo
            var pen = new Pen(Color.Black);
            var brush = new SolidBrush(Color.Black);
            Lcd.Clear();
            Lcd.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Lcd.Width - 1, Lcd.Height - 1));
            Lcd.Update();

            for (int i = 1; i <= 6; i++)
            {
                pen.Width = i;
                Lcd.Graphics.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10, 20));
                Lcd.Graphics.DrawLine(pen, new Point(i * 10, 5), new Point(i * 10 - 5, 10));
                Lcd.Update();
                Thread.Sleep(750);
            }

            Lcd.Graphics.FillRectangle(brush, new Rectangle(0, Lcd.Height / 2, Lcd.Width, Lcd.Height / 2));

            pen.Color = Color.White;
            for (int i = 1; i <= 6; i++)
            {
                pen.Width = i;
                Lcd.Graphics.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10, 45));
                Lcd.Graphics.DrawLine(pen, new Point(i * 10, 30), new Point(i * 10 - 5, 35));
                Lcd.Update();
                Thread.Sleep(750);
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
                  Thread.Sleep(1500);
              });

            // Menu demo
            var ui = new ONUI.UI(AssemblyDirectory + "/Assets/MainMenu.xaml");
            ui.Display = Lcd;
            ui.BackButton = ButtonA;
            ui.OkButton = ButtonB;
            ui.DownButton = ButtonC;
            ui.UpButton = ButtonD;
            ui.Show();
        }

        static void ButtonA_Pressed(object sender, EventArgs e)
        {
            Lcd.Graphics.Clear(ClearColor);
            Lcd.Update();
            ClearColor = ClearColor == Color.Black ? Color.White : Color.Black;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }
    }
}
