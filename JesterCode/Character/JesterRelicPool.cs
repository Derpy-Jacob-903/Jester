using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace Jester.JesterCode.Character;

public class JesterRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Jester.CharacterId;
    public override Color LabOutlineColor => Jester.Color;
}



[HarmonyPatch(typeof(RelicPoolModel))]
public static class JesterRelicPoolPatch
{
    [HarmonyPatch("GetUnlockedRelics")]
    [HarmonyPrefix]
    public static bool Prefix(RelicPoolModel __instance, UnlockState unlockState, ref IEnumerable<RelicModel> __result)
    {
        if (__instance is not JesterRelicPool)
            return true;
        __result = ModelDb.AllCharacterRelicPools.Where(n => n is not JesterRelicPool).SelectMany(c => c.GetUnlockedRelics(unlockState)).ToList();
        return false;
    }
}