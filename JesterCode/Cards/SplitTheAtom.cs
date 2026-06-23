using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class SplitTheAtom() : JesterCard(6,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AllEnemies)//, ISplitTheAtom
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(50m, ValueProp.Move),
        new DynamicVar("Poison", 10)
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState?.RunState?.CurrentRoom is not CombatRoom combatRoom) return;
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
            .Execute(context);
        await PowerCmd.Apply<PoisonPower>(context, CombatState.HittableEnemies, DynamicVars["Poison"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner.PlayerCombatState != null && side == CombatSide.Player && participants.Contains(Owner.Creature))
        {
            if (LocalContext.IsMe(Owner))
                await Cmd.CustomScaledWait(0.05f, 0.1f);
            EnergyCost.AddUntilPlayed(-1 * Math.Min(Owner.PlayerCombatState.Energy, FakeResolveEnergyXValue(this)));
            await PlayerCmd.LoseEnergy(Math.Min(Owner.PlayerCombatState.Energy, FakeResolveEnergyXValue(this)), Owner);
            if (LocalContext.IsMe(Owner))
                await Cmd.CustomScaledWait(0.1f, 0.2f);
        }
    }

    //public override bool HasTurnEndInHandEffect => true;
    
    private static int FakeResolveEnergyXValue(CardModel __instance)
    {
        var combatState = __instance.CombatState;
        return combatState != null ? Hook.ModifyXValue(combatState, __instance, __instance.Owner.PlayerCombatState?.Energy ?? 0) : 0;
    }
    
    /*[HarmonyPrefix]
    public static bool OnTurnEndInHandWrapperPatch(CardModel __instance, PlayerChoiceContext choiceContext)
    {
        if (__instance is not ISplitTheAtom) return true;
        _ = OnTurnEndInHandWrapperPatchTask(__instance, choiceContext);
        return false;
    }

    private static async Task OnTurnEndInHandWrapperPatchTask(CardModel card, PlayerChoiceContext choiceContext)
    {
        await CardPileCmd.Add(card, PileType.Play);
        if (LocalContext.IsMe(card.Owner))
            await Cmd.CustomScaledWait(0.3f, 0.6f);
        if (card.Owner.PlayerCombatState != null)
        {
            card.EnergyCost.AddUntilPlayed(-FakeResolveEnergyXValue(card));
            card.Owner.PlayerCombatState.Energy = 0;
        }
        if (card.Keywords.Contains(CardKeyword.Ethereal))
        {
            await CardCmd.Exhaust(choiceContext, card, true);
        }
        else
        {
            await CardPileCmd.Add(card, PileType.Hand.GetPile(card.Owner));
        }
    }*/
}

//interface ISplitTheAtom {}

