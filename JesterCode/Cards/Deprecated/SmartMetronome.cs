using BaseLib.Utils;
using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace Jester.JesterCode.Cards;

[Pool(typeof(DeprecatedCardPool))]
public class SmartMetronome() : JesterCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Replay", 2M)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, CardFactory
            .FilterForCombat(ModelDb.AllCards).Where<CardModel>((c => c.Type is CardType.Attack or CardType.Skill or CardType.Power && !c.Keywords.Contains(CardKeyword.Unplayable))), 1, this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            var banana = card.cardAdded;
            //CardCmd.ApplyKeyword(card.cardAdded, [CardKeyword.Exhaust]);
            if (card.cardAdded.EnergyCost.Canonical == 0)
            {
                var cardPileAddResult = card;
                banana.BaseReplayCount = banana.BaseReplayCount + this.DynamicVars["Replay"].IntValue;
            }
            if (this.IsUpgraded)
            {
                CardCmd.Upgrade(banana);
            }
            if (banana.Type != CardType.Power)
            {
                banana.ExhaustOnNextPlay = true;
            }
            await CardCmd.AutoPlay(choiceContext, banana, play.Target);
            /*if (banana.Type != CardType.Power)
            {
                await CardCmd.Exhaust(choiceContext, banana);
            }*/
        }
    }


    protected override void OnUpgrade()
    {
        
    }
}