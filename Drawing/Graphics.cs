using System;
using System.Drawing;

namespace Celones.Drawing {
  class Graphics {
    public ICanvas Canvas {get; set;}

    public Graphics(ICanvas canvas) {
      Canvas = canvas;
    }

    public void DrawLine(Pen pen, Point pt1, Point pt2) {
      if(Math.Abs(pt1.X - pt2.X) > Math.Abs(pt1.Y - pt2.Y)) {
        var a = (double)(pt1.Y - pt2.Y) / (double)(pt1.X - pt2.X);
        var b = pt2.Y - a * pt2.X;

        for(int x = Math.Min(pt1.X, pt2.X); x <= Math.Max(pt1.X, pt2.X); x++) {
          pen.Apply(Canvas, new Point(x, (int)Math.Round(a * x + b)));
        }
      } else {
        var a = (double)(pt1.X - pt2.X) / (double)(pt1.Y - pt2.Y);
        var b = pt2.X - a * pt2.Y;

        for(int y = Math.Min(pt1.Y, pt2.Y); y <= Math.Max(pt1.Y, pt2.Y); y++) {
          pen.Apply(Canvas, new Point((int)Math.Round(a * y + b), y));
        }
      }

      var rect = Rectangle.FromLTRB(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y), Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));
      Canvas.Invalidate(Rectangle.Inflate(rect, pen.Size, pen.Size));
    }
  }
}
