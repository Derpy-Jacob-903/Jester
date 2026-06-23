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

public class JesterBasic() : JesterCard(1, CardType.Quest,
    CardRarity.Basic, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room.RoomType != RoomType.Event)
            return Task.CompletedTask;

        // Collect ALL basic cards from ALL unlocked characters
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
        
        if (pool.Count == 0)
            return Task.CompletedTask;

        var card = this.CardScope.CreateCard(Owner.PlayerRng.Transformations.NextItem<CardModel>((IEnumerable<CardModel>) pool), this.Owner);

        return CardCmd.Transform(this, card);
    }

    private static List<string> blacklist =>
    [
        "METEOR_FRAGMENT", //relies on creating Debris explicitly
        "STS2_STARTING_DECK_SELECT-METEOR_FRAGMENT",
        "TEMPERED_EDGE", //relies on having a Sovereign Blade 
        "STS2_STARTING_DECK_SELECT-TEMPERED_EDGE"
    ];
}