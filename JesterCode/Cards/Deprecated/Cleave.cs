using System.Runtime.ExceptionServices;
using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class CleaveJester() : JesterCard(1, CardType.Attack,
    CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Meat>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8m, ValueProp.Move)
    ];
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var damageResults = await CreatureCmd.Damage(context, CombatState?.HittableEnemies, DynamicVars.Damage, Owner.Creature, this, play);
        var enumerable = damageResults as DamageResult[] ?? damageResults.ToArray();
        if (CombatState is null) return;
        for (var i = 0; i < enumerable.Count(n => n.WasTargetKilled); ++i)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Meat>(Owner), PileType.Hand, Owner));
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}