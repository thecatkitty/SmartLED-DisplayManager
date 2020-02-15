using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using Portable.Xaml.Markup;

namespace Celones.ONUI
{
    public class Menu : ItemsControl
    {

        public Menu()
        {
            Items = new List<Control>();
        }

        public override void OnRender(Graphics graphics)
        {
            var font = new Font("Tahoma", 8);
            var brush = new SolidBrush(Color.Black);
            var i = 1;
            foreach (var item in Items)
            {
                brush.Color = Color.Black;
                if (item == SelectedItem)
                {
                    graphics.FillRectangle(brush, new Rectangle(0, i * 12, 84, 12));
                    brush.Color = Color.White;
                }
                var menuItem = (MenuItem)item;
                graphics.DrawString(menuItem.Header, font, brush, new PointF(0, i * 12 - 2));
                i++;
            }
        }

        public MenuItem SelectedItem { get; set; }
    }

    [ContentProperty("Header")]
    public class MenuItem : ItemsControl, IHeaderedControl
    {
        public MenuItem()
        {
            Items = new List<Control>();
        }

        public System.Windows.Input.ICommand Command { get; set; }
        public bool IsCheckable { get; set; }
        public string Header { get; set; }

        public event System.EventHandler Checked;
        public event System.EventHandler Unchecked;
        public event System.EventHandler Click;
    }
}
