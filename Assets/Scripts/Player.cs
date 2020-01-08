﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour, IBumpable
{
    
    float horizontalInput;
    int dir = 1;

    #region Public Variables & References
    //Variables:
    public string playerIndex = "1";
    public int stockCount, playerNumber = 1;
    public bool playerIsDead;

    //Ref:
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public SpriteRenderer rend;
    [HideInInspector]
    public TextMeshPro textMesh;
    #endregion

    #region AbilityFunction Related
    //Player States:
    bool isUsingAbility = false;
    bool isAttacking = false;
    bool isBlocking = false;

    bool canUseAbility = true;
    float startGravity;
    float abilityNumber;

    //A Abilities:
    int jumpCount;
    float jetpackAmount, extraHeight;
    bool usingJetpack, canJetpack, coyoteJump, canHighJump;
    Quaternion desiredRot;

    //X Abilities:
    bool chargingSword;
    float swordCharge, slamPower;
    private float knockbackAmount; 
    bool knockbackState, canSlam;
    #endregion

    #region Cooldown system
    private float aCooldown, xCooldown, yCooldown, bCooldown;
    private float missileCooldown => Manager.WorldOptions.missileCooldownTime;
    private float bubbleGunCooldown => Manager.WorldOptions.bubbleGunCooldownTime;
    #endregion

    #region Abilities:
    public BAbility bAbility = BAbility.DefaultBlock;
    public AAbility aAbility = AAbility.DefaultJump;
    public XAbility xAbility = XAbility.DefaultPunch;
    public YAbility yAbility = YAbility.None;
    
    #endregion

    private void Awake() {
        //Ref:
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();
        textMesh = GetComponent<TextMeshPro>();

        //Dont Destroy:
        DontDestroyOnLoad(gameObject);

        bAbility = Manager.WorldOptions.bDefault;
        aAbility = Manager.WorldOptions.aDefault;
        xAbility = Manager.WorldOptions.xDefault;
        yAbility = Manager.WorldOptions.yDefault;
    }

    private void Start() {
        stockCount = Manager.WorldOptions.stockCount;
        textMesh.text = "P" + playerNumber;
        textMesh.color = Manager.WorldOptions.playerColors[playerNumber - 1];
        rend.color = Manager.WorldOptions.playerColors[playerNumber - 1];

        startGravity = rb.gravityScale;
        playerIsDead = false;
        desiredRot = Quaternion.identity;
    }

    void FixedUpdate() {
        if (playerIsDead || knockbackState)
            return;

        Move(horizontalInput);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }


    private void Update() {
        if (playerIsDead || knockbackState) {
            return;
        }



        horizontalInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);
        if (Manager.debugMode)
        {
            if (Input.GetKey(KeyCode.A))
                horizontalInput -= 1;
            if (Input.GetKey(KeyCode.D))
                horizontalInput += 1;
        }
       
        if(chargingSword)
        {
            if(swordCharge < Manager.WorldOptions.maxSwordCharge)
            {
                swordCharge += Time.deltaTime;
            }
            else
            {
                swordCharge = Manager.WorldOptions.maxSwordCharge;
            }
        }

        if (Manager.debugMode) {
            DebugUpdate();
        }

        if (Input.GetButtonDown("A" + "_P" + playerIndex)) 
        {
            if(canUseAbility)
            {
                DoAAbility();
            }
        }
        if (Input.GetButtonDown("X" + "_P" + playerIndex)) 
        {
            if(canUseAbility)
            {
                canUseAbility = false;
                isUsingAbility = true;
                isAttacking = true;
                DoXAbility();
            }
        }
        if (Input.GetButtonDown("Y" + "_P" + playerIndex))
        {
            if(canUseAbility)
                DoYAbility();
        }
        if (Input.GetButtonDown("B" + "_P" + playerIndex))
        {
            if (canUseAbility)
            {
                canUseAbility = false;
                isUsingAbility = true;
                isBlocking = true;
                DoBAbility();
            }
        }


        if (horizontalInput != 0 && !isUsingAbility) {
            dir = (int)Mathf.Sign(horizontalInput);
        }  

        if(!isUsingAbility)
        {
            if (dir == -1)
            {
                rend.flipX = true;
            }
            else
                rend.flipX = false;
        }

        UpdateCooldowns();
        JetPackFlying();
        HighJump();
        SlamDunk();
    }


    private void UpdateCooldowns()
    {
        aCooldown = Mathf.Clamp(aCooldown - Time.deltaTime, 0, 999);
        xCooldown = Mathf.Clamp(xCooldown - Time.deltaTime, 0, 999);
        yCooldown = Mathf.Clamp(yCooldown - Time.deltaTime, 0, 999);
        bCooldown = Mathf.Clamp(bCooldown - Time.deltaTime, 0, 999);
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (canUseAbility)
                DoAAbility();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (canUseAbility) {
                canUseAbility = false;
                isUsingAbility = true;
                isAttacking = true;
                DoXAbility();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (canUseAbility)
                DoYAbility();

        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (canUseAbility)
                DoBAbility();

        }
    }

    public void StopAbility(string abilityString) {
        anim.SetBool(abilityString, false);
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
        if (isUsingAbility)
            return;
        rb.velocity = new Vector2(direction * Manager.WorldOptions.maxSpeed, rb.velocity.y);
        anim.SetFloat("HorizontalMovement", horizontalInput);
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
        //TODO: Add bump method
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.GetContact(0).normal.y > 0)
        {
            coyoteJump = true;
            usingJetpack = false;
            jetpackAmount = Manager.WorldOptions.MaxJetpackDuration;
            anim.SetBool("JumpAnim", false);
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
        if (!isUsingAbility)
            return;
        var player = col.GetComponent<Player>();
        if (!player)
            return;
        player.Damage((player.transform.position - transform.position).normalized);
    }

    public void ResetKnockback()
    {
        knockbackAmount = Manager.WorldOptions.KnockbackStartAmount;
    }

    public void Damage(Vector2 dir) {
        rb.velocity = dir * knockbackAmount + (Vector2.up * knockbackAmount * 0.5f);
        StartCoroutine(KnockbackTimer());      
        knockbackAmount += 1;
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
        anim.SetBool("IsDead", true);
    }

    public void CheckIfStillDead()
    {
        if(stockCount > 0)
        {
            playerIsDead = false;
            anim.SetBool("IsDead", false);
        }
    }

    private void JetPackFlying()
    {      
        if (!canUseAbility)
            return;
        

        if(usingJetpack)
        {
            if(Input.GetButton("A" + "_P" + playerIndex))
            {
                jetpackAmount -= Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight * .3f);
                anim.SetBool("JumpAnim", false);
                anim.SetBool("IsRocket", true);
            }
            else
            {
                anim.SetBool("JumpAnim", true);
                anim.SetBool("IsRocket", false);
            }
        }
        else
        {
            //anim.SetBool("IsRocket", false);
        }

        if(jetpackAmount <= 0)
        {
            anim.SetBool("JumpAnim", true);
            anim.SetBool("IsRocket", false);
            usingJetpack = false;
        }
    }

    private void HighJump()
    {
        if(canHighJump)
        {
            if (Input.GetButton("A" + "_P" + playerIndex))
            {
                rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight);
                extraHeight += Time.deltaTime;
            }
        }

        if(extraHeight >= .5f || Input.GetButtonUp("A" + "_P" + playerIndex))
        {
            canHighJump = false;
        }
    }

    private void SlamDunk()
    {
        if(canSlam)
        {
            slamPower += Time.deltaTime * 5;
            rb.velocity = new Vector2(0, -Manager.WorldOptions.jumpHeight * slamPower);
        }
    }

    IEnumerator AbilityDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isUsingAbility = false;
        canUseAbility = true;
        isAttacking = false;
        isBlocking = false;
        anim.SetBool("UsingAbility", false);
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
        switch (xAbility) {
            case XAbility.DefaultPunch:
                DefaultPunch();
                break;
            case XAbility.Sword:
                StartCoroutine(Sword());
                break;
            case XAbility.Axe:
                Axe();
                break;
            case XAbility.Hammer:
                StartCoroutine(Hammer());
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
    #region A Abilities
    private void DefaultJump() {
        if(coyoteJump)
        {
            anim.SetBool("JumpAnim", true);
            rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight);
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
            anim.SetBool("JumpAnim", true);
            rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight);
            print("Can Double Jump");
            coyoteJump = false;
        }
    }
    private void Jetpack()
    {
        if (coyoteJump)
        {
            anim.SetBool("JumpAnim", true);
            rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight);
            coyoteJump = false;
            canJetpack = true;
        }
        else if(canJetpack)
        {
            anim.SetBool("JumpAnim", false);
            canJetpack = false;
            usingJetpack = true;
        }
    }
    private void BouncyShoes()
    {
        if (coyoteJump)
        {
            anim.SetBool("JumpAnim", true);
            coyoteJump = false;
            extraHeight = 0;
            canHighJump = true;
        }
    }
    #endregion

    //X Abilities:
    #region X Abilities
    private void DefaultPunch()
    {
        //Animation:
        abilityNumber = 0;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:
        rb.velocity = new Vector2(Manager.WorldOptions.punchDistance * dir, 0);

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.5f));
        print("DefaultPunch");
    }
    IEnumerator Sword()
    {
        //Animation:
        abilityNumber = 1;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0.1f;
        swordCharge = 0;
        chargingSword = true;
        
        yield return new WaitUntil(() => Input.GetButtonUp("X" + "_P" + playerIndex));
        chargingSword = false;
        rb.velocity = new Vector2(swordCharge * 40 * dir, 0);
        rb.gravityScale = 0;

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(swordCharge * 0.5f));
        yield return new WaitUntil(() => !isUsingAbility);
        rb.gravityScale = startGravity;
        print("Sword");
    }
    private void Axe()
    {
        //Animation:
        abilityNumber = 2;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.5f));
        print("Axe");
    }
    IEnumerator Hammer()
    {
        //Animation:
        abilityNumber = 3;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:
        slamPower = -1.2f;
        canSlam = true;
        coyoteJump = false;
        yield return new WaitUntil(() => coyoteJump);
        canSlam = false;

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.2f));
        print("Hammer");
    }
    #endregion

    //B Abilities:
    #region B Abilities
    private void DefaultBlock()
    {
        //Animation:
        abilityNumber = 4;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(0.5f));
        print("DefaultBlock");
    }
    private void ReflectiveShield()
    {
        //Animation:
        abilityNumber = 5;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(0.5f));
        print("ReflectiveShield");
    }
    private void Barrier()
    {
        //Animation:
        abilityNumber = 6;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.5f));
        print("Barrier");
    }
    private void Evade()
    {
        //Animation:
        abilityNumber = 7;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.5f));
        print("Evade");
    }
    #endregion


    //Y Abilities:
    #region Y Abilities
    private void None()
    {
        //Animation:

        //Ability Func:

        //CoolDown and Debug:
        print("None");
    }
    private void Missile()
    {
        if (yCooldown > 0)
            return;
        //Animation:

        //Ability Func:
        var missile = Instantiate(Manager.WorldOptions.missilePrefab, transform.position + Vector3.right * dir * 1.6f, transform.rotation);
        missile.GetComponent<Projectile>().Initialize(dir, Manager.WorldOptions.missileSpeed);
        //CoolDown and Debug:
        yCooldown += missileCooldown;
        print("Missile");
    }
    private void PickUp()
    {
        //Animation:

        //Ability Func:

        //CoolDown and Debug:
        print("PickUp");
    }
    private void FireShield()
    {
        //Animation:

        //Ability Func:

        //CoolDown and Debug:
        print("FireShield");
    }
    private void BubbleGun()
    {
        if (yCooldown > 0)
            return;
        //Animation:

        //Ability Func:
        var bubbleProjectile = Instantiate(Manager.WorldOptions.bubbleProjectilePrefab, transform.position + Vector3.right * dir * 1.6f, transform.rotation);
        bubbleProjectile.GetComponent<BubbleProjectile>().Initialize(dir, Manager.WorldOptions.bubbleGunProjectileSpeed);
        //CoolDown and Debug:
        yCooldown += bubbleGunCooldown;
        print("Bubble Gun");
    }
    #endregion
    #endregion
}
