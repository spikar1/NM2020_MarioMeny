using System;
using UnityEngine;
[CreateAssetMenu(fileName = "PrintAbility", menuName = "MarioMenu/New Print Ability")]
class PrintAbility : AbilityAsset
{
    public override void ButtonDown(Player player) {
        Debug.Log(name + " down");
    }

    public override void ButtonHeld(Player player) {
        Debug.Log(name + " held");
    }

    public override void ButtonUp(Player player) {
        Debug.Log(name + " up");
    }
}