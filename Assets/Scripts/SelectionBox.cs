using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using TMPro;

using Random = UnityEngine.Random;

public class SelectionBox : MonoBehaviour, IBumpable
{
    Vector2 desiredDirection;
    public Sprite[] sprites;
    SpriteRenderer rend;
    TextMeshPro textMesh;
    public string displayText;
    GameObject manager;

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
    bool canGetAbility;

    void Start()
    {
        canGetAbility = true;
        manager = GameObject.FindGameObjectWithTag("Manage");
        textMesh = GetComponent<TextMeshPro>();

        startPos = transform.position;
        if(abilitySelection)
        {
            abilityType = UnityEngine.Random.Range(0, 4);
            abilityType = DetermineAbilityType();
            rend = GetComponentInChildren<SpriteRenderer>();
            rend.sprite = sprites[abilityType];
            SetAbility();
        }
        else
        {
            textMesh.text = displayText;
        }
    }

    private int DetermineAbilityType()
    {
        //Sum total amount of abilities
        int sum = 0;
        int aSum = Manager.WorldOptions.availableAAbilities.Count;
        int xSum = Manager.WorldOptions.availableXAbilities.Count;
        int ySum = Manager.WorldOptions.availableYAbilities.Count;
        int bSum = Manager.WorldOptions.availableBAbilities.Count;
        sum += aSum;
        sum += xSum;
        sum += ySum;
        sum += bSum;

        //If the total amount of abilites is zero....
        if (sum <= 0)
            throw new Exception("No available abilities found in world options");

        var r = Random.Range(0, sum);

        //Use a "weighted" random to determine ability type
        r -= aSum;
        if (r < 0)
            return 0;
        r -= xSum;
        if (r < 0)
            return 1;
        r -= ySum;
        if (r < 0)
            return 2;
        r -= bSum;
        if (r < 0)
            return 3;

        //If this happens, I do not know how..... :s
        throw new Exception("Something is not right...");
    }

    public void Bumped(Player bumpee, Vector2 collisionVector) {
        //Invoke Unity Events
        if (SceneManager.GetActiveScene().name == "Init Scene")
        {
            if(bumpee.playerNumber.ToString() == "1")
                StartCoroutine(WaitBeforeEvents());
        }
        else if(SceneManager.GetActiveScene().name == "WinScreen")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(player);
            manager.GetComponent<Manager>().sceneLoader.LoadScene("Init Scene");
        }
        else
        {
            StartCoroutine(WaitBeforeEvents());
        }

        if(abilitySelection && bumpee.stockCount > 1)
        {
            if(canGetAbility)
            {
                canGetAbility = false;
                bumpee.LooseStock();
                UpdatePlayersAlive();

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

                StartCoroutine(SelfDestruct());
            }
        }

        BumpEffect(bumpee, collisionVector);
    }


    IEnumerator WaitBeforeEvents() {
        yield return new WaitForSeconds(.4f);

        onBumped.Invoke();
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
                var abilityListA = Manager.WorldOptions.availableAAbilities;
                var rA = Random.Range(0, abilityListA.Count);
                textMesh.text = abilityListA[rA].ToString();
                newAAbility = abilityListA[rA];

                //First get Enum length, then select one random:
                /*abilitySubType = UnityEngine.Random.Range(1,Enum.GetValues(typeof(AAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(AAbility)))
                {
                    if(i == abilitySubType)
                    {
                        print((AAbility)i);
                        textMesh.text = Enum.GetName(typeof(AAbility), i);
                        newAAbility = (AAbility)i;
                    }
                }*/
                break;

            case 1:
                var abilityListX = Manager.WorldOptions.availableXAbilities;
                var rX = Random.Range(0, abilityListX.Count);
                textMesh.text = abilityListX[rX].ToString();
                newXAbility = abilityListX[rX];

                //First get Enum length, then select one random:
                /*abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(XAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(XAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((XAbility)i);
                        textMesh.text = Enum.GetName(typeof(XAbility), i);
                        newXAbility = (XAbility)i;
                    }
                }*/
                break;

            case 2:
                var abilityListY = Manager.WorldOptions.availableYAbilities;
                var rY = Random.Range(0, abilityListY.Count);
                textMesh.text = abilityListY[rY].ToString();
                newYAbility = abilityListY[rY];

                //First get Enum length, then select one random:
                /*abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(YAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(YAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((YAbility)i);
                        textMesh.text = Enum.GetName(typeof(YAbility), i);
                        newYAbility = (YAbility)i;
                    }
                }*/
                break;

            case 3:
                var abilityListB = Manager.WorldOptions.availableBAbilities;
                var rB = Random.Range(0, abilityListB.Count);
                textMesh.text = abilityListB[rB].ToString();
                newBAbility = abilityListB[rB];

                //First get Enum length, then select one random:
                /*abilitySubType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(BAbility)).Length);
                foreach (int i in Enum.GetValues(typeof(BAbility)))
                {
                    if (i == abilitySubType)
                    {
                        print((BAbility)i);
                        textMesh.text = Enum.GetName(typeof(BAbility), i);
                        newBAbility = (BAbility)i;
                    }
                }*/
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

    private void UpdatePlayersAlive()
    {
        GameObject selectionChecker = GameObject.FindGameObjectWithTag("CheckSelection");
        var selectionCheck = selectionChecker.GetComponent<CheckSelection>();
        if(selectionCheck != null)
        {
            selectionCheck.checkDeadPlayers();
        }
    }
}
