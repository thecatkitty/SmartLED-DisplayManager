using System.Drawing;

namespace Celones.Drawing {
  interface ICanvas {
    int Width {get;}
    int Height {get;}

    Image Image {get;}

    void Clear();
    void Invalidate(Rectangle rect);
  }
}
