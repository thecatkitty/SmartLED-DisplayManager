using System;
using System.Drawing;

namespace Celones.Drawing {
  class Pen {
    public byte Color {get; set; }
    public int Size {get; set;}

    public Pen(byte color = 1, int size = 1) {
      Color = color;
      Size = size;
    }

    public void Apply(ICanvas canvas, Point point) {
      var left = Math.Max(point.X - Size / 2, 0);
      var right = Math.Min(point.X + Size / 2, canvas.Width - 1);
      var top = Math.Max(point.Y - Size / 2, 0);
      var bottom = Math.Min(point.Y + Size / 2, canvas.Height - 1);

      var c = Size % 2 == 1 ? 0.0 : 0.5;

      for(int x = left; x <= right; x++) {
        for(int y = top; y <= bottom; y++) {
          var dx = x - point.X - c;
          var dy = y - point.Y - c;
          if ((dx * dx + dy * dy) <= (Size * Size / 4.0)) {
            canvas[x, y] = Color;
          }
        }
      }
    }
  }
}
