using System.Drawing;

[assembly: Portable.Xaml.Markup.XmlnsDefinition("http://schemas.celones.pl/xaml/2019/onui", "Celones.ONUI")]

namespace Celones.ONUI
{
    class UI
    {
        private ONUI.Page _root;

        public ICanvas Display { get; set; }
        public IHardwareButton OkButton { get; set; }
        public IHardwareButton BackButton { get; set; }
        public IHardwareButton DownButton { get; set; }
        public IHardwareButton UpButton { get; set; }
        public ONUI.Page Root { get => _root; }

        public UI(string file)
        {
            _root = (ONUI.Page)Portable.Xaml.XamlServices.Load(file);
        }

        public void Show()
        {
            Display.Clear();
            Display.Graphics.Clear(Color.White);
            var brush = new SolidBrush(Color.Black);
            var font = new Font("Tahoma", 8, FontStyle.Bold);
            var format = new StringFormat(StringFormat.GenericDefault);
            format.Alignment = StringAlignment.Center;
            Display.Graphics.DrawString(Root.Header, font, brush, new RectangleF(0, -2, Display.Width, 12), format);

            foreach (var item in Root.Items)
            {
                if (item is ONUI.Menu)
                {
                    var menu = (ONUI.Menu)item;
                    if (menu.Items.Count > 0)
                    {
                        menu.SelectedItem = (ONUI.MenuItem)menu.Items[0];
                    }
                }
            }

            foreach (var item in Root.Items)
            {
                item.OnRender(Display.Graphics);
            }
            Display.Update();
        }
    }
}
