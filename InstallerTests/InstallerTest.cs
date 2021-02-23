using System;
using System.IO;
using System.Threading.Tasks;

using Installer;

using Xunit;

namespace InstallerTests {
    public class InstallerTests {
        [Fact]
        public async Task TestDirectorySearch() {
            var gameTitle = "ChilloutVR.exe";
            var dirSep = Path.DirectorySeparatorChar;
            var rootDir = Directory.GetDirectoryRoot(Environment.CurrentDirectory);

            var dirLocation = $"{rootDir}{dirSep}SomeFile{dirSep}steamapps{dirSep}common";
            var gameLocation = $"{dirLocation}{dirSep}{gameTitle}";

            // When building on CI/CD artifact server, this test will still be ran.
            Directory.CreateDirectory(dirLocation);
            File.WriteAllText(gameLocation, default);

            var steamGame = await GameScanner.FindSteamGame(gameTitle);
            Assert.Equal(gameTitle, Path.GetFileName(steamGame));

            File.Delete(gameLocation);
            Directory.Delete(dirLocation);
        }
    }
}
