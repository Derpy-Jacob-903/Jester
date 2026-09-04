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

namespace Jester.JesterCode.Cards;

public class MindMeld() : JesterCard(2, CardType.Skill,
    CardRarity.Rare, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ExtraTurnPower>(1)
    ];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null) await CommonActions.Apply<ExtraTurnPower>(choiceContext, play.Target, this);
    }
}
