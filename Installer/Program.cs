using System;
using System.IO;

namespace Installer {
    class Program {
        static void Main() {
            Console.WriteLine("Scanning for ChilloutVR...");
            var gameLocation = GameScanner.FindSteamGame("ChilloutVR.exe")
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Console.WriteLine($"Found {Path.GetFileName(gameLocation)}\n{gameLocation}");
            Console.ReadLine();
        }

        private static void FindGameLocation() {

        }
    }
}