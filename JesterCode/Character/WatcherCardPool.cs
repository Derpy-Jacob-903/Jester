using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using Jester.JesterCode.Character;
using Jester.JesterCode.Extensions;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Unlocks;

namespace Jester.JesterCode.Character;

public class WatcherCardPool : CustomCardPoolModel
{
    public override string Title => "Jester-FakeWatcherPool";

    public override float H => 0.8f; //Hue; changes the color.
    public override float S => 0.5f; //Saturation
    public override float V => 1f; //Brightness

    public override Color DeckEntryCardColor => new(0x552262FF);

    public override bool IsColorless => false;

    public override string BigEnergyIconPath => "res://Jester/images/charui/watcher_energy_icon.png";
    public override string TextEnergyIconPath => "res://Jester/images/charui/watcher_energy_text_icon.png";
}
