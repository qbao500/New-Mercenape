using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Bao: Mower's Behaviour, child of EnemyBehaviour
public class MowerBehaviour : EnemyBehaviour
{
    private MowerStatsSO mowerStat;

    public enum ForceFieldState { Inactive, Generating, Active, Destroyed }
    private ForceFieldState currentState;

    [SerializeField] private Transform backside;
    private GameObject forceShield;
    private Material shieldMat;

    private float fieldHP;

    private SpriteRenderer fieldSprite;
    private EnemyHealthBar fieldBarHealth;

    private Coroutine dmgCoroutine;

    private bool isAttacking, isGenerating;

    private bool isBackSideHit, isGeneratorHit;

    private CapsuleCollider capsuleCollider;
    private SphereCollider generatorCollider;

    protected override void Awake()
    {
        base.Awake();

        mowerStat = (MowerStatsSO)stat;

        fieldBarHealth = backside.GetChild(0).GetComponent<EnemyHealthBar>();
        fieldSprite = backside.GetComponent<SpriteRenderer>();

        capsuleCollider = backside.GetComponent<CapsuleCollider>();
        generatorCollider = backside.GetComponent<SphereCollider>();
        forceShield = backside.GetChild(1).gameObject;
        shieldMat = forceShield.GetComponent<MeshRenderer>().material;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ChangeToInactive();
        Inactive();

        isAttacking = false; isGenerating = false;

        fieldHP = mowerStat.FieldMaxHP;
        fieldBarHealth.UpdateHealthBar(fieldHP, mowerStat.FieldMaxHP);

        animator.SetBool("IsDestroyed", false); ;

        fieldSprite.enabled = true;
        generatorCollider.enabled = true;
        capsuleCollider.enabled = true;
        fieldBarHealth.gameObject.SetActive(false);
    }

    private void Update() => MowerCheck();

    #region Generator State
    private void Generating()
    {
        if (IsDestroyed) { return; }

        ChangeToGenerating();
        animator.SetTrigger("Generating");

        forceShield.SetActive(true);
        mowerStat.ShieldGenerating(shieldMat);

        fieldSprite.color = Color.yellow;
        speed = 0;
        isGenerating = false;

        Invoke("Active", 2f);
    }

    private void Active()
    {
        if (IsDestroyed) { return; }

        ChangeToActive();
        animator.SetBool("IsActive", true);

        mowerStat.ShieldOn(shieldMat);

        fieldSprite.color = Color.red;
        speed = stat.RunningSpeed;

        Invoke("Inactive", 5f);
    }

    private void Inactive()
    {
        if (IsDestroyed) { return; }

        ChangeToInactive();
        animator.SetBool("IsActive", false);
        forceShield.SetActive(false);

        fieldSprite.color = Color.white;
    }

    private void Destroyed()
    {
        ChangeToDestroyed();
        animator.SetBool("IsDestroyed", true);

        forceShield.SetActive(false);
        speed = stat.RunningSpeed;
        fieldSprite.enabled = false;
        generatorCollider.enabled = false;
        fieldBarHealth.gameObject.SetActive(false);
    }
    #endregion


    #region Damaging Mower and override bleed
    // Check whether backside or generator get hit, then act accordingly
    protected override void EnemyGetHit(bool isMowerBackSide, bool isMowerGenerator, Collider selfCol, float playerDmg)
    {
        this.isBackSideHit = isMowerBackSide;
        this.isGeneratorHit = isMowerGenerator;

        base.EnemyGetHit(isMowerBackSide, isMowerGenerator, selfCol, playerDmg);
    }

    // Special TakerDamage mechanic for Mower
    public override void TakeDamage(float playerDamage)
    {
        if (isBackSideHit)
        {
            DamagingBackside(() => { base.TakeDamage(playerDamage); });
        }

        if (isGeneratorHit) { DamagingForceField(playerDamage); }

        // And take no damage if none of them are true
    }   
    
    // Backside main mechanic
    public void DamagingBackside(Action TakeDamage)
    {      
        if (IsInactive|| IsDestroyed)
        {           
            TakeDamage();

            if (IsInactive) { speed = stat.RunningSpeed / 2; }
            
            if (currentHP <= 0)
            {
                capsuleCollider.enabled = false;

                animator.SetBool("IsDestroyed", false);              
            }

            if (!isGenerating)
            {
                isGenerating = true;

                Invoke("Generating", 1.5f);
            }
        }

        if (IsGenerating)
        {

        }

        if (IsActive)
        {
            // Field Generator will damage back the player and push player
            playerHealth.PlayerTakeDamage(mowerStat.FieldDamage);
            PushPlayer();
            TempCamScript.Instance.ShakeCamera(5f, .0000001f);
            StartCoroutine(mowerStat.ShieldReflect(shieldMat));
        }
    }

    // Only for damaging force field
    public void DamagingForceField(float playerDmg)
    {
        fieldBarHealth.gameObject.SetActive(true);

        fieldHP -= playerDmg;
        fieldBarHealth.UpdateHealthBar(fieldHP, mowerStat.FieldMaxHP);
        PushPlayer();

        DamagePopUp.Create(PopUpPos(backside), playerDmg, mowerStat.fieldDmgColor, 22);

        if (fieldHP <= 0) { Destroyed(); }       
    }

    private void PushPlayer()
    {
        playerMovement.PlayerRigid2d.AddExplosionForce(200000f, backside.position, 50f, 10f);
    }

    public override void ApplyBleeding(float damage, float duration, int ticks, Collider selfCol)
    {
        // Don't bleed the generator
        if (isGeneratorHit) { return; }

        // Don't bleed Mower's front side
        if (!isBackSideHit) { return; }

        // Only bleed when Inactive or Destroyed
        if (IsInactive || IsDestroyed)
        {
            base.ApplyBleeding(damage, duration, ticks, selfCol);
        }
    }
    #endregion
   

    #region Mower attack 
    private void MowerCheck()
    {
        // Don't check if dead or attacking or just new born
        if (currentHP <= 0 || isAttacking || isNewBorn) { return; }

        var horizontalDetect = Physics.Raycast(frontDetection.position, transform.right, 3f, LayerMask.GetMask("Player"));
        var verticalDetect = Physics.Raycast(frontDetection.position + (transform.right * 3f), Vector3.down, 3f, LayerMask.GetMask("Player"));
        
        // If player is not in front of Mower's teeth, don't attack
        if (!horizontalDetect && !verticalDetect) { return; }

        // Then attack player if not generating
        if (!IsGenerating)
        {
            playerMovement.isPlayerBlock = false;   // Cancel block if player is

            MowerAttack();
        }
    }
    
    private void MowerAttack()
    {
        isAttacking = true;
        playerMovement.PlayerRigid2d.velocity = Vector3.zero;       
        
        KnockPlayerDown();

        if (dmgCoroutine != null)
        {
            StopCoroutine(dmgCoroutine);           
        }
        StartCoroutine("Attacking");
    }

    private IEnumerator Attacking()
    {
        rb.useGravity = false;
        boxCollier.isTrigger = true;
        capsuleCollider.isTrigger = true;
        speed = stat.RunningSpeed / 1.8f;

        dmgCoroutine = StartCoroutine(ApplyDamage(6, stat.Damage));

        yield return new WaitForSeconds(6f);

        ReturnPhysics();
    }

    // Deal damage
    private IEnumerator ApplyDamage(int damageCount, int damageAmount)
    {
        int currentCount = 0;

        while (currentCount < damageCount)
        {
            playerHealth.PlayerTakeDamage(damageAmount);
            TempCamScript.Instance.ShakeCamera(5f, .0000001f);
            yield return new WaitForSeconds(1f);
            currentCount++;
        }
    }
    #endregion

    protected override void PlayerUp()
    {
        base.PlayerUp();

        // Stop dealing damage
        StopCoroutine("Attacking");
        if (dmgCoroutine != null) { StopCoroutine(dmgCoroutine); }

        Invoke("ReturnPhysics", 0.5f);

        // If not Mower, return 
        if (enemyID != GetInstanceID()) { return; }
        
        // Push player up 
        playerMovement.PlayerRigid2d.velocity = Vector3.up * 50;       
    }

    protected override void PlayerDown()
    {
        base.PlayerDown();
        
        if (Physics.Raycast(frontDetection.position + (Vector3.down * 5), transform.right, 100f, LayerMask.GetMask("Player"))) { return; }

        ChangeDirection();
        if (!IsGenerating && !IsActive) { animator.Play("Turning"); }
    }

    void ReturnPhysics()
    {
        boxCollier.isTrigger = false;
        capsuleCollider.isTrigger = false;
        rb.useGravity = true;
        speed = stat.RunningSpeed;
        isAttacking = false;
    }

    protected override void Movement()
    {
        base.Movement();

        animator.SetFloat("MovementSpeed", speed / stat.RunningSpeed);       
    }

    protected override void ChangeDirection()
    {
        base.ChangeDirection();
        fieldBarHealth.ScaleLeftUI(rb);       
    }

    protected override Vector3 PopUpPos(Transform trans)
    {
        var pos = new Vector3(trans.position.x + UnityEngine.Random.Range(-4f, 4f), trans.position.y + 8, -10);

        return pos;
    }

    #region State Functions
    public void ChangeToInactive() => currentState = ForceFieldState.Inactive;
    public void ChangeToGenerating() => currentState = ForceFieldState.Generating;
    public void ChangeToActive() => currentState = ForceFieldState.Active;
    public void ChangeToDestroyed() => currentState = ForceFieldState.Destroyed;

    public bool IsInactive => currentState == ForceFieldState.Inactive;
    public bool IsGenerating => currentState == ForceFieldState.Generating;
    public bool IsActive => currentState == ForceFieldState.Active;
    public bool IsDestroyed => currentState == ForceFieldState.Destroyed;
    #endregion
}
