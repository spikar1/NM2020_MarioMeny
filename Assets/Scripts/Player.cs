using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour, IBumpable
{
    public string playerIndex = "1";
    public int playerNumber = 1;

    public Color[] playerColors;

    #region PlayerStats
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public float acceleration = 1;
    public int stockCount = 3;
    public float punchDistance = 15;
    #endregion

    #region InputRelated
    float horizontalInput;
    public bool isattacking;
    #endregion

    #region AbilityFunction Related
    int jumpCount;
    bool usingJetpack, canJetpack, coyoteJump;
    bool playerIsDead;
    #endregion

    #region Abilities
    public AAbility aAbility = AAbility.DefaultJump;
    public XAbility xAbility = XAbility.DefaultPunch;
    public YAbility yAbility = YAbility.None;
    public BAbility bAbility = BAbility.DefaultBlock;
    #endregion

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;

    SpriteRenderer rend;
    TextMeshPro textMesh;

    private void Awake() {
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();
        textMesh = GetComponent<TextMeshPro>();
        rb = GetComponent<Rigidbody2D>();

        DontDestroyOnLoad(gameObject);
    }

    public void ResetPlayer() {
        aAbility = Manager.worldOptions.aDefault;
        xAbility = Manager.worldOptions.xDefault;
        yAbility = Manager.worldOptions.yDefault;
        bAbility = Manager.worldOptions.bDefault;
    }

    private void Start() {
        textMesh.text = "P" + playerNumber;
        textMesh.color = playerColors[playerNumber - 1];
        stockCount = 3;

        rend.color = playerColors[playerNumber - 1];

        playerIsDead = false;
    }

    void FixedUpdate() {
        if (playerIsDead || knockbackState)
            return;

        //Using JETPACK:
        if (usingJetpack)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * .3f);
        }


        Move(horizontalInput);
    }

    int dir = 1;
    private bool knockbackState;
    [SerializeField]
    private float knockbackAmount = 10;  

    private void Update() {
        if (playerIsDead || knockbackState) {       
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);
       

        if (Input.GetButtonDown("A" + "_P" + playerIndex)) {
            DoAAbility();
        }
        if (Input.GetButtonDown("X" + "_P" + playerIndex)) {
            DoXAbility();
        }
        if (Input.GetButtonDown("Y" + "_P" + playerIndex))
        {
            DoYAbility();
        }
        if (Input.GetButtonDown("B" + "_P" + playerIndex))
        {
            DoBAbility();
        }


        if (horizontalInput != 0) {
            dir = (int)Mathf.Sign(horizontalInput);
        }  

        if(!isattacking)
        {
            if (dir == -1)
            {
                rend.flipX = true;
            }
            else
                rend.flipX = false;
        }
    }

    public void StopAbility(string abilityString) {
        anim.SetBool(abilityString, false);
        isattacking = false;
    }

    #region ChangeAbilities
    public void ChangeAAbility(AAbility newAAbility)
    {
        aAbility = newAAbility;
    }
    public void ChangeXAbility(XAbility newXAbility)
    {
        xAbility = newXAbility;
    }
    public void ChangeYAbility(YAbility newYAbility)
    {
        yAbility = newYAbility;
    }
    public void ChangeBAbility(BAbility newBAbility)
    {
        bAbility = newBAbility;
    }
    #endregion


    private void Move(float direction) {
        if (isattacking)
            return;
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
        anim.SetFloat("HorizontalMovement", horizontalInput);
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
        //TODO: Add bump method
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.GetContact(0).normal.y > 0)
        {
            print("Hit");
            coyoteJump = true;
        }


        Transform other = collision.collider.transform;
        float otherY = other.position.y;
        IBumpable bumpable = other.GetComponent<IBumpable>();
        if (bumpable == null)
            return;


        if(otherY > transform.position.y && collision.GetContact(0).normal.y < 0) {
            bumpable.Bumped(this, collision.relativeVelocity);          
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (!isattacking)
            return;
        var player = col.GetComponent<Player>();
        if (!player)
            return;
        player.Damage((player.transform.position - transform.position).normalized);
    }

    private void Damage(Vector2 dir) {
        rb.velocity = dir * knockbackAmount + (Vector2.up * knockbackAmount * 0.5f);
        StartCoroutine(KnockbackTimer());
    }

    IEnumerator KnockbackTimer()
    {
        knockbackState = true;
        yield return new WaitForSeconds(knockbackAmount * 0.05f);
        knockbackState = false;
    }

    public void LooseStock()
    {
        print($"{playerIndex} Lost a Stock!");
        stockCount--;
        playerIsDead = true;
    }

    public void CheckIfStillDead()
    {
        if(stockCount > 0)
        {
            playerIsDead = false;
        }
    }


    #region Do Abilities
    void DoAAbility() {
        switch (aAbility) {
            case AAbility.DefaultJump:
                DefaultJump();
                break;
            case AAbility.DoubleJump:
                DoubleJump();
                break;
            case AAbility.Jetpack:
                Jetpack();
                break;
            case AAbility.BouncyShoes:
                BouncyShoes();
                break;
            default:
                break;
        }
    }

    void DoXAbility() {
        isattacking = true;
        switch (xAbility) {
            case XAbility.DefaultPunch:
                DefaultPunch();
                break;
            case XAbility.Sword:
                Sword();
                break;
            case XAbility.Axe:
                Axe();
                break;
            case XAbility.Hammer:
                Hammer();
                break;
            default:
                break;
        }
    }

    void DoYAbility() {
        switch (yAbility) {
            case YAbility.None:
                None();
                break;
            case YAbility.Missile:
                Missile();
                break;
            case YAbility.PickUp:
                PickUp();
                break;
            case YAbility.FireShield:
                FireShield();
                break;
            case YAbility.BubbleGun:
                BubbleGun();
                break;
            default:
                break;
        }
    }


    void DoBAbility() {
        switch (bAbility) {
            case BAbility.DefaultBlock:
                DefaultBlock();
                break;
            case BAbility.ReflectiveShield:
                ReflectiveShield();
                break;
            case BAbility.Barrier:
                Barrier();
                break;
            case BAbility.Evade:
                Evade();
                break;
            default:
                break;
        }
    }
    #endregion


    #region Ability Functions
    //A Abilities:
    private void DefaultJump() {
        if(coyoteJump)
        {
            anim.SetBool("JumpAnim", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            coyoteJump = false;  
        }
    }
    private void DoubleJump()
    {
        if (coyoteJump)
        {
            jumpCount = 2;
        }

        if(jumpCount > 0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            print("Can Double Jump");
            coyoteJump = false;
        }
    }
    private void Jetpack()
    {
        if (coyoteJump)
        {
            canJetpack = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            coyoteJump = false;
        }
        else if(canJetpack)
        {
            canJetpack = false;
            StartCoroutine(JetPackBurst());
            print("Using the Jetpack so High");
        }
    }
    private void BouncyShoes()
    {
        if (coyoteJump)
        {
            print("Look at me flying with these boots");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * 2);
            coyoteJump = false;         
        }
    }

    IEnumerator JetPackBurst()
    {
        usingJetpack = true;
        yield return new WaitForSeconds(2);
        usingJetpack = false;
    }


    //X Abilities:
    private void DefaultPunch()
    {
        isattacking = true;
        anim.SetBool("DefaultAttack", true);
        rb.velocity = new Vector2(punchDistance * dir, 0);
        print("Punching the Bastard");
    }
    private void Sword()
    {
        print("Attacking with Sword");
    }
    private void Axe()
    {
        print("Swinging Axe");
    }
    private void Hammer()
    {
        print("Toss a Hammer Wil ya?");
    }


    //Y Abilities:
    private void None()
    {
        print("You aint got nothing son");
    }
    private void Missile()
    {
        print("Shoot tha damn missile BOI");
    }
    private void PickUp()
    {
        print("He He imma pick u Up Fool");
    }
    private void FireShield()
    {
        print("Burn EveryBody around me");
    }
    private void BubbleGun()
    {
        print("You got stuck in bubble mate");
    }


    //B Abilities:
    private void DefaultBlock()
    {
        print("You just got BLOCKED");
    }
    private void ReflectiveShield()
    {
        print("Blocked and i send it back");
    }
    private void Barrier()
    {
        print("Setting up a Barrier");
    }
    private void Evade()
    {
        print("Can't hit us, fool");
    }
    #endregion
}
