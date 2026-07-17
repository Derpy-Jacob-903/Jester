using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using ICardSelector = MegaCrit.Sts2.Core.TestSupport.ICardSelector;

namespace Jester.JesterCode.Powers;

public class AutopilotPower : JesterPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterAutoPrePlayPhaseEnteredLate(
      PlayerChoiceContext choiceContext,
      Player player)
    {
      var whisperingEarring = this;
      ICombatState combatState;
      if (player != Owner.Player)
      {
        combatState = (ICombatState)null;
      }
      else
      {
        combatState = player.Creature.CombatState;
        whisperingEarring.Flash();
        bool flag;
        using (CardSelectCmd.PushSelector((ICardSelector)new VakuuCardSelector()))
        {
          int cardsPlayed = 0;
          int startTurn = whisperingEarring.Owner.Player.PlayerCombatState.TurnNumber;
          while (cardsPlayed < 13 && !CombatManager.Instance.IsOverOrEnding &&
                 !CombatManager.Instance.IsPlayerReadyToEndTurn(player) &&
                 whisperingEarring.Owner.Player.PlayerCombatState.TurnNumber == startTurn)
          {
            CardModel card = PileType.Hand.GetPile(whisperingEarring.Owner.Player).Cards
              .FirstOrDefault<CardModel>((Func<CardModel, bool>)(c => c.CanPlay()));
            if (card != null)
            {
              Creature target = this.GetTarget(card, combatState);
              (int, int) valueTuple = await card.SpendResources();
              await CardCmd.AutoPlay(choiceContext, card, target, skipXCapture: true);
              ++cardsPlayed;
              card = (CardModel)null;
              target = (Creature)null;
            }
            else
              break;
          }

          flag = cardsPlayed >= 13;
          if (cardsPlayed == 0)
          {
            combatState = (ICombatState)null;
            return;
          }
        }

        TalkCmd.Play(
          flag
            ? new LocString("relics", "WHISPERING_EARRING.warning")
            : new LocString("relics", "WHISPERING_EARRING.approval"), whisperingEarring.Owner,
          VfxColor.Purple);
        await PowerCmd.Decrement(this);
        combatState = (ICombatState)null;
      }
    }

    /// <summary>
  /// Gets the target for a card during Vakuu's auto-play.
  /// Enemies: leftmost first. Allies: random.
  /// </summary>
  private Creature GetTarget(CardModel card, ICombatState combatState)
  {
    Rng combatTargets = this.Owner.Player.RunState.Rng.CombatTargets;
    Creature target;
    switch (card.TargetType)
    {
      case TargetType.AnyEnemy:
        target = combatState.HittableEnemies.FirstOrDefault<Creature>();
        break;
      case TargetType.AnyPlayer:
        target = Owner;
        break;
      case TargetType.AnyAlly:
        target = combatTargets.NextItem<Creature>(combatState.Allies.Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c != this.Owner)));
        break;
      default:
        target = (Creature) null;
        break;
    }
    return target;
  }
}