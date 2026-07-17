using BaseLib.Utils;
using Jester.JesterCode.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Relics;

public class BGPicse() : JesterRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props,
        Creature dealer, CardModel cardSource)
    {
        amount = Math.Round(amount * 6, MidpointRounding.ToPositiveInfinity) / 6.0m;
        return base.BeforeDamageReceived(choiceContext, target, amount, props, dealer, cardSource);
    }

    public override Task BeforeBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource)
    {
        amount = Math.Round(amount * 6, MidpointRounding.ToPositiveInfinity) / 6.0m;
        return base.BeforeBlockGained(creature, amount, props, cardSource);
    }
}