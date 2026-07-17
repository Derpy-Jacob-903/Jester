using BaseLib.Patches.Features;
using Godot;
using Jester.JesterCode.Cards;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Jester.JesterCode.Cards;

public class StrikeCleric() : JesterCard(1, CardType.Attack,
    CardRarity.Basic, CustomTargetType.Anyone)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6m, ValueProp.Move)
    ];
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<ClericCardPool>();
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target == null || CombatState == null) return;
        if (CombatState.Allies.Contains(play.Target))
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        else
        {
          var modifiedAmount = base.DynamicVars.Damage.BaseValue;
          if (RunState != null)
          {
            modifiedAmount = Hook.ModifyDamage(RunState, CombatState, play.Target, Owner.Creature, modifiedAmount, base.DynamicVars.Damage.Props, this, play, ModifyDamageHookType.All, CardPreviewMode.None, out var __);
            await Hook.AfterModifyingDamageAmount(RunState, CombatState, this, __);
          }
          await CreatureCmd.Heal(play.Target, modifiedAmount);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}