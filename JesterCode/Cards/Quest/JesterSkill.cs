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

public class JesterSkill() : JesterCard(0, CardType.Quest,
    CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, Owner);
    }
    public override async Task BeforeRoomEntered(AbstractRoom room)
    {
        if (room.RoomType is not (RoomType.Monster or RoomType.Elite or RoomType.Boss))
            return;

        var pool = new List<CardModel>();

        foreach (var character in Owner.UnlockState.Characters)
        {
            var unlocked = character.CardPool
                .GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint);

            pool.AddRange(
                unlocked.Where(c => c.Rarity == CardRarity.Basic && c.Type == CardType.Skill && c is not JesterSkill && !blacklist.Contains(c.Id.Entry)) //oh fuck it doesn't use a suffix :sob: !c.Tags.Contains(CardTag.Defend)
            );
        }

        List<CardModel> list2 = [];
        for (int i = 0; i < 3; i++)
        {
            list2.Add(Owner.PlayerRng.Transformations.NextItem(pool));
        }
        //var list2 = CardFactory.GetDistinctForCombat(Owner, pool, 3, Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        if (IsUpgraded)
        {
            foreach (CardModel card2 in list2)
                CardCmd.Upgrade(card2);
        }
        //CardModel card = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), (IReadOnlyList<CardModel>) list2, Owner, false);
        var cardScope = this.CardScope;
        if (cardScope != null)
        {
            var card = cardScope.CreateCard(Owner.PlayerRng.Transformations.NextItem<CardModel>((IEnumerable<CardModel>) pool), this.Owner);
            //if (pool.Count == 0)
            //return Task.CompletedTask;
            //var card = this.CardScope.CreateCard(
            //Owner.PlayerRng.Transformations.NextItem<CardModel>((IEnumerable<CardModel>)pool), this.Owner);
            if (card is null) return;
            await CardCmd.Transform(this, card);
        }
    }

    private static List<string> blacklist =>
    [
        "GUARDIAN-SECOND_SLAM", //token of Twin Slam
        "METEOR_FRAGMENT", //relies on creating Debris explicitly
        "STS2_STARTING_DECK_SELECT-METEOR_FRAGMENT",
        "TEMPERED_EDGE", //relies on having a Sovereign Blade 
        "STS2_STARTING_DECK_SELECT-TEMPERED_EDGE", 
        "STS2_STARTING_DECK_SELECT-TEMPERED_EDGE", //relies on having a Sovereign Blade 
        "MOONSCREEDPORT-HARMONY", //relies on having orbs
        "MOONSCREEDPORT-LIGHT_BARRIER" //relies on having orbs
    ];
}