using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChillPatcher {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                WriteHelp();
                return;
            }

            var command = args[0];
            switch (command) {
                case "patch":
                    Patch(args.Skip(1).ToArray());
                    return;

                case "unpatch":
                    UnPatch(args.Skip(1).ToArray());
                    return;

                default:
                    WriteHelp();
                    return;
            }
        }

        public static void Patch(string[] args) {
            if (args.Length < 1) {
                WriteHelp();
            }

            if (!File.Exists(args[0])) {
                Console.WriteLine($"File '{args[0]}' does not exist.");
                return;
            }


        }

        public static void UnPatch(string[] args) {
            if (args.Length < 1) {
                WriteHelp();
            }


        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteHelp() {
            Console.WriteLine(Properties.Resources.CommandHelpfile);
        }
    }
}