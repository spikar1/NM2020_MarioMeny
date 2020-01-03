using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewAbilty", menuName = "MarioMenu/New Ability Asset")]
abstract public class AbilityAsset : ScriptableObject
{
    public Sprite icon;
    

    abstract public void ButtonDown(Player player);
    abstract public void ButtonUp(Player player);
    abstract public void ButtonHeld(Player player);

}
