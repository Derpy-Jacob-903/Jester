using BaseLib.Abstracts;
using Jester.JesterCode.Cards;
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

public class ConfusionPower : JesterPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (this.Amount == 1)
        {
            var card = CombatState.CreateCard<ConfusionCard>(Owner.Player);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
            await PowerCmd.Apply<StunnedPower>(ctx, Owner, 2, Owner, null);
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }
    
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext ctx, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
        {
            await PowerCmd.Apply<StunnedPower>(ctx, Owner, 1, Owner, null);
            await PowerCmd.Decrement(this);
        }
    }
}