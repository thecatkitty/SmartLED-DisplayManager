using System;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class LcdScreen {
    private Device.Pcd8544 _ctl;
    private GpioPin _bl;

    private double _brightness, _contrast;

    public LcdScreen(Device.Pcd8544 controller, GpioPin backlight) {
      _ctl = controller;
      _bl = backlight;
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
      for (int index = 0; index < Width * Height / 8; index++) {
        _ctl.Write(0x00);
      }
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
  }
}
