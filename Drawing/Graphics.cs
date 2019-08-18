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

    public void DrawRectangle(Pen pen, Rectangle rectangle) {
      for (int x = 0; x < rectangle.Width; x++) {
        pen.Apply(Canvas, new Point(rectangle.Left + x, rectangle.Top));
        pen.Apply(Canvas, new Point(rectangle.Left + x, rectangle.Top + rectangle.Height - 1));
      }
      
      for (int y = 0; y < rectangle.Height; y++) {
        pen.Apply(Canvas, new Point(rectangle.Left, rectangle.Top + y));
        pen.Apply(Canvas, new Point(rectangle.Left + rectangle.Width - 1, rectangle.Top + y));
      }
      
      Canvas.Invalidate(new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1));
      Canvas.Invalidate(new Rectangle(rectangle.Left, rectangle.Top + rectangle.Height - 1, rectangle.Width, 1));

      Canvas.Invalidate(new Rectangle(rectangle.Left, rectangle.Top + 1, 1, rectangle.Height - 2));
      Canvas.Invalidate(new Rectangle(rectangle.Left + rectangle.Width - 1, rectangle.Top + 1, 1, rectangle.Height - 2));
    }

    public void FillRectangle(byte color, Rectangle rectangle) {
      for (int x = 0; x < rectangle.Width; x++) {
        for (int y = 0; y < rectangle.Height; y++) {
          Canvas[rectangle.Left + x, rectangle.Top + y] = color;
        }
      }
      Canvas.Invalidate(rectangle);
    }
  }
}
