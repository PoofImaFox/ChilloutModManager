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

            var fileName = $"{FILE_NAME}.{FILE_EXT}";
            testAssembly.Write(fileName);
            Assert.IsTrue(File.Exists(fileName));
            Assert.IsTrue(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));

            testAssembly.Dispose();
            Patcher.PatchAssembly(fileName);

            testAssembly = AssemblyDefinition.ReadAssembly(fileName);
            Assert.IsFalse(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            File.Delete(fileName);
            Assert.IsFalse(File.Exists(fileName));
        }

        [TestMethod]
        public void TestUnpatchAssembly() {
            var testAssembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(
                    FILE_NAME,
                    new Version(1, 0, 0, 0)),
                    FILE_NAME,
                    ModuleKind.Dll);

            var fileName = $"{FILE_NAME}.{FILE_EXT}";
            testAssembly.Write(fileName);
            testAssembly.Dispose();

            Assert.IsTrue(File.Exists(fileName));
            Patcher.PatchAssembly(fileName);

            testAssembly = AssemblyDefinition.ReadAssembly(fileName);
            Assert.IsFalse(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            Patcher.UnPatchAssembly(fileName);
            testAssembly = AssemblyDefinition.ReadAssembly(fileName);
            Assert.IsTrue(testAssembly.MainModule.Types.All(i => i.Name != nameof(ChilloutPatch)));
            testAssembly.Dispose();

            File.Delete(fileName);
            Assert.IsFalse(File.Exists(fileName));
        }
    }
}