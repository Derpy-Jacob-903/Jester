using BaseLib.Utils;
using Jester.JesterCode.Cards;
using Jester.JesterCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class LightenTheLoad() : JesterCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HpLossVar(25),
        new PowerVar<ExtraTurnPower>(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, Owner.Creature,
            new DamageVar(DynamicVars.HpLoss.BaseValue, ValueProp.Unpowered | ValueProp.Move
            ),(CardModel) this, play);
        await CommonActions.ApplySelf<ExtraTurnPower>(choiceContext, this);
    }
}
