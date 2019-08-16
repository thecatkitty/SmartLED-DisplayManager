using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Celones.Device {
  public class Pcd8544 {
    private ISpiChannel _spi;
    private IGpioPin _res, _dc;
    private GpioPin _bl;
    private double _brightness;

    public int ScreenWidth { get => 84; }
    public int ScreenHeight { get => 48; }

    public enum PayloadType {
      Command = 0,
      Data = 1
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
      Brightness = 1.0;

      Write(PayloadType.Command, 0x21);
      Write(PayloadType.Command, 0xB1);
      Write(PayloadType.Command, 0x04);
      Write(PayloadType.Command, 0x14);
      Write(PayloadType.Command, 0x20);
      Write(PayloadType.Command, 0x0C);
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
          _bl.PwmRegister = (int)(_brightness * _bl.PwmRange);
          _brightness = value;
        }
    }
  }
}
