using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;

namespace Jester.JesterCode.Potions;

public class CatnipPotion : JesterPotion
{
    public override PotionRarity Rarity => PotionRarity.Event;
    public override PotionUsage Usage => PotionUsage.AnyTime;
    public override TargetType TargetType => TargetType.Self;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculatedVar("CalcEnergy"),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1)
    ];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        if (target?.Player == null) target = Owner.Creature;
        if (target.Player != null)
            await PlayerCmd.GainEnergy(((CalculatedVar)DynamicVars["CalcEnergy"]).Calculate(target), target.Player);
    }
}
