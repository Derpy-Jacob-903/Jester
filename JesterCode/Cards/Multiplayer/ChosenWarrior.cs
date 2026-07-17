using Jester.JesterCode.Cards;
using Jester.JesterCode.Character;
using Jester.JesterCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Jester.JesterCode.Cards;

public class ChosenWarrior() : JesterCard(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ..MakeCalculatedVar("isAlpha", 1, (c, creature) => c.CombatState != null && c.CombatState.Allies.Any(p => p.HasPower<AlphaPower>()) ? 2 : 1),
        new PowerVar<StrengthPower>(1),
        new PowerVar<FocusPower>(0)
    ];
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<ClericCardPool>();
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}