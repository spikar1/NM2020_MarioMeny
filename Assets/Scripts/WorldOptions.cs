using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WorldOptions", menuName ="MarioMenu/New World Options")]
public class WorldOptions : ScriptableObject
{
    public AbilityAsset xDefault;
    public List<AbilityAsset> xAbilities;
}
