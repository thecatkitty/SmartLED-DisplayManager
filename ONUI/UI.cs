using System.Drawing;
using Celones.DisplayManager;
using Unosquare.RaspberryIO.Peripherals;

[assembly: Portable.Xaml.Markup.XmlnsDefinition("http://schemas.celones.pl/xaml/2019/onui", "Celones.ONUI")]

namespace Celones.ONUI {
  class UI {
    private ONUI.Page _root;

    public LcdScreen Lcd {get; set;}
    public Button OkButton {get; set;}
    public Button BackButton {get; set;}
    public Button DownButton {get; set;}
    public Button UpButton {get; set;}
    public ONUI.Page Root { get => _root; }

    public UI(string file) {
      _root = (ONUI.Page)Portable.Xaml.XamlServices.Load(file);
    }

    public void Show() {
      Lcd.Clear();
      Lcd.Graphics.Clear(Color.White);
      var brush = new SolidBrush(Color.Black);
      var font = new Font("Tahoma", 8, FontStyle.Bold);
      var format = new StringFormat(StringFormat.GenericDefault);
      format.Alignment = StringAlignment.Center;
      Lcd.Graphics.DrawString(Root.Header, font, brush, new RectangleF(0, -2, Lcd.Width, 12), format);

      ((ONUI.Menu)Root.Items[0]).SelectedItem = (MenuItem)((ONUI.Menu)Root.Items[0]).Items[0];
      ((Control)Root.Items[0]).OnRender(Lcd.Graphics);
      Lcd.Update();
    }
  }
}
