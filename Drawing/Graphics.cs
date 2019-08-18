using System;
using System.Drawing;

namespace Celones.Drawing {
  class Graphics {
    private System.Drawing.Graphics _gc;

    public ICanvas Canvas {get; set;}

    public Graphics(ICanvas canvas) {
      Canvas = canvas;
      _gc = System.Drawing.Graphics.FromImage(Canvas.Image);
    }

    public void DrawLine(Pen pen, Point pt1, Point pt2) {
      _gc.DrawLine(pen, pt1, pt2);
      var rect = Rectangle.FromLTRB(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y), Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));
      Canvas.Invalidate(Rectangle.Inflate(rect, (int)pen.Width, (int)pen.Width));
    }

    public void DrawRectangle(Pen pen, Rectangle rect) {
      _gc.DrawRectangle(pen, rect);
      Canvas.Invalidate(Rectangle.Inflate(rect, (int)pen.Width, (int)pen.Width));
    }

    public void FillRectangle(Brush brush, Rectangle rect) {
      _gc.FillRectangle(brush, rect);
      Canvas.Invalidate(rect);
    }
  }
}
