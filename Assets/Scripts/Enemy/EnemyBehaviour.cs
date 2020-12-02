using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Bao: main enemy behaviour, parent of ShredBehaviour and MowerBehaviour
// Edited by Arttu Paldán on 23.10.2020: Added an virtual function for player to add bleed on enemies. 
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] protected EnemyStatsSO stat;

    protected bool isNewBorn;

    protected GameObject healthBarUI;
    protected EnemyHealthBar barHealth;

    protected float speed;
    [SerializeField] protected float currentHP;
    protected static int enemyID;

    protected float weaponBleedDamage, weaponBleedDuration;
    protected int bleedTicks, currentBleedTicks;

    private Vector3 enemyRotation;
    protected Rigidbody rb;
    protected BoxCollider boxCollier;
    [SerializeField] protected Transform frontDetection;
    protected bool groundInfo, wallInfo;

    protected GameObject player;
    protected PlayerHealth playerHealth;
    protected PlayerMovement playerMovement;

    private EnemyLootDrop enemyLoot;

    protected virtual void Awake()
    {       
        enemyRotation = transform.rotation.eulerAngles;

        barHealth = transform.GetChild(1).GetComponent<EnemyHealthBar>();
            
        boxCollier = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;

        enemyLoot = GetComponent<EnemyLootDrop>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();

        // Subscribe to the OnHitEnemy event in PlayerAttack
        playerMovement.playerAttack.OnHitEnemy += EnemyGetHit;
        playerMovement.playerAttack.OnBleedEnemy += ApplyBleeding;
        playerMovement.playerAttack.StaggerEnemy += WeaponStagger;
        playerMovement.OnBounceUp += PlayerUp;
    }

    protected virtual void OnEnable()
    {
        DeactivePhysics();
        Invoke("ActivePhysics", 1f);

        rb.useGravity = true;
        boxCollier.enabled = true;
        boxCollier.isTrigger = false;

        speed = stat.RunningSpeed;
        currentHP = stat.MaxHP;
        barHealth.UpdateHealthBar(currentHP, stat.MaxHP);
        barHealth.ScaleRightUI(rb);
    }

    protected void FixedUpdate() => Movement();

    protected bool IsFacingRight => (int)transform.rotation.eulerAngles.y == 0;

    protected virtual void Movement()
    {
        if (currentHP <= 0) { return; } // Don't move if dead

        // Check direction facing and adjust to velocity according to that
        if (IsFacingRight)
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
        }

        if (isNewBorn) { return; }  // Don't check if new born

        groundInfo = Physics.Raycast(frontDetection.position, Vector3.down, 15f, LayerMask.GetMask("Ground"));
        wallInfo = Physics.Raycast(frontDetection.position, transform.right, 4.25f, LayerMask.GetMask("Wall", "Border"));
       
        if (!groundInfo || wallInfo)
        {
            enemyRotation += new Vector3(0, -(Mathf.Sign(rb.velocity.x)) * 180, 0);
            transform.rotation = Quaternion.Euler(0, enemyRotation.y, 0);
            barHealth.ScaleRightUI(rb);
        }        
    }

    #region Take damage and bleed
    // Check to see which type of enemy get hit
    protected virtual void EnemyGetHit(bool isMowerBackSide, bool isMowerGenerator, Collider selfCol, float playerDmg)
    {
        if (!IsSelf(selfCol)) { return; }
        
        TakeDamage(playerDmg);  // Then deal damage to the correct enemy
    }

    // Take damage from player
    public virtual void TakeDamage(float playerDamage)
    {      
        currentHP -= playerDamage;
        barHealth.UpdateHealthBar(currentHP, stat.MaxHP);

        DamagePopUp.Create(PopUpPos(transform), playerDamage, Color.clear, 15); 

        // If dead
        if (currentHP <= 0)
        {
            StartCoroutine("EnemyDeath");
        }
    }

    // Take bleed damage from player's weapon
    public void TakeBleedDammage(float bleedDmg)
    {
        currentHP -= bleedDmg;
        barHealth.UpdateHealthBar(currentHP, stat.MaxHP);

        DamagePopUp.Create(PopUpPos(transform), bleedDmg, Color.magenta, 10);

        // If dead
        if (currentHP <= 0)
        {          
            StartCoroutine("EnemyDeath");
        }
    }

    public virtual void ApplyBleeding(float damage, float duration, int ticks, Collider selfCol)
    {
        if (!IsSelf(selfCol)) { return; }

        weaponBleedDamage = damage;
        weaponBleedDuration = duration;
        bleedTicks = ticks;
        currentBleedTicks = 1;

        // Stop stacking bleed before begin new bleed
        StopCoroutine(BleedTick());
        StartCoroutine(BleedTick());
    }

    IEnumerator BleedTick()
    {
        yield return new WaitForSeconds(.2f);

        while (currentBleedTicks <= bleedTicks)
        {
            TakeBleedDammage(weaponBleedDamage);
            yield return new WaitForSeconds(weaponBleedDuration);
            currentBleedTicks++;
        }
    }

    // Mainly for the Shred, so gonna leave this as an empty virtual. 
    public virtual void WeaponStagger(float duration, Collider selfCol)
    {
    }
    #endregion

    protected void KnockPlayerDown()
    {
        // Don't knock player down again when bouncing back
        if (playerMovement.animator.GetCurrentAnimatorStateInfo(0).IsTag("BounceBack")) { return; }

        if (stat.spaceToGetUp >= playerMovement.getUpNeed)
        {
            playerMovement.getUpNeed = stat.spaceToGetUp;
            playerHealth.SetNeededSpace(stat.spaceToGetUp);
            enemyID = GetInstanceID();
        }

        // If player is already knocked down, don't do anything
        if (playerMovement.isKnockDown) { return; }

        playerMovement.animator.ResetTrigger("Attack");
        playerMovement.animator.SetTrigger("KnockDown");
        playerMovement.isKnockDown = true;
        playerHealth.spaceTextGrid.gameObject.SetActive(true);

        playerMovement.getUpCount = 0;
        playerHealth.SetCurrentSpace(playerMovement.getUpCount);        
    }

    protected virtual void PlayerUp()
    {
        playerHealth.spaceTextGrid.gameObject.SetActive(false);
    }
    
    protected IEnumerator EnemyDeath()
    {        
        speed = 0;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        boxCollier.enabled = false;

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);

        enemyLoot.GiveLoot();
    }

    protected void ActivePhysics()
    {
        isNewBorn = false;
        Physics.IgnoreLayerCollision(10, 12, false);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    protected void DeactivePhysics()
    {
        isNewBorn = true;
        Physics.IgnoreLayerCollision(10, 12, true);
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    // Check to make sure only the enemy get hit is called, not every enemy
    protected bool IsSelf(Collider selfCol) => selfCol.gameObject.transform.root.gameObject.GetInstanceID() == this.gameObject.GetInstanceID();

    protected virtual Vector3 PopUpPos(Transform trans) => Vector3.zero;

}