using System.Collections;
using System.Collections.Generic;

namespace Celones.ONUI
{
    public class Page : ItemsControl, IHeaderedControl
    {
        public Page()
        {
            Items = new List<Control>();
            Header = this.GetType().Name;
        }

        public string Header { get; set; }
    }
}
