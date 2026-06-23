using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace Jester.JesterCode.Character;

public class JesterPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Jester.CharacterId;
    public override Color LabOutlineColor => Jester.Color;
}



[HarmonyPatch(typeof(PotionPoolModel))]
public static class JesterPotionPoolPatch
{
    [HarmonyPatch("GetUnlockedPotions")]
    [HarmonyPrefix]
    public static bool Prefix(PotionPoolModel __instance, UnlockState unlockState, ref IEnumerable<PotionModel> __result)
    {
        if (__instance is not JesterPotionPool)
            return true;
        __result = ModelDb.AllCharacterPotionPools.Where(n => n is not JesterPotionPool).SelectMany(c => c.GetUnlockedPotions(unlockState)).ToList();
        return false;
    }
}