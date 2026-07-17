using BaseLib.Abstracts;
using BaseLib.Utils;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Jester.JesterCode.Cards;

[Pool(typeof(JesterCardPool))]
public class RngCannon() : CustomCardModel(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var allCards = new List<CardModel>();
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, CardFactory
            .FilterForCombat(ModelDb.AllCards).Where<CardModel>((Func<CardModel, bool>) (c => c.Type is CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))), 2, this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            CardCmd.ApplyKeyword(card.cardAdded, [CardKeyword.Exhaust]);
            if (this.IsUpgraded)
            {
                CardCmd.Upgrade(card.cardAdded);
            }
            if (card.cardAdded.Type != CardType.Power)
            {
                card.cardAdded.ExhaustOnNextPlay = true;
            }
            await CardCmd.AutoPlay(choiceContext, card.cardAdded, play.Target);
            if (card.cardAdded.Keywords.Contains(CardKeyword.Unplayable))
            {
                await CardCmd.Exhaust(choiceContext, card.cardAdded);
            }
        }
    }
}