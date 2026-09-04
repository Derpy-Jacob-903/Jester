using BaseLib.Utils;
using Jester.JesterCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace Jester.JesterCode.Potions;

[Pool(typeof(SharedPotionPool))]
public class AbilitySyringe : JesterPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.AnyTime;
    public override TargetType TargetType => TargetType.Self;
    
    private IEnumerable<IHoverTip> _extraHoverTips = (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
    
    private const string _ancientCardKey = "AncientCard";
    private ModelId _ancientCard = ModelDb.GetId<DeprecatedCard>();
    [SavedProperty]
    public ModelId AncientCard
    {
        get => _ancientCard;
        set
        {
            AssertMutable();
            _ancientCard = value;
            if (!(_ancientCard != null && _ancientCard != ModelDb.GetId<DeprecatedCard>()))
                return;
            CardModel card = SaveUtil.CardOrDeprecated(_ancientCard);
            _extraHoverTips = card.HoverTips.Concat([HoverTipFactory.FromCard(card, true)]);
            ((StringVar) this.DynamicVars[nameof (AncientCard)]).StringValue = card.Title;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("AncientCard")];
    public override IEnumerable<IHoverTip> ExtraHoverTips => this._extraHoverTips;
    
    

    public override Task BeforeRoomEntered(AbstractRoom room)
    {
        SetupForPlayer(Owner);
        return base.BeforeRoomEntered(room);
    }

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        var dustyTome = this;
        CardModel card = dustyTome.Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(dustyTome.AncientCard), dustyTome.Owner);
        SetupForPlayer(Owner);
        CardCmd.Upgrade(card);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
    }

    public void SetupForPlayer(Player player)
    {
        if (!(_ancientCard != null && _ancientCard != ModelDb.GetId<DeprecatedCard>()))
            return;
        var items = ModelDb.CardPool<JesterCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
        AncientCard = player.PlayerRng.Rewards.NextItem(items).Id;
    }
}
