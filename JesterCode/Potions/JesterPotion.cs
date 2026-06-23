using BaseLib.Abstracts;
using BaseLib.Utils;
using Jester.JesterCode.Character;

namespace Jester.JesterCode.Potions;

[Pool(typeof(JesterPotionPool))]
public abstract class JesterPotion : CustomPotionModel;