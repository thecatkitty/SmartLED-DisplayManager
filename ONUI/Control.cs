using System.Collections;
using Portable.Xaml.Markup;

namespace Celones.ONUI {
  public class Control {
    public virtual void OnRender(System.Drawing.Graphics graphics) {}
  }

  [ContentProperty("Content")]
  public class ContentControl : Control {
    public ContentControl() {}

    public object Content {get; set;}
  }

  [ContentProperty("Items")]
  public class ItemsControl : Control {
    public ItemsControl() {}

    public IList Items {get; set;}
  }

  public interface IHeaderedControl {
    string Header {get; set;}
  }
}
