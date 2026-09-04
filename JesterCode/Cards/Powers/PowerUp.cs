using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Jester.JesterCode.Cards;

public class PowerUp() : JesterCard(1, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power && !c.Keywords.Contains(CardKeyword.Unplayable) && c.Rarity is not (CardRarity.Common or CardRarity.Basic))), this.ResolveEnergyXValue(), this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            await CardCmd.AutoPlay(choiceContext, card.cardAdded, this.Owner.Creature);
        }
    }

    protected override void OnUpgrade()
    {
        
    }
}