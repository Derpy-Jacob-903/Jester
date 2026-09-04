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

[Pool(typeof(TokenCardPool))]
public class Ponder() : JesterCard(0, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<DefendJester>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Ponder abundance = this;
        List<CardModel> list = ModelDb.CardPool<JesterCardPool>().AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedInCombat && c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Event)).Distinct<CardModel>();(abundance.Owner, ModelDb.AllCards.Where((Func<CardModel, bool>) (c => )), 3, abundance.Owner.RunState.Rng.CombatCardGeneration).ToList();
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