using System;
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
    StockCanvas stockCanvas;
    #endregion

    #region AbilityFunction Related
    //Player States:
    bool isUsingAbility = false;
    bool isAttacking = false;
    public bool isBlocking = false;

    bool canUseAbility = true;
    float startGravity;
    float abilityNumber;

    //A Abilities:
    int jumpCount;
    float jetpackAmount;
    bool usingJetpack, canJetpack;
    public bool coyoteJump;

    //X Abilities:
    bool isChargingSword;
    float swordCharge, slamPower;
    private float knockbackAmount; 
    bool knockbackState, canSlam;
    #endregion

    #region Cooldown system
    private float aCooldown, xCooldown, yCooldown, bCooldown;
    private float bubbleGunCooldown => Manager.WorldOptions.bubbleGunCooldownTime;
    #endregion

    #region Abilities:
    public BAbility bAbility = BAbility.DefaultBlock;
    public AAbility aAbility = AAbility.DefaultJump;
    public XAbility xAbility = XAbility.DefaultPunch;
    public YAbility yAbility = YAbility.None;
    private static bool isIcyFloor = false;
    private static int iceIndex = -1;

    #endregion
    float groundedXOffset = .43f;
    float groundedYOffset = .7f;

    bool Grounded {
        get {
            bool b = !Physics2D.Linecast(
                transform.position + Vector3.down * groundedYOffset + Vector3.right * groundedXOffset,
                transform.position + Vector3.down * groundedYOffset + Vector3.left * groundedXOffset
                );
            print("Bool - " + b);
            return b;
        }
    }

    private void Awake() {
        //Ref:
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();
        textMesh = GetComponent<TextMeshPro>();
        stockCanvas = GameObject.FindGameObjectWithTag("StockCanvas").GetComponent<StockCanvas>();

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

        ResetKnockback();
    }

    void FixedUpdate() {
        if (playerIsDead || knockbackState)
            return;

        if (Manager.WorldOptions.useAcceleration)
            Accelerate(horizontalInput);
        else
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
       
        if(isChargingSword)
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
            if(canUseAbility && xCooldown <= 0.01f)
            {
                canUseAbility = false;
                isUsingAbility = true;
                isAttacking = true;
                DoXAbility();
            }
        }
        if (Input.GetButtonDown("Y" + "_P" + playerIndex))
        {
            if(canUseAbility && yCooldown <= 0.01f)
                DoYAbility();
        }
        if (Input.GetButtonDown("B" + "_P" + playerIndex))
        {
            if (canUseAbility && bCooldown <= 0.01f)
            {
                canUseAbility = false;
                isUsingAbility = true;
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
        else if(isAttacking && !isChargingSword){
            rend.flipX = Mathf.Sign(rb.velocity.x) > 0 ? false : true;
        }

        UpdateCooldowns();
        JetPackFlying();
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

        if (Input.GetKeyDown(KeyCode.I))
            isIcyFloor = true;

        if (Input.GetKeyDown(KeyCode.K))
            isIcyFloor = false;

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
    private void Accelerate(float direction) {
        if (isUsingAbility)
            return;
        if (Mathf.Abs(direction) > .1f) {
            var canMove = false;

            if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(horizontalInput))
                canMove = true;
            else if (Mathf.Abs(rb.velocity.x) < Manager.WorldOptions.maxSpeed)
                canMove = true;

            anim.SetFloat("HorizontalMovement", horizontalInput);

            var acc = Mathf.Sign(rb.velocity.x) == Mathf.Sign(direction) ? Manager.WorldOptions.acceleration : Manager.WorldOptions.deaccelerationSpeed;
            if (isIcyFloor && iceIndex != playerNumber && Grounded)
                acc = Manager.WorldOptions.iceFloorAcceleration;

            if (canMove)
                rb.velocity += new Vector2(direction * acc, 0);
            if(Mathf.Abs(rb.velocity.x) > Manager.WorldOptions.maxSpeed) {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * Manager.WorldOptions.maxSpeed, rb.velocity.y);
            }
        }
        else {
            var acc = Manager.WorldOptions.deaccelerationSpeed;
            if (isIcyFloor && iceIndex != playerNumber && Grounded)
                acc = Manager.WorldOptions.iceFloorAcceleration;
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, acc), rb.velocity.y);
        }
        
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
        //TODO: Add bump method
    }


    private void OnCollisionEnter2D(Collision2D c) {
        if (Manager.WorldOptions.bounceGameplay && 
            rb.velocity.y > -1f &&
            rb.velocity.y < 5f
            ) {
            rb.velocity = new Vector2(rb.velocity.x, 0);

        }

        if (isBlocking)
            rb.velocity *= new Vector2(1, Manager.WorldOptions.shieldVelocityCutoff);

        if(c.GetContact(0).normal.y > 0.2f)
        {
            coyoteJump = true;
            usingJetpack = false;
            jetpackAmount = Manager.WorldOptions.MaxJetpackDuration;
            anim.SetBool("JumpAnim", false);
        }

        Transform other = c.collider.transform;
        float otherY = other.position.y;
        IBumpable bumpable = other.GetComponent<IBumpable>();
        if (bumpable == null)
            return;


        if(otherY > transform.position.y && c.GetContact(0).normal.y < 0) {
            bumpable.Bumped(this, c.relativeVelocity);          
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (!isAttacking)
            return;
        var player = col.GetComponent<Player>();
        if (!player)
            return;

        isAttacking = false;
        isUsingAbility = false;
        anim.SetBool("UsingAbility", false);

        if(Manager.WorldOptions.bounceGameplay)
        {
            player.BouncyDamage((player.transform.position - transform.position).normalized);
        }
        else
        {
            player.Damage((player.transform.position - transform.position).normalized);
        }
    }

    public void ResetKnockback()
    {
        if(Manager.WorldOptions.bounceGameplay)
        {
            knockbackAmount = Manager.WorldOptions.bouncyKnockback;
        }
        else
        {
            knockbackAmount = Manager.WorldOptions.KnockbackStartAmount;
        }
    }

    public void Damage(Vector2 dir) {
        if(isBlocking)
        {
            rb.velocity = dir * knockbackAmount * Manager.WorldOptions.blockedKnockback + (Vector2.up * knockbackAmount * 0.5f * Manager.WorldOptions.blockedKnockback);
            StartCoroutine(KnockbackTimer());
            knockbackAmount += Manager.WorldOptions.knockbackScaler * Manager.WorldOptions.blockedKnockback;
        }
        else
        {
            rb.velocity = dir * knockbackAmount + (Vector2.up * knockbackAmount * 0.5f);
            StartCoroutine(KnockbackTimer());      
            knockbackAmount += Manager.WorldOptions.knockbackScaler;
        }
    }

    public void BouncyDamage(Vector2 dir)
    {
        if (isBlocking)
        {
            knockbackState = true;
            rb.velocity = dir * knockbackAmount * Manager.WorldOptions.blockedKnockback + (Vector2.up * knockbackAmount * 0.5f * Manager.WorldOptions.blockedKnockback);
            StartCoroutine(KnockbackTimer());
        }
        else
        {
            print("Hit");
            knockbackState = true;
            rb.velocity = dir * knockbackAmount + (Vector2.up * knockbackAmount * 0.5f);
            StartCoroutine(KnockbackTimer());
        }
    }

    IEnumerator KnockbackTimer()
    {
        if(isBlocking)
        {
            yield return new WaitForSeconds(knockbackAmount * 0.05f * Manager.WorldOptions.blockedKnockback);
            knockbackState = false;
        }
        else
        {
            yield return new WaitForSeconds(knockbackAmount * 0.05f);
            knockbackState = false;
        }

    }

    public void LooseStock()
    {
        print($"{playerIndex} Lost a Stock!");
        stockCount--;
        playerIsDead = true;
        anim.SetBool("IsDead", true);

        stockCanvas.UpdatePlayerStock(playerNumber, stockCount);
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
            anim.SetBool("IsRocket", false);
        }

        if(jetpackAmount <= 0)
        {
            anim.SetBool("JumpAnim", true);
            anim.SetBool("IsRocket", false);
            usingJetpack = false;
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
            default:
                break;
        }
    }

    void DoYAbility() {
        switch (yAbility) {
            case YAbility.None:
                None();
                break;
            case YAbility.Blizzard:
                StartCoroutine(Blizzard());
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
            case BAbility.Hammer:
                StartCoroutine(Hammer());
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
            print("I Jumped");
            anim.SetBool("JumpAnim", true);
            rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.jumpHeight);
            coyoteJump = false;

            Manager.SoundManager.PlayJumpSound();
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

            Manager.SoundManager.PlayJumpSound();
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

            Manager.SoundManager.PlayJumpSound();
        }
        else if(canJetpack)
        {
            anim.SetBool("JumpAnim", false);
            canJetpack = false;
            usingJetpack = true;


            Manager.SoundManager.PlayJetPackLoop();
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
        isAttacking = true;
        rb.velocity = new Vector2(Manager.WorldOptions.punchDistance * dir, 0);

        //Sound
        Manager.SoundManager.PlayDefaultPunch();


        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.2f));
        xCooldown += Manager.WorldOptions.defaultPunchCooldownTime;
        print("DefaultPunch");
    }
    IEnumerator Sword()
    {
        //Animation:
        abilityNumber = 8;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        //Ability Func:
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0.1f;
        swordCharge = 0;
        isChargingSword = true;
        Manager.SoundManager.PlaySwordChargeUp();

        var t = 0f;
        var p = transform.GetChild(0).localPosition;
        while (!Input.GetButtonUp("X" + "_P" + playerIndex) && !Input.GetKeyUp(KeyCode.Alpha2)) {
            //transform.position = p + new Vector3(Mathf.Sin(t * swordCharge * 60) * swordCharge * .1f, 0, 0);
            transform.GetChild(0).localPosition = p + new Vector3(Mathf.Sin(t * swordCharge * 60) * swordCharge * .1f, 0, 0);
            t += Time.deltaTime;
            yield return null;
        }
        transform.GetChild(0).localPosition = p;
        //yield return new WaitUntil(() => Input.GetButtonUp("X" + "_P" + playerIndex));

        //Animation:
        abilityNumber = 1;
        anim.SetFloat("AbilityNumber", abilityNumber);

        isChargingSword = false;
        isAttacking = true;
        rb.velocity = new Vector2(swordCharge * 40 * dir, 0);
        rb.gravityScale = 0;

        Manager.SoundManager.PlaySwordSwing();

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(swordCharge * 0.5f));
        yield return new WaitUntil(() => !isUsingAbility);
        rb.gravityScale = startGravity;
        xCooldown += Manager.WorldOptions.swordCooldownTime;
        print("Sword");
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
        isBlocking = true;

        Manager.SoundManager.PlayDefaultBlock();

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
        Manager.SoundManager.PlayReflectiveShield();

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(0.5f));
        print("ReflectiveShield");
    }
    IEnumerator Hammer()
    {
        //Animation:
        abilityNumber = 3;
        anim.SetFloat("AbilityNumber", abilityNumber);
        anim.SetBool("UsingAbility", true);

        Manager.SoundManager.PlayHammerSound();

        //Ability Func:
        slamPower = -1.2f;
        canSlam = true;
        coyoteJump = false;
        canUseAbility = false;
        yield return new WaitUntil(() => coyoteJump);

        canSlam = false;

        //CoolDown And Debug:
        StartCoroutine(AbilityDuration(.2f));
        xCooldown += Manager.WorldOptions.hammerCooldownTime;
        print("Hammer");
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
    private IEnumerator Blizzard()
    {
        //Animation:

        //Ability Func:
        isIcyFloor = true;
        iceIndex = playerNumber;
        print("Blizzard done by player" + playerNumber);
        Manager.SoundManager.PlayBlizzardSound();
        yield return new WaitForSeconds(Manager.WorldOptions.blizzardDuration);
        isIcyFloor = false;
        iceIndex = -1; 
        print("Blizzard by player" + playerNumber + "Has stopped");

        //CoolDown and Debug:
        yCooldown += Manager.WorldOptions.blizzardCooldown;
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

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + Vector3.down * groundedYOffset + Vector3.right * groundedXOffset,
            transform.position + Vector3.down * groundedYOffset + Vector3.left * groundedXOffset
            );
    }
}
