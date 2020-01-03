using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour, IBumpable
{
    AbilityAsset abilityDisplay;

    public void Bumped(Player bumpee)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        abilityDisplay = Manager.worldOptions.xAbilities[Random.Range(0, Manager.worldOptions.xAbilities.Count)];
    }

    void Update()
    {
        
    }
}
