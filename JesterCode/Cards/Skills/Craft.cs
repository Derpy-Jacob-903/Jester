using BaseLib.Utils;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class Craft() : JesterCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<StrikeIronclad>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new CardsVar(2) ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Craft abundance = this;
        List<CardModel> list = CardFactory.GetDistinctForCombat(abundance.Owner, ModelDb.AllCards.Where((Func<CardModel, bool>) (c => c.Type == CardType.Skill && c.Rarity == CardRarity.Basic && !c.Tags.Contains(CardTag.Defend))), DynamicVars.Cards.IntValue, abundance.Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (abundance.IsUpgraded)
        {
            foreach (CardModel card in list)
                CardCmd.Upgrade(card);
        }
        CardModel card1 = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, abundance.Owner);
        if (card1 == null)
            return;
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Hand, abundance.Owner);
    }
}