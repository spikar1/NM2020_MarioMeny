using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JumpAbility", menuName = "MarioMenu/New Jump Ability")]
public class JumpAbility : AbilityAsset
{
    public float jumpHeight = 10;

    bool held;

    public override void ButtonDown(Player player) {
        Jump(player);
    }

    public override void ButtonUp(Player player) {
    }

    public override void ButtonHeld(Player player) {

    }

    void Jump(Player player) {

        player.rb.velocity = new Vector2(player.rb.velocity.x, jumpHeight);
    }
}
