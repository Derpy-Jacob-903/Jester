using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Jester.JesterCode.Powers;

public class MadnessPower : JesterPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    [HarmonyPatch(typeof(CombatState), nameof(CombatState.GetOpponentsOf))]
    public static class GetOpponentsOf_Patch
    {
        static void Postfix(
            CombatState __instance,
            Creature creature,
            ref IReadOnlyList<Creature> __result)
        {
            if (creature.HasPower<MadnessPower>())
            {
                __result = (IReadOnlyList<Creature>)__instance.GetCreaturesOnSide(creature.Side)
                    .Union(__instance.GetCreaturesOnSide(GetOppositeSide(creature.Side)));
            }
            static CombatSide GetOppositeSide(CombatSide side)
            {
                switch (side)
                {
                    case CombatSide.None:
                        return CombatSide.None;
                    case CombatSide.Player:
                        return CombatSide.Enemy;
                    case CombatSide.Enemy:
                        return CombatSide.Player;
                    default:
                        throw new ArgumentOutOfRangeException(nameof (side), (object) side, (string) null);
                }
            }
        }
    }
}