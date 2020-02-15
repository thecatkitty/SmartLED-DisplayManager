using System;
using System.Drawing;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class LcdScreen : ICanvas {
    private Device.Pcd8544 _ctl;
    private GpioPin _bl;

    private double _brightness, _contrast;

    private Bitmap _img;
    private Graphics _gc;

    public LcdScreen(Device.Pcd8544 controller, GpioPin backlight = null) {
      _ctl = controller;
      _bl = backlight;
      
      _img = new Bitmap(Width, Height);
      _gc = Graphics.FromImage(_img);
    }

    public void Init() {
      _ctl.Init();

      if (_bl != null)
      {
        _bl.PinMode = GpioPinDriveMode.PwmOutput;
        _bl.PwmMode = PwmMode.Balanced;
        _bl.PwmClockDivisor = 2;
      }

      Brightness = 1.0;
      Contrast = 1.0;
    }

    public void Clear() {
      for (int index = 0; index < _ctl.DramSizeX * _ctl.DramSizeY; index++) {
        _ctl.Write(0x00);
      }
      for (int x = 0; x < Width; x++) {
        for (int y = 0; y < Height; y++) {
          _img.SetPixel(x, y, Color.White);
        }
      }
    }

    public int Width { get => _ctl.DramSizeX; }
    public int Height { get => _ctl.DramSizeY * 8; }

    public double Brightness
    {
        get { return _brightness; }
        set {
            if (_bl != null)
            {
                _brightness = Math.Min(Math.Max(value, 0.0), 1.0);
                _bl.PwmRegister = (int)(_brightness * _bl.PwmRange);
            }
        }
    }

    public double Contrast
    {
      get { return _contrast; }
      set {
        _contrast = Math.Min(Math.Max(value, 0.0), 1.0);
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationMode(instructionSet: Device.Pcd8544.InstructionSet.Extended));
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationVoltage((int)(60 * _contrast)));
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationMode(instructionSet: Device.Pcd8544.InstructionSet.Basic));
      }
    }

    public void Update() {
      Update(new Rectangle(0, 0, Width, Height));
    }

    public void Update(Rectangle rect) {
      rect.Intersect(new Rectangle(0, 0, Width, Height));
      int yFirst = rect.Top / 8;
      int yLast = (int)Math.Ceiling((double)(rect.Top + rect.Height) / 8.0);

      for(int y = yFirst; y < yLast; y++) {
        _ctl.Write(Device.Pcd8544.Instruction.SetYAddress(y));
        _ctl.Write(Device.Pcd8544.Instruction.SetXAddress(rect.Left));
        for(int i = 0; i < rect.Width; i++) {
          int data = 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 0).GetBrightness() < 0.5 ? 1 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 1).GetBrightness() < 0.5 ? 2 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 2).GetBrightness() < 0.5 ? 4 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 3).GetBrightness() < 0.5 ? 8 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 4).GetBrightness() < 0.5 ? 16 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 5).GetBrightness() < 0.5 ? 32 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 6).GetBrightness() < 0.5 ? 64 : 0;
          data |= _img.GetPixel(rect.Left + i, y * 8 + 7).GetBrightness() < 0.5 ? 128 : 0;
          _ctl.Write((byte)data);
        }
      }
    }

    public Graphics Graphics {
      get => _gc;
    }
  }
}
