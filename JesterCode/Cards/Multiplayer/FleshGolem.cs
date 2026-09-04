using BaseLib.Utils;
using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Jester.JesterCode.Cards;

[Pool(typeof(DeprecatedCardPool))]
public class FleshGolem() : JesterCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.AllAllies)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<StrikeJester>()
    ];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
            foreach (var p1 in CombatState.Players)
            {
                foreach (var p2 in CombatState.Players)
                {
                    
                }
            }
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}