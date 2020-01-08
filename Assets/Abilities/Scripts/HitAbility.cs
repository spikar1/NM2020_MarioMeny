using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newHitAbility", menuName = "MarioMenu/New Hit Ability")]
public class HitAbility : AbilityAsset
{
    public Animation hitAnim;

    public override void ButtonDown(Player player) {
        player.anim.SetTrigger("SwordAttack");
    }

    public override void ButtonHeld(Player player) {
    }

    public override void ButtonUp(Player player) {
    }


}
