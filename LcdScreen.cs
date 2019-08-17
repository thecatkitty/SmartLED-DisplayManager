using System;
using System.Drawing;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class LcdScreen {
    private Device.Pcd8544 _ctl;
    private GpioPin _bl;

    private double _brightness, _contrast;

    private double[] _bmp;

    public LcdScreen(Device.Pcd8544 controller, GpioPin backlight) {
      _ctl = controller;
      _bl = backlight;

      _bmp = new double[Width * Height];
      Array.Fill(_bmp, 0.0);
    }

    public void Init() {
      _ctl.Init();

      _bl.PinMode = GpioPinDriveMode.PwmOutput;
      _bl.PwmMode = PwmMode.Balanced;
      _bl.PwmClockDivisor = 2;

      Brightness = 1.0;
      Contrast = 1.0;
    }

    public void Clear() {
      for (int index = 0; index < _ctl.DramSizeX * _ctl.DramSizeY; index++) {
        _ctl.Write(0x00);
      }
      Array.Fill(_bmp, 0.0);
    }

    public int Width { get => _ctl.DramSizeX; }
    public int Height { get => _ctl.DramSizeY * 8; }

    public double Brightness
    {
        get { return _brightness; }
        set {
          _brightness = Math.Clamp(value, 0.0, 1.0);
          _bl.PwmRegister = (int)(_brightness * _bl.PwmRange);
        }
    }

    public double Contrast
    {
      get { return _contrast; }
      set {
        _contrast = Math.Clamp(value, 0.0, 1.0);
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationMode(instructionSet: Device.Pcd8544.InstructionSet.Extended));
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationVoltage((int)(60 * _contrast)));
        _ctl.Write(Device.Pcd8544.Instruction.SetOperationMode(instructionSet: Device.Pcd8544.InstructionSet.Basic));
      }
    }

    private void Update(Rectangle rect) {
      int yFirst = rect.Top / 8;
      int yLast = (int)Math.Ceiling((double)(rect.Top + rect.Height) / 8.0);

      for(int y = yFirst; y < yLast; y++) {
        _ctl.Write(Device.Pcd8544.Instruction.SetYAddress(y));
        _ctl.Write(Device.Pcd8544.Instruction.SetXAddress(rect.Left));
        for(int i = 0; i < rect.Width; i++) {
          int data = 0;
          data |= _bmp[rect.Left + i + (y * 8) * Width] >= 0.5 ? 1 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 1) * Width] >= 0.5 ? 2 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 2) * Width] >= 0.5 ? 4 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 3) * Width] >= 0.5 ? 8 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 4) * Width] >= 0.5 ? 16 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 5) * Width] >= 0.5 ? 32 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 6) * Width] >= 0.5 ? 64 : 0;
          data |= _bmp[rect.Left + i + (y * 8 + 7) * Width] >= 0.5 ? 128 : 0;
          _ctl.Write((byte)data);
        }
      }
    }

    public double this[int x, int y] {
      get => _bmp[x + y * Width];
      set {
        _bmp[x + y * Width] = value;
        Update(new Rectangle(x, y, 1, 1));
      }
    }
  }
}
