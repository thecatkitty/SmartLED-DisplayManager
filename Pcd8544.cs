using Unosquare.RaspberryIO.Abstractions;

namespace Celones.Device {
  public class Pcd8544 {
    private ISpiChannel _spi;
    private IGpioPin _res, _dc;

    public int DramSizeX { get => 84; }
    public int DramSizeY { get => 6; }

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
      public static Instruction NoOperation() => new Instruction(0);
      public static Instruction SetOperationMode(PowerMode powerMode = PowerMode.Active, AddressingMode addressingMode = AddressingMode.Horizontal, InstructionSet instructionSet = InstructionSet.Basic) => new Instruction((byte)(32 | ((int)powerMode << 2) | ((int)addressingMode << 1) | (int)instructionSet));

      public static Instruction SetDisplayConfiguration(DisplayMode displayMode) => new Instruction((byte)(8 | (((int)displayMode & 2) << 1) | ((int)displayMode & 1)));
      public static Instruction SetYAddress(int address) => new Instruction((byte)(64 | (address & 7)));
      public static Instruction SetXAddress(int address) => new Instruction((byte)(128 | (address & 127)));

      public static Instruction SetTemperatureCoefficient(int coefficient) => new Instruction((byte)(4 | (coefficient & 3)));
      public static Instruction SetBiasSystem(int biasSystem) => new Instruction((byte)(16 | (biasSystem & 7)));
      public static Instruction SetOperationVoltage(int voltage) => new Instruction((byte)(128 | (voltage & 127)));

      public byte Code { get; private set; }

      private Instruction(byte code) {
        Code = code;
      }
    }

    public Pcd8544(ISpiChannel spiChannel, IGpioPin resetPin, IGpioPin dcPin) {
      _spi = spiChannel;
      _res = resetPin;
      _dc = dcPin;
    }

    public void Init() {
      _res.PinMode = GpioPinDriveMode.Output;
      _dc.PinMode = GpioPinDriveMode.Output;

      _res.Write(GpioPinValue.Low);
      _res.Write(GpioPinValue.High);

      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Extended));
      Write(Instruction.SetTemperatureCoefficient(0));
      Write(Instruction.SetBiasSystem(4));
      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Basic));
      Write(Instruction.SetDisplayConfiguration(DisplayMode.Normal));
    }

    public void Write(byte data)  {
      _dc.Write(GpioPinValue.High);
      _spi.Write(new byte[] {data});
    }

    public void Write(Instruction instruction)  {
      _dc.Write(GpioPinValue.Low);
      _spi.Write(new byte[] {instruction.Code});
    }
  }
}
