using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Jester.JesterCode.Cards;

public class FleshGolem() : JesterCard(0, CardType.Skill,
    CardRarity.Uncommon, TargetType.AllAllies)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<StrikeJester>()
    ];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var allCards = play.Target.Player.PlayerCombatState.Hand.Cards;
        
        IReadOnlyList<CardPileAddResult> combat = 
            await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, 
                allCards.Count > 0 ? CardFactory.FilterForCombat(allCards).Where((Func<CardModel, bool>) 
                    (c => c.Type is CardType.Attack or CardType.Skill or CardType.Power && !c.Keywords.Contains(CardKeyword.Unplayable))) : [ModelDb.Card<StrikeJester>()], 1, this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            CardCmd.ApplyKeyword(card.cardAdded, [CardKeyword.Exhaust]);
            await CardCmd.AutoPlay(choiceContext, card.cardAdded, play.Target);
            if (card.cardAdded.Type != CardType.Power)
            {
                await CardCmd.Exhaust(choiceContext, card.cardAdded);
            }
        }
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}