using System;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Celones.DisplayManager {
  class Program {
    static void Main(string[] args) {
      Pi.Init<BootstrapWiringPi>();
      Console.WriteLine("Hello there~ :3");
    }
  }
}
