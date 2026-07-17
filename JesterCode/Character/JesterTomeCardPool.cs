using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using Jester.JesterCode.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Unlocks;

namespace Jester.JesterCode.Character;

public class JesterTomeCardPool : CustomCardPoolModel
{
    public override string Title => Jester.CharacterId; //This is not a display name.
    public override string EnergyColorName => "necrobinder";
    public override string CardFrameMaterialPath => "necrobinder";//"card_frame_jester";
    //"res://Jester/shaders/jestercardback.gdshader"
    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    //public override float H => 1f; //Hue; changes the color.
    //public override float S => 1f; //Saturation
    //public override float V => 1f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load Jester/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("ff7ad4");

    public override bool IsColorless => false;
}

[HarmonyPatch(typeof(CardPoolModel))]
public static class JesterTomeCardPoolPatch
{
    [HarmonyPatch("GetUnlockedCards")]
    [HarmonyPrefix]
    public static bool Prefix(CardPoolModel __instance, UnlockState unlockState, CardMultiplayerConstraint multiplayerConstraint, ref IEnumerable<CardModel> __result)
    {
        if (__instance is not JesterCardPool)
            return true; // run original
        var allUnlocked = ModelDb.AllCharacterCardPools.Where(n => n is not JesterCardPool && !n.AllCards.Any(m => m is ITomeCard)).SelectMany(c => c.GetUnlockedCards(
                unlockState,
                multiplayerConstraint))
            .ToList();
        
        //List<CardModel> list = this.FilterThroughEpochs(unlockState, this.AllCards).ToList<CardModel>();
        switch (multiplayerConstraint)
        {
            case CardMultiplayerConstraint.MultiplayerOnly:
                allUnlocked.RemoveAll((Predicate<CardModel>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.SingleplayerOnly));
                break;
            case CardMultiplayerConstraint.SingleplayerOnly:
                allUnlocked.RemoveAll((Predicate<CardModel>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly));
                break;
        }

        foreach (var VARIABLE in allUnlocked)
        {
            
        }
        allUnlocked.RemoveAll((Predicate<CardModel>) (c => c.Rarity != CardRarity.Ancient));
        allUnlocked.RemoveAll((Predicate<CardModel>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly));
        __result = __result == null ? allUnlocked : __result.Union(allUnlocked);
        return false; // skip original
        //__result = ModelDb.AllCharacterCardPools.Aggregate(__result, (current, pp) => current.Union(pp.AllCards.AsEnumerable()).ToArray());
        //__result = __result.Union(ModelDb.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Pool != __instance)));
    }
}
