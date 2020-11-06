using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourNotUsing : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
 
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float stoppingDistance = 1.5f;

    private Transform rayPos;
    private float rayDistance = 4f;

    private Vector2 destination;

    protected EnemyState currentState;
    protected Rigidbody2D rb;
    private EnemyBehaviour enemyStat;

    [SerializeField] protected GameObject player;
    protected Rigidbody2D playerRigid;
    protected PlayerHealth playerHealth;
    protected PlayerMovement playerMovement;
    protected PlayerAttackTrigger playerAttack;
    protected SpriteRenderer playerRenderer;

    [HideInInspector] public int enemyDamage;

    [SerializeField] protected int forcePower; 
    [SerializeField] private float delayTimeBetweenAttack;
    [SerializeField] private float timeBetweenAttack;
    
    protected bool isPlayerGetKnocked = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigid = player.GetComponent<Rigidbody2D>();
            playerHealth = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();
            playerAttack = player.GetComponent<PlayerAttackTrigger>();
            playerRenderer = player.GetComponent<SpriteRenderer>();
        }
        
        rayPos = gameObject.transform.GetChild(0).GetComponent<Transform>();

        rb = this.GetComponent<Rigidbody2D>();
        enemyStat = this.GetComponent<EnemyBehaviour>();
        //enemyDamage = enemyStat.damage;

        timeBetweenAttack = delayTimeBetweenAttack;
    }

    private void Update()
    {
        Debug.DrawRay(rayPos.position, transform.right * rayDistance);

        // Cool down between attacks
        timeBetweenAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerRigid.AddForce(new Vector2(150, 3), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(800, 100), ForceMode2D.Impulse);
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                {
                    speed = 2;

                    if (NeedDestination())
                    {
                        GetRandomDestination();
                    }
                    
                    // Go to destination with speed
                    transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);

                    // When get to ray cast range
                    if (ShouldChase())
                    {
                        currentState = EnemyState.Chase;
                    }

                    break;
                }
            case EnemyState.Chase:
                {
                    if (player == null)
                    {
                        Debug.Log("There's NO player");
                        currentState = EnemyState.Patrol;
                        return;
                    }

                    speed = 4;
                    
                    // Chase player
                    GetPlayerDestination();
                    transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);                  

                    // When get to attack range
                    if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
                    {
                        currentState = EnemyState.Attack;
                    }

                    break;
                }
            case EnemyState.Attack:
                {
                    if (player == null)
                    {
                        Debug.Log("There's NO player");
                        currentState = EnemyState.Patrol;
                        return;
                    }

                    EnemyAttack();                    

                    // When out of attack range
                    if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
                    {
                        currentState = EnemyState.Chase;                       
                    }
                                     
                    break;
                }
        }
    }

    // Check if enemy need a new destination
    private bool NeedDestination()
    {
        if (destination == Vector2.zero)
        {
            return true;
        }

        var distance = Vector2.Distance((Vector2)transform.position, destination);
        if (distance <= stoppingDistance)
        {           
            return true;
        }

        return false;
    }

    // Get a new random destination when needed
    private void GetRandomDestination()
    {
        destination = new Vector2(transform.position.x + Random.Range(-8f, 8f), transform.position.y);
        
        // Facing right direction
        if ((destination.x - transform.position.x) > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {           
            transform.eulerAngles = new Vector2(0, -180);
        }
    }

    // Chase if player is in ray sight
    private bool ShouldChase()
    {
        RaycastHit2D ray = Physics2D.Raycast(rayPos.position, transform.right, rayDistance);

        if (ray.collider)
        {         
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player detected!!!");
                return true;
            }
        }
        
         return false;    
    }

    // Get player position to chase
    private void GetPlayerDestination()
    {
        // Rage mode
        transform.localScale = new Vector2(0.3f, 0.3f);       
        destination = new Vector2(player.transform.position.x, transform.position.y);      

        // Facing right direction
        if ((destination.x - transform.position.x) > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, -180);
        }
    }

    protected virtual void EnemyAttack()
    {
        Vector2 force = ((Vector2)transform.right * forcePower) + new Vector2(0, 30);
        
        if (timeBetweenAttack <= 0)
        {
            // Jump to player
            rb.AddForce(force, ForceMode2D.Impulse);
                        
            timeBetweenAttack = delayTimeBetweenAttack;           
         }   
    }
}
