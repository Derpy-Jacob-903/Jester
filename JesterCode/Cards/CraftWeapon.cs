using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Jester.JesterCode.Cards;

public class CraftWeapon() : JesterCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var pool = new List<CardModel>();

        foreach (var character in Owner.UnlockState.Characters)
        {
            var unlocked = character.CardPool
                .GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint);

            pool.AddRange(
                unlocked.Where(c => c.Rarity == CardRarity.Basic && (c.Type == CardType.Attack || c.Id.Entry == "BONE_RALLY") && c is not JesterBasic
                                    && !blacklist.Contains(c.Id.Entry)) //oh fuck it doesn't use a suffix :sob:
            );
        }
        var card = CardFactory.GetForCombat(this.Owner, CardFactory
            .FilterForCombat(pool), 1, this.Owner.RunState.Rng.CombatCardGeneration).First();
        var card2 = CombatState.CreateCard<ThrowWeapon>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Hand, Owner);
    }

    private static List<string> blacklist =>
    [
        "GUARDIAN-SECOND_SLAM", //token of Twin Slam
        "METEOR_FRAGMENT", //relies on creating Debris explicitly
        "STS2_STARTING_DECK_SELECT-METEOR_FRAGMENT",
        "TEMPERED_EDGE", //relies on having a Sovereign Blade 
        "STS2_STARTING_DECK_SELECT-TEMPERED_EDGE"
    ];
}