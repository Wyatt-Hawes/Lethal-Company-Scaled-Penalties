using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ScaledPenalty.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        //[HarmonyPatch(nameof(HUDManager.ApplyPenalty))]
        [HarmonyPatch(typeof(HUDManager), "ApplyPenalty")]
        [HarmonyPrefix]
        public static bool ApplyPenalty(ref EndOfGameStatUIElements ___statsUIElements, ref StartOfRound ___playersManager, int playersDead, int bodiesInsured)
        {
            //Create penalty based on the players in the lobby
            ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource("Migis.ScaledPenaltyMod");
            mls.LogInfo("Inside Game!");
            mls.LogInfo("Connected Players: " + ___playersManager.connectedPlayersAmount);
            mls.LogInfo("Connected Players+1: " + (___playersManager.connectedPlayersAmount + 1));

            float penaltyPercentage = (float)(1 / (float)(___playersManager.connectedPlayersAmount + 1.0f));

            mls.LogInfo("Calculated Penalty Per Player: "+ penaltyPercentage);

            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            int groupCredits = terminal.groupCredits;
            bodiesInsured = Mathf.Max(bodiesInsured, 0);

            //Penalty applied when you dont recover a body
            for (int i = 0; i < playersDead - bodiesInsured; i++)
            {
                terminal.groupCredits -= (int)((float)groupCredits * penaltyPercentage);
            }
            terminal.groupCredits = Mathf.Max(terminal.groupCredits, 0);

            ___statsUIElements.penaltyAddition.text = $"{playersDead} casualties: -{penaltyPercentage * 100f * (float)(playersDead - bodiesInsured)}%\n({bodiesInsured} friends recovered)";
            ___statsUIElements.penaltyTotal.text = $"DUE: ${groupCredits - terminal.groupCredits}";
            Debug.Log($"New group credits after penalty: {terminal.groupCredits}");
            return false;
        }
    }
}
