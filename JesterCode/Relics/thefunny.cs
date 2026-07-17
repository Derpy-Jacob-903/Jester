using BaseLib.Utils;
using Jester.JesterCode.Character;
using Jester.JesterCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Jester.JesterCode.Relics;

[Pool(typeof(JesterRelicPool))]
public class TheFunny() : JesterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Event;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(999)
    ];

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        foreach (var c in Owner.PlayerCombatState.DrawPile.Cards)
        {
            await CardCmd.Exhaust(choiceContext, c);
        }
        await PowerCmd.Apply<DoomPower>(choiceContext, Owner.Creature, 999, Owner.Creature, null);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.IsAutoPlay) return;
        await CardPileCmd.Draw(choiceContext, cardPlay.Resources.EnergySpent, cardPlay.Card.Owner);
    }
    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        return player != this.Owner ? amount : amount + (Decimal) this.DynamicVars.Energy.IntValue;
    }
}

