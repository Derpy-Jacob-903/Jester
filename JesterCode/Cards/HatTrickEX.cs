using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class HatTrickEx() : JesterCard(1, CardType.Skill,
    CardRarity.Ancient, TargetType.Self)
{
    public override int MaxUpgradeLevel => 10;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new StarsVar(3), //+1
        new SummonVar(9), //+6
        new DynamicVar("Orbs", 2) //+1
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        for (int i = 0; i < DynamicVars["Orbs"].BaseValue; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Stars.UpgradeValueBy(1M);
        DynamicVars.Summon.UpgradeValueBy(4M);
        DynamicVars["Orbs"].UpgradeValueBy(1M);
    }
}

