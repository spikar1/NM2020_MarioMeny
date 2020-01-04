using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour, IBumpable
{
    AbilityAsset abilityDisplay;
    Vector2 desiredDirection;

    Vector2 startPos;
    Vector2 offset;
    float decayingOffset;


    public void Bumped(Player bumpee, Vector2 collisionVector)
    {
        Debug.Log("Ouch Mah Dude!");

        //Do Cool Bumpy Effect:
        decayingOffset = .3f;
        desiredDirection = (transform.position - bumpee.transform.position).normalized;
        offset = desiredDirection * collisionVector.magnitude;

        //Play Bumped Animation

        //Spawn Contained Ability Asset

        //Player looses a Life but gains said Ability

        //Destroy this Box
    }

    void Start()
    {
        startPos = transform.position;
        //abilityDisplay = Manager.worldOptions.xAbilities[Random.Range(0, Manager.worldOptions.xAbilities.Count)];
    }

    private void Update()
    {
        if(decayingOffset > 0)
        {
            decayingOffset -= 0.8f * Time.deltaTime;
        }
        else
        {
            decayingOffset = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, startPos + (offset * decayingOffset), 0.1f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, (offset.x * -30) * decayingOffset), 1f);
    }
}
