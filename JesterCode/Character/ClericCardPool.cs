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

public class ClericCardPool : CustomCardPoolModel
{
    public override string Title => "Jester-FakeClericPool"; //This is not a display name.
    public override string BigEnergyIconPath => "res://Jester/images/charui/white_energy_icon.png";
    public override string TextEnergyIconPath => "res://Jester/images/charui/white_energy_text_icon.png";
    public override float H => 0.15f; //Hue; changes the color.
    public override float S => 0.4f; //Saturation
    public override float V => 1.2f; //Brightness
    public override Color DeckEntryCardColor => new("ffff93");
    public override bool IsColorless => false;
}
