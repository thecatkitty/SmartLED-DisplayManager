using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Celones.DisplayManager.Simulation
{  
    public class PixelBox : PictureBox
    { 
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.OnPaint(paintEventArgs);
        }
    }
}

