using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IBumpable
{
    public string playerIndex = "1";
    public string playerNumber = "1";

    #region PlayerStats
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public float acceleration = 1;
    public int stockCount = 3;
    #endregion

    #region InputRelated
    float horizontalInput;
    public bool isattacking;
    #endregion

    #region AbilityFunction Related
    public Transform raycastStart;
    int jumpCount;
    bool usingJetpack, canJetpack, coyoteJump;
    #endregion

    #region Abilities
    AAbility aAbility = AAbility.DefaultJump;
    XAbility xAbility = XAbility.DefaultPunch;
    YAbility yAbility = YAbility.None;
    BAbility bAbility = BAbility.DefaultBlock;
    #endregion

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;

    private void Awake() {
        anim = gameObject.GetComponentInChildren<Animator>();
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
        stockCount = 3;
    }

    void FixedUpdate() {

        //Grounded:
        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, Vector2.right * dir, 1f);
        if (hit.collider != null)
        {
            coyoteJump = true;
        }

        if (knockbackState) {
            if (rb.velocity.magnitude < exitKnockbackMagnitude)
                knockbackState = false;
            return;
        }

        Move(horizontalInput);

        //Using JETPACK:
        if(usingJetpack)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * .3f);
        }
    }

    int dir = 1;
    private bool knockbackState;
    public float exitKnockbackMagnitude = 3f;
    [SerializeField]
    private float knockbackAmount = 20;  

    private void Update() {
        if (knockbackState) {
            
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

        //TODO: Reiterate upon turning method.
        if (horizontalInput != 0) {
            dir = (int)Mathf.Sign(horizontalInput);
        }
        if(!isattacking)
            transform.localScale = new Vector3(
                dir, 
                transform.localScale.y, 
                transform.localScale.z);
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
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
        //TODO: Add bump method
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (isattacking) {
            var player = collision.collider.GetComponent<Player>();
            if(player) {
                print("Has connected attack");
                player.Damage((transform.position - player.transform.position).normalized);
            } 
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
        print("Trying to damage");

        rb.velocity = dir * knockbackAmount + Vector2.up;
        //rb.velocity += Vector2.up * 100;
        knockbackState = true;
    }

    public void LooseStock()
    {
        print($"{playerIndex} Lost a Stock!");
        stockCount--;
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
        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, Vector2.right * dir, 1f);

        if(hit.collider != null)
        {
            print("Normal Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }
    private void DoubleJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, Vector2.right * dir, 1f);
        if (hit.collider != null)
        {
            jumpCount = 2;
        }

        if(jumpCount > 0)
        {
            coyoteJump = false;
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            print("Can Double Jump");
        }
        else if(coyoteJump)
        {
            coyoteJump = false;
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            print("Can Double Jump");
        }
    }
    private void Jetpack()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, Vector2.right * dir, 1f);
        if (hit.collider != null)
        {
            canJetpack = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        else if(canJetpack)
        {
            coyoteJump = false;
            canJetpack = false;
            StartCoroutine(JetPackBurst());
            print("Using the Jetpack so High");
        }
        else if(coyoteJump)
        {
            coyoteJump = false;
            canJetpack = false;
            StartCoroutine(JetPackBurst());
            print("Using the Jetpack so High");
        }
    }
    private void BouncyShoes()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, Vector2.right * dir, 1f);

        if (hit.collider != null)
        {
            print("Look at me flying with these boots");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * 2);
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
        anim.SetBool("DefaultAttack", true);
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
