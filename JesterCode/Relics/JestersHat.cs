using BaseLib.Utils;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Character;
using Jester.JesterCode.Extensions;
using Jester.JesterCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Jester.JesterCode.Relics;

[Pool(typeof(JesterRelicPool))]
public class JestersHat() : JesterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars
        =>
        [
            new SummonVar(5M),
            new StarsVar(3),
            new DynamicVar("Rerolls", 1M)
        ];
    /*public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        var miracle = combatState.CreateCard<Ponder>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(miracle, PileType.Hand, player);
        Flash();
    }*/
    public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom room)
    {
        if (player != this.Owner)
            return false;
        foreach (CardReward cardReward in rewards.OfType<CardReward>())
            cardReward.CanReroll = true;
        return true;
    }
    public override string PackedIconPath => $"relic.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"relic_outline.png".RelicImagePath();
    protected override string BigIconPath => $"relic.png".BigRelicImagePath();
    
    /*public override async Task BeforeCombatStart()
    {
        JestersHat source = this;
        foreach (var i in Owner.Deck.Cards.OfType<Unleash>())
        {
            await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), source.Owner, source.DynamicVars.Summon.BaseValue, source);
        }
        foreach (var j in Owner.Deck.Cards.OfType<FallingStar>())
        {
            await PlayerCmd.GainStars(source.DynamicVars.Stars.BaseValue, Owner);
        }
    }*/
    
    /*public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        var modifyMerchantCardPool = options as CardModel[] ?? options.ToArray();
        if (player != this.Owner || modifyMerchantCardPool.All<CardModel>((Func<CardModel, bool>) (p => p.Pool.IsColorless)))
            return modifyMerchantCardPool;
        CardModel[] array = modifyMerchantCardPool.ToArray();
        foreach (var pp in player.UnlockState.CharacterCardPools)
        {
            modifyMerchantCardPool = modifyMerchantCardPool.Union(pp.AllCards.AsEnumerable()).ToArray();
        }
        return modifyMerchantCardPool;
    }
    
    public override CardCreationOptions ModifyCardRewardCreationOptions(
        Player player,
        CardCreationOptions options) //Prismaic Gem
    {
        if (player != this.Owner || options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications) || options.CustomCardPool != null || options.CardPools.All<CardPoolModel>((Func<CardPoolModel, bool>) (p => p.IsColorless)))
            return options;
        IEnumerable<CardPoolModel> pools = player.UnlockState.CharacterCardPools.Union<CardPoolModel>((IEnumerable<CardPoolModel>) options.CardPools);
        return options.WithCardPools(pools, options.CardPoolFilter);
    }*/

    /*public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
    {
        foreach (var model in ModelDb.AllCards)
        {
            var result = new CardCreationResult(model);
            cards.Add(result);
        }
        cards.AddRange(ModelDb.AllCards);
    }*/
}