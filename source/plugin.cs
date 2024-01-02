using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ScaledPenalty.Patches;

namespace ScaledPenalty
{
    [BepInPlugin(modGUID,modName,modVersion)]
    public class ClassPenaltyBase : BaseUnityPlugin
    {
        private const string modGUID = "Migis.ScaledPenaltyMod";
        private const string modName = "Scaled Penalty Mod";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static ClassPenaltyBase Instance;

        internal ManualLogSource mls;

        //Entry point
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Scaled Penalty Mod Awoken");

            harmony.PatchAll(typeof(ClassPenaltyBase));
            harmony.PatchAll(typeof(HUDManagerPatch));
        }
    }
}
