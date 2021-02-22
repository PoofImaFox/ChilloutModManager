using ChillPatch;

using ChillPatcher;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mono.Cecil;

using System;
using System.IO;
using System.Linq;

namespace ChillPatcherTests {
    [TestClass]
    public class PatcherTests {

        private const string FILE_NAME = "PatchTest";
        private const string FILE_EXT = "dll";

        public PatcherTests() {
            if (File.Exists($"{FILE_NAME}.{FILE_EXT}")) {
                File.Delete($"{FILE_NAME}.{FILE_EXT}");
            }
        }

        [TestMethod]
        public void TestPatchAssembly() {
            var testAssembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(
                    FILE_NAME,
                    new Version(1, 0, 0, 0)),
                    FILE_NAME,
                    ModuleKind.Dll);

            testAssembly.Write($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsTrue(File.Exists($"{FILE_NAME}.{FILE_EXT}"));
            Assert.IsTrue(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));

            testAssembly.Dispose();
            Patcher.PatchAssembly($"{FILE_NAME}.{FILE_EXT}");

            testAssembly = AssemblyDefinition.ReadAssembly($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsFalse(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            File.Delete($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsFalse(File.Exists($"{FILE_NAME}.{FILE_EXT}"));
        }

        [TestMethod]
        public void TestUnpatchAssembly() {
            var FILE_NAME = "PatchTest";
            var FILE_EXT = "dll";

            var testAssembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(
                    FILE_NAME,
                    new Version(1, 0, 0, 0)),
                    FILE_NAME,
                    ModuleKind.Dll);

            testAssembly.Write($"{FILE_NAME}.{FILE_EXT}");
            testAssembly.Dispose();

            Assert.IsTrue(File.Exists($"{FILE_NAME}.{FILE_EXT}"));
            Patcher.PatchAssembly($"{FILE_NAME}.{FILE_EXT}");

            testAssembly = AssemblyDefinition.ReadAssembly($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsFalse(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            Patcher.UnPatchAssembly($"{FILE_NAME}.{FILE_EXT}");
            testAssembly = AssemblyDefinition.ReadAssembly($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsTrue(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            File.Delete($"{FILE_NAME}.{FILE_EXT}");
            Assert.IsFalse(File.Exists($"{FILE_NAME}.{FILE_EXT}"));
        }
    }
}