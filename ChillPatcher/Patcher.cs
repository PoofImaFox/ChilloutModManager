using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ChillPatch;

using Mono.Cecil;

using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace ChillPatcher {
    public static class Patcher {
        public static void PatchAssembly(string assemblyFILE_NAME) {
            var unityCoreModuleDef = ModuleDefinition.ReadModule("UnityEngine.CoreModule.dll");

            var assembly = AssemblyDefinition.ReadAssembly(assemblyFILE_NAME,
                new ReaderParameters { ReadWrite = true });

            var patchedImportType = new TypeDefinition("", nameof(ChilloutPatch),
                TypeAttributes.Class | TypeAttributes.Public,
                new TypeReference("UnityEngine", "MonoBehaviour", unityCoreModuleDef, unityCoreModuleDef));

            foreach (var methodDef in typeof(ChilloutPatch).GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
                patchedImportType.Methods.Add(new MethodDefinition(
                    methodDef.Name,
                    Mono.Cecil.MethodAttributes.Public,
                    assembly.MainModule.TypeSystem.Void));
            }

            assembly.MainModule.Types.Add(patchedImportType);
            assembly.Write();
            assembly.Dispose();
        }

        public static void UnPatchAssembly(string assemblyFILE_NAME) {
            var assembly = AssemblyDefinition.ReadAssembly(assemblyFILE_NAME,
               new ReaderParameters { ReadWrite = true });

            if (!assembly.MainModule.Types.Any(i => i.Name == nameof(ChilloutPatch))) {
                return;
            }

            var cecilTypeDef = assembly.MainModule.Types.First(i => i.Name == nameof(ChilloutPatch));
            assembly.MainModule.Types.Remove(cecilTypeDef);
            assembly.Write();
            assembly.Dispose();
        }
    }
}