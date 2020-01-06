using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class SelectionBox : MonoBehaviour, IBumpable
{
    Vector2 desiredDirection;
    public Sprite[] sprites;
    SpriteRenderer rend;

    Vector2 startPos;
    Vector2 offset;
    float decayingOffset;

    public bool abilitySelection;
    public UnityEvent onBumped;

    //Setting Contained Ability:
    AAbility newAAbility;
    XAbility newXAbility;
    YAbility newYAbility;
    BAbility newBAbility;

    int abilityType;
    int abilitySubType;

    void Start()
    {
        startPos = transform.position;
        if(abilitySelection)
        {
            abilityType = UnityEngine.Random.Range(0, 4);
            rend = GetComponentInChildren<SpriteRenderer>();
            rend.sprite = sprites[abilityType];
            SetAbility();
        }
    }


    public void Bumped(Player bumpee, Vector2 collisionVector) {
        //Invoke Unity Events

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if(bumpee.playerNumber.ToString() == "1")
                StartCoroutine(WaitBeforeEvents());
        }
        else
        {
            StartCoroutine(WaitBeforeEvents());
        }

        if(abilitySelection)
        {
            bumpee.LooseStock();

            switch (abilityType)
            {
                case 0:
                    bumpee.ChangeAAbility(newAAbility);
                    break;
                case 1:
                    bumpee.ChangeXAbility(newXAbility);
                    break;
                case 2:
                    bumpee.ChangeYAbility(newYAbility);
                    break;
                case 3:
                    bumpee.ChangeBAbility(newBAbility);
                    break;
                default:
                    break;
            }
        }

        BumpEffect(bumpee, collisionVector);
    }


    IEnumerator WaitBeforeEvents() {
        yield return new WaitForSeconds(.4f);

        onBumped.Invoke();
    }


    private void BumpEffect(Player bumpee, Vector2 collisionVector) {
        //Do Cool Bumpy Effect:
        decayingOffset = .3f;
        desiredDirection = (transform.position - bumpee.transform.position).normalized;
        offset = desiredDirection * (collisionVector.magnitude * .2f);
    }

    void SetAbility()
    {
        switch (abilityType)
        {
            case 0:
                //First get Enum length, then select one random:
                abilitySubType = UnityEngine.Random.Range(1,Enum.GetValues(typeof(AAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(AAbility)))
                {
                    if(i == abilitySubType)
                    {
                        print((AAbility)i);
                        newAAbility = (AAbility)i;
                    }
                }         
                break;

            case 1:
                //First get Enum length, then select one random:
                abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(XAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(XAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((XAbility)i);
                        newXAbility = (XAbility)i;
                    }
                }
                break;

            case 2:
                //First get Enum length, then select one random:
                abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(YAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(YAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((YAbility)i);
                        newYAbility = (YAbility)i;
                    }
                }
                break;

            case 3:
                //First get Enum length, then select one random:
                abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(BAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(BAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((BAbility)i);
                        newBAbility = (BAbility)i;
                    }
                }
                break;
            default:
                break;
        }
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
