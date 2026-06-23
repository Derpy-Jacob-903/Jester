using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace Jester.JesterCode.Cards;

public class HatTrick() : JesterCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
            new StarsVar(1),
            new SummonVar(3),
            new DynamicVar("Orbs", 0) //+1
        ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainStars(1, Owner);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        for (int i = 0; i < DynamicVars["Orbs"].BaseValue; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Orbs"].UpgradeValueBy(1M);
    }
}