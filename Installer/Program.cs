using System;
using System.IO;

namespace Installer {
    class Program {
        // TODO: I still have to search in predictive file locations before scanning all drives.
        // TODO: Fix scanner console output.
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