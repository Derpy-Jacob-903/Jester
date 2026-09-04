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
public class Metronome() : CustomCardModel(1, CardType.Skill,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var allCards = new List<CardModel>();

        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, CardFactory
            .FilterForCombat(ModelDb.AllCards).Where<CardModel>((Func<CardModel, bool>) (c => c.Type is CardType.Attack or CardType.Skill or CardType.Power && !c.Keywords.Contains(CardKeyword.Unplayable) && !IsBlacklisted(c))), 1, this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            CardCmd.ApplyKeyword(card.cardAdded, [CardKeyword.Exhaust]);
            if (this.IsUpgraded)
            {
                CardCmd.Upgrade(card.cardAdded);
            }
            await CardCmd.AutoPlay(choiceContext, card.cardAdded, play.Target);
            if (card.cardAdded.Type != CardType.Power)
            {
                await CardCmd.Exhaust(choiceContext, card.cardAdded);
            }
        }
    }

    public static bool IsBlacklisted(CardModel card)
    {
        if (card.EnergyCost.CostsX && SplitTheAtom.FakeResolveEnergyXValue(card) > 0) return true;
        if (card.DynamicVars.HpLoss.BaseValue > card.Owner.Creature.CurrentHp) return true;
        if (card.DynamicVars.ExtraDamage.BaseValue > 0 && card.DynamicVars.CalculatedDamage.Calculate(null) == 0) return true;
        if (card.DynamicVars.CalculationExtra.BaseValue > 0 && card.DynamicVars.CalculatedBlock.Calculate(null) == 0) return true;
        return false;
    }
}