using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WorldOptions", menuName ="MarioMenu/New World Options")]
public class WorldOptions : ScriptableObject
{
    public AbilityAsset aDefault;
    public AbilityAsset xDefault;
    public AbilityAsset yDefault;
    public AbilityAsset bDefault;

    public List<AbilityAsset> aAbilities;
    public List<AbilityAsset> xAbilities;
    public List<AbilityAsset> yAbilities;
    public List<AbilityAsset> bAbilities;
}
