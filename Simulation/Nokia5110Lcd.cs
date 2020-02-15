using System;
using System.Drawing;
using Unosquare.RaspberryIO.Abstractions;
using static Celones.Device.Pcd8544;

namespace Celones.DisplayManager.Simulation
{
    class Nokia5110Lcd : ISpiChannel, IGpioPin
    {
        private GpioPinValue _dc = GpioPinValue.High;
        private AddressingMode _addressingMode = AddressingMode.Horizontal;
        private InstructionSet _instructionSet = InstructionSet.Basic;
        private DisplayMode _displayMode = DisplayMode.Normal;
        private int _xAddress = 0;
        private int _yAddress = 0;
        private int _temperatureCoefficient = 0;
        private int _biasSystem = 0;
        private int _operationalVoltage = 60;

        public Bitmap Image { get; private set; } = new Bitmap(84, 48);

        public BcmPin BcmPin => throw new NotImplementedException();

        public int BcmPinNumber => throw new NotImplementedException();

        public int PhysicalPinNumber => throw new NotImplementedException();

        public GpioHeader Header => throw new NotImplementedException();

        public GpioPinDriveMode PinMode { get => GpioPinDriveMode.Output; set { } }
        public GpioPinResistorPullMode InputPullMode { get => GpioPinResistorPullMode.Off; set { } }
        public bool Value {
            get => _dc == GpioPinValue.High;
            set => _dc = value ? GpioPinValue.High : GpioPinValue.Low;
        }

        public int FileDescriptor => throw new NotImplementedException();

        public int Channel => throw new NotImplementedException();

        public int Frequency => throw new NotImplementedException();

        public bool Read() => _dc == GpioPinValue.High;

        public void RegisterInterruptCallback(EdgeDetection edgeDetection, Action callback)
        {
            throw new NotImplementedException();
        }

        public void RegisterInterruptCallback(EdgeDetection edgeDetection, Action<int, int, uint> callback)
        {
            throw new NotImplementedException();
        }

        public byte[] SendReceive(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public bool WaitForValue(GpioPinValue status, int timeOutMillisecond)
        {
            throw new NotImplementedException();
        }

        public void Write(bool value)
        {
            _dc = value ? GpioPinValue.High : GpioPinValue.Low;
        }

        public void Write(GpioPinValue value)
        {
            _dc = value;
        }

        public void Write(byte[] buffer)
        {
            foreach (byte b in buffer)
            {
                // Control
                if (_dc == GpioPinValue.Low)
                {
                    // Set operation mode
                    if ((b & 32) != 0)
                    {
                        _addressingMode = (AddressingMode)Enum.ToObject(typeof(AddressingMode), (b & 2) >> 1);
                        _instructionSet = (InstructionSet)Enum.ToObject(typeof(InstructionSet), b & 1);
                    }

                    // Basic instruction set
                    else if (_instructionSet == InstructionSet.Basic)
                    {
                        if ((b & 8) != 0)
                        {
                            _displayMode = (DisplayMode)Enum.ToObject(typeof(DisplayMode), b & 3);
                        }
                        else if ((b & 64) != 0)
                        {
                            _yAddress = b & 7;
                        }
                        else if ((b & 128) != 0)
                        {
                            _xAddress = b & 127;
                        }
                    }

                    // Extended instruction set
                    else
                    {
                        if ((b & 4) != 0)
                        {
                            _temperatureCoefficient = b & 3;
                        }
                        else if ((b & 16) != 0)
                        {
                            _biasSystem = b & 7;
                        }
                        else if ((b & 128) != 0)
                        {
                            _operationalVoltage = b & 127;
                        }
                    }
                }

                // Data
                else
                {
                    var bits = b;
                    for (int i = 0; i < 8; i++)
                    {
                        lock (Image)
                        {
                            Image.SetPixel(_xAddress, _yAddress * 8 + i, ((bits & 1) == 1) ? Color.Black : Color.Transparent);
                            bits >>= 1;
                        }
                    }

                    _xAddress++;
                    if (_xAddress == Image.Width)
                    {
                        _xAddress = 0;
                        _yAddress++;
                    }
                    if (_yAddress == Image.Height / 8)
                    {
                        _yAddress = 0;
                    }
                }
            }
        }
    }
}
