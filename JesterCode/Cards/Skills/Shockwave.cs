using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class ShockwaveJester() : JesterCard(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Power", 3M)];
    
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_flying_slash");
        var amount = DynamicVars["Power"].IntValue;
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, amount, Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, amount, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}