using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ChillModsHandler;

using UnityEngine;

namespace ChillPatcher {
    public class ChilloutPatch : MonoBehaviour {
        public void Start() {
            var modManager = Assembly.LoadFrom(typeof(ModLoader).Namespace);
            var modLoader = (ModLoader)modManager.CreateInstance(typeof(ModLoader).Name);
            modLoader.LoadMods();
        }
    }
}