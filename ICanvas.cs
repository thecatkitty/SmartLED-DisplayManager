using System.Drawing;

namespace Celones
{
    interface ICanvas
    {
        int Width { get; }
        int Height { get; }

        Graphics Graphics { get; }

        void Clear();
        void Update();
        void Update(Rectangle rect);
    }
}
