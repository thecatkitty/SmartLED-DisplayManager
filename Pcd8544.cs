using System;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.Device {
  public class Pcd8544 {
    private ISpiChannel _spi;
    private IGpioPin _res, _dc;
    private GpioPin _bl;

    private double _brightness, _contrast;

    public int ScreenWidth { get => 84; }
    public int ScreenHeight { get => 48; }

    public enum PayloadType {
      Command = 0,
      Data = 1
    }

    public enum PowerMode {
      Active = 0,
      PowerDown = 1
    }

    public enum AddressingMode {
      Horizontal = 0,
      Vertical = 1
    }

    public enum InstructionSet {
      Basic = 0,
      Extended = 1
    }

    public enum DisplayMode {
      Blank = 0,
      AllOn = 1,
      Normal = 2,
      Inverse = 3
    }

    public class Instruction {
      public static byte NoOperation() => 0;
      public static byte SetOperationMode(PowerMode powerMode = PowerMode.Active, AddressingMode addressingMode = AddressingMode.Horizontal, InstructionSet instructionSet = InstructionSet.Basic) => (byte)(32 | ((int)powerMode << 2) | ((int)addressingMode << 1) | (int)instructionSet);

      public static byte SetDisplayConfiguration(DisplayMode displayMode) => (byte)(8 | (((int)displayMode & 2) << 1) | ((int)displayMode & 1));
      public static byte SetYAddress(int address) => (byte)(64 | (address & 7));
      public static byte SetXAddress(int address) => (byte)(128 | (address & 127));

      public static byte SetTemperatureCoefficient(int coefficient) => (byte)(4 | (coefficient & 3));
      public static byte SetBiasSystem(int biasSystem) => (byte)(16 | (biasSystem & 7));
      public static byte SetOperationVoltage(int voltage) => (byte)(128 | (voltage & 127));
    }

    public Pcd8544(ISpiChannel spiChannel, IGpioPin resetPin, IGpioPin dcPin, GpioPin backlightPin) {
      _spi = spiChannel;
      _res = resetPin;
      _dc = dcPin;
      _bl = backlightPin;
    }

    public void Init() {
      _res.PinMode = GpioPinDriveMode.Output;
      _dc.PinMode = GpioPinDriveMode.Output;

      _res.Write(GpioPinValue.Low);
      _res.Write(GpioPinValue.High);

      _bl.PinMode = GpioPinDriveMode.PwmOutput;
      _bl.PwmMode = PwmMode.Balanced;
      _bl.PwmClockDivisor = 2;

      Write(PayloadType.Command, Instruction.SetOperationMode(instructionSet: InstructionSet.Extended));
      Write(PayloadType.Command, Instruction.SetTemperatureCoefficient(0));
      Write(PayloadType.Command, Instruction.SetBiasSystem(4));
      Write(PayloadType.Command, Instruction.SetOperationMode(instructionSet: InstructionSet.Basic));
      Write(PayloadType.Command, Instruction.SetDisplayConfiguration(DisplayMode.Normal));

      Brightness = 1.0;
      Contrast = 1.0;
    }

    public void Write(PayloadType payloadType, byte value)  {
      _dc.Write(payloadType == PayloadType.Data);
      _spi.Write(new byte[] {value});
    }

    public void Clear() {
      for (int index = 0; index < ScreenWidth * ScreenHeight / 8; index++) {
        Write(PayloadType.Data, 0x00);
      }
    }

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

        Write(PayloadType.Command, Instruction.SetOperationMode(instructionSet: InstructionSet.Extended));
        Write(PayloadType.Command, Instruction.SetOperationVoltage((int)(60 * _contrast)));
        Write(PayloadType.Command, Instruction.SetOperationMode(instructionSet: InstructionSet.Basic));
      }
    }
  }
}
