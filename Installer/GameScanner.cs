using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer {
    public static class GameScanner {
        public static Task<string?> FindSteamGame(string gameTitle) {
            var fileSystemDrives = Directory.GetLogicalDrives();

            var runningTasks = new List<Task<(bool success, string fileName)>>();
            foreach (var drive in fileSystemDrives) {
                runningTasks.Add(ScanDrive(drive, gameTitle));
            }

            do {
                if (runningTasks.All(i => i.IsCompleted)) {
                    break;
                }

                Task.WaitAny(runningTasks.ToArray());
            }
            while (!runningTasks.Any(i => i.Result.success));

            foreach (var runningTask in runningTasks.Where(i => i.Status != TaskStatus.RanToCompletion)) {
                runningTask.Dispose();
            }

            if (runningTasks.All(i => !i.Result.success)) {
                return Task.FromResult<string?>(null);
            }

            var successTask = runningTasks.First(i => i.Result.success);
            return Task.FromResult<string?>(successTask.Result.fileName);
        }

        private static async Task<(bool success, string fileName)> ScanDrive(string fileSystemRoot, string gameTitle) {
            var scanResult = await Task.Run(() => ScanDirectory(fileSystemRoot, $"steamapps{Path.DirectorySeparatorChar}common"));

            if (!scanResult.found) {
                return (false, string.Empty);
            }

            var gamesInLibrary = Directory.GetDirectories(scanResult.location);
            foreach (var gameInstall in gamesInLibrary) {
                var gameFiles = Directory.GetFiles(gameInstall);
                if (gameFiles.Any(i => Path.GetFileName(i) == gameTitle)) {
                    return (true, $"{gameInstall}{Path.DirectorySeparatorChar}{gameTitle}");
                }
            }

            return (false, string.Empty);
        }

        private static (bool found, string location) ScanDirectory(string directory, string folderDirSuffix) {
            if (directory.Count(i => i == Path.DirectorySeparatorChar) > 7
                || !HasWriteAccessToFolder(directory)) {
                return (false, string.Empty);
            }

            var directories = Directory.GetDirectories(directory);
            if (!directories.Any(i => i.EndsWith(folderDirSuffix))) {
                foreach (var scanDirectory in directories) {
                    Task.Run(() => UpdateStatus(scanDirectory));

                    var recersiveResult = ScanDirectory(scanDirectory, folderDirSuffix);
                    if (recersiveResult.found) {
                        return recersiveResult;
                    }
                }
            }
            else {
                return (true, directories.First(i => i.EndsWith(folderDirSuffix)));
            }

            return (false, string.Empty);
        }

        private static Task UpdateStatus(string scanDirectory) {
            if (NativeMethods.GetConsoleWindow() == IntPtr.Zero) {
                Trace.WriteLine($"Scanning: {scanDirectory}...");
                return Task.CompletedTask;
            }

            Console.WriteLine($"Scanning: {new DirectoryInfo(scanDirectory).Name}...");
            return Task.CompletedTask;
        }

        private static bool HasWriteAccessToFolder(string folderPath) {
            try {
                var access = Directory.GetDirectories(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException) {
                return false;
            }
            catch (IOException) {
                return false;
            }
        }
    }
}