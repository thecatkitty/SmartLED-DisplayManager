using System.Collections;

namespace Celones.ONUI {
  public class Page : ItemsControl, IHeaderedControl {
    public Page() {
      Items = new ArrayList();
      Header = this.GetType().Name;
    }

    public string Header {get; set;}
  }
}
