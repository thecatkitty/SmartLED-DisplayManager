using System.Drawing;

namespace Celones.Drawing {
  interface ICanvas {
    int Width {get;}
    int Height {get;}

    byte this[int x, int y] {get; set;}

    void Clear();
    void Invalidate(Rectangle rect);
  }
}
