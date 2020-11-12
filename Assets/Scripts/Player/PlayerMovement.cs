using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public float PlayerSpeed; // for move left and right
    public float PlayerJumpPow; // for jump
    public float MidAirSpeed; // for move left and right while mid air
    [SerializeField] private LayerMask groundlayermask, walllayermask, ladderlayermask;

    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerAttackTrigger playerAttack;
    [HideInInspector] public Animator PlayerAnimator;
    [HideInInspector] public Rigidbody PlayerRigid2d;

    float inputH, inputV;

    public bool isGrounded;

    public bool FaceRight = true;
    public Transform PlayerFrontPos, PlayerBehindPos;
    public Transform PlayerUnderPos, PlayerAbovePos;
    public float CheckRadius;

    public float PlayerClimbSpeed;

    public bool isPlayerBlock = false;

    [HideInInspector] public bool isKnockDown = false;
    [HideInInspector] public int getUpCount = 0;
    [HideInInspector] public int getUpNeed;

    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;

    public Animator animator;

    public bool isCollideWall;

    public bool isGrabWall = false;
    public bool isJumping = false;

    TextMeshProUGUI climbPrompt;
    public bool isCollidePlatform;
    public bool canLedgeClimb = true;
    Vector3 playerClimbPos;
    Vector3 destination;
    float ledgePosX;
    float ledgePosY;



    //Start Hash ID 
    [HideInInspector]
    public int isJumping_animBool,
        isGrounded_animBool,
        isGrabWall_animBool,
        knockedDown_animBool,
        blocking_animBool,
        isRunning_animBool,
        isOnTop_animBool;

    [HideInInspector]
    public int inputH_animFloat,
        inputV_animFloat,
        vSpeed_animafloat;
    //end Hash ID

    public event Action OnBounceUp;

    void Awake()
    {
        playerHealth = transform.GetComponent<PlayerHealth>();
        playerAttack = transform.GetComponent<PlayerAttackTrigger>();
        PlayerRigid2d = transform.GetComponent<Rigidbody>();
        //PlayerAnimator = transform.GetComponent<Animator>();
        boxCollider = transform.GetComponent<BoxCollider>();
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
        PlayerRigid2d.centerOfMass = Vector3.zero;
        climbPrompt = GameObject.FindGameObjectWithTag("PlayerUI").transform.Find("climbPrompt").GetComponent<TextMeshProUGUI>();


        //HashID animator parameters for performances
        isJumping_animBool = Animator.StringToHash("isJumping");
        isGrounded_animBool = Animator.StringToHash("isGrounded");
        isGrabWall_animBool = Animator.StringToHash("isGrabWall");
        blocking_animBool = Animator.StringToHash("Blocking");
        knockedDown_animBool = Animator.StringToHash("KnockedDown");
        isOnTop_animBool = Animator.StringToHash("isOnTop");
        isRunning_animBool = Animator.StringToHash("IsRunning");
        inputH_animFloat = Animator.StringToHash("inputH");
        inputV_animFloat = Animator.StringToHash("inputV");
        vSpeed_animafloat = Animator.StringToHash("vSpeed");
    }

    void Update()
    {
        SetAnimatorPara();

        CheckKnockDown();
        CheckGrounded();
        CheckCollideWall();
        CheckOnTop();
        CheckGrabWall();
        CheckCollidePlatform();

        if (isGrounded)
        {

            isJumping = false;
        }
        else
        {
            isJumping = true;
            isPlayerBlock = false;
        }

        if (!isKnockDown)
        {
            CheckPlayerBlock();
            InputHorrizontal(); // Included player flip
            InputVertical();
        }

        if (!isKnockDown && !CheckColliderAbove())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerJump();
            }
        }




        if (isCollideWall && !isGrabWall && !isKnockDown)
        {
            climbPrompt.gameObject.SetActive(true);
            climbPrompt.SetText("Press W to Grab the Vine");
        }
        else
        {
            climbPrompt.gameObject.SetActive(false);
        }

        if (!isCollidePlatform && !CheckOnTop())
        {
            canLedgeClimb = false;
        }
        if (canLedgeClimb == true)
        {
            playerClimbPos = transform.position;
        }


        if (isGrabWall == true)
        {
            PlayerRigid2d.useGravity = false;
            PlayerRigid2d.rotation = Quaternion.Euler(5, 90, 0);
            climbPrompt.gameObject.SetActive(true);
            climbPrompt.SetText("Press W or S to Move Up or Down");
            PlayerClimbWal();
        }
        else
        {
            PlayerRigid2d.useGravity = true;
            PlayerRigid2d.rotation = Quaternion.Euler(0, 90, 0);

        }

        if (isGrabWall && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            isPlayerBlock = false;
            isGrabWall = false;
            PlayerRigid2d.velocity = Vector3.zero;
            PlayerRigid2d.velocity = (Vector3.up * PlayerJumpPow);

        }




    }

    void FixedUpdate()
    {
        if (!isPlayerBlock && !isKnockDown)// when player is not blocking they can move
        {
            PlayerMove();
        }
    }

    void CheckGrounded()
    {
        Vector3 boxColliderCheckPoint = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y + boxCollider.bounds.size.y, boxCollider.bounds.center.z);
        float distance = 1f;
        Debug.DrawRay(boxCollider.bounds.center, Vector3.down * distance, Color.red);

        if (Physics.BoxCast(boxColliderCheckPoint, boxCollider.bounds.size / 2, Vector3.down, transform.rotation, distance, groundlayermask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    void CheckPlayerBlock()
    {
        if (!isGrounded) { return; }    // Don't check if player is on air     

        // check if player click right mouse 
        if (Input.GetMouseButton(1))
        {
            isPlayerBlock = true;
        }
        else
        {
            isPlayerBlock = false;
        }

    }

    void InputHorrizontal()
    {

        inputH = Input.GetAxisRaw("Horizontal");// Note if dont get raw axis it feels like splippery

        FlipPlayer();

        if (inputH != 0 && !isPlayerBlock)
        {
            //animator.SetBool("IsRunning", true);
            animator.SetBool(isRunning_animBool, true);
        }
        else
        {
            //animator.SetBool("IsRunning", false);
            animator.SetBool(isRunning_animBool, false);
        }
    }

    bool CheckColliderAbove()
    { 
        float distance = 2f;
        Vector3 checkPnts = new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.center.z);
        //Debug.DrawRay(checkPnts, Vector3.up * distance, Color.yellow);
        return (Physics.Raycast(checkPnts, Vector3.up, distance, walllayermask) || Physics.Raycast(checkPnts, Vector3.up, distance, groundlayermask));

    }
   


    void InputVertical()
    {
        inputV = Input.GetAxisRaw("Vertical");
    }
      
    // check collide with wall  
    void CheckCollideWall()
    {
            
        float distance = 2f;
        if (FaceRight)
        {
           isCollideWall=Physics.Raycast(capsuleCollider.bounds.center, Vector3.right, distance, walllayermask);
            //Debug.DrawRay(capsuleCollider.bounds.center, Vector3.right * distance, Color.yellow);
        }
        else
        {
            isCollideWall=Physics.Raycast(capsuleCollider.bounds.center, Vector3.left, distance, walllayermask);
           //Debug.DrawRay(capsuleCollider.bounds.center, Vector3.left * distance, Color.yellow);

        }
    }

    bool CheckOnTop()
    {
       
        float distance = 2f;
        if (FaceRight)
        {
            //Debug.DrawRay(PlayerAbovePos.position, Vector3.right * distance, Color.yellow);
            return (!Physics.Raycast(PlayerAbovePos.position , Vector3.right, distance, walllayermask)&& Physics.Raycast(capsuleCollider.bounds.center, Vector3.right, distance, walllayermask));


        }
        else
        {
            //Debug.DrawRay(PlayerAbovePos.position, Vector3.left * distance, Color.yellow);
            return (!Physics.Raycast(PlayerAbovePos.position, Vector3.left, distance, walllayermask) && Physics.Raycast(capsuleCollider.bounds.center, Vector3.left, distance, walllayermask));

        }

    }

    void CheckCollidePlatform()
    {
        Vector3 checkPntsAbove = new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.center.z);
        Vector3 checkPntsBellow = new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.bounds.center.z);
        float distance = 3f;
        if (FaceRight)
        {
            isCollidePlatform = Physics.CapsuleCast(checkPntsBellow,checkPntsAbove,capsuleCollider.radius, Vector3.right, distance, groundlayermask);
           
        }
        else
        {
            isCollidePlatform = Physics.CapsuleCast(checkPntsBellow, checkPntsAbove, capsuleCollider.radius, Vector3.left, distance, groundlayermask);
          

        }
    }


    void CheckGrabWall()
    {
        if (isCollideWall && !CheckColliderAbove())
        {
            if (isGrabWall == false)
            {
                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.S))
                {
                    isGrabWall = true;
                    isJumping = false;
                }
            }
           
        }
        if (!isCollideWall)
        {
            isGrabWall = false;
        }

    }

    //player climb on wall
    void PlayerClimbWal()
    {
        float xSpeed = 2f;
        float offsetX = 3f;
        float offsetY = 6f;
        if (inputV == 0)
        {
            PlayerRigid2d.velocity = Vector3.zero;
        }
        else
        {


            if (!CheckColliderAbove())
            {

                if (!CheckOnTop())
                {
                    if (inputV != 0)
                    {
                        PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.up * inputV * PlayerClimbSpeed * Time.deltaTime + Vector3.right * xSpeed * transform.localScale.z * Time.deltaTime);
                    }

                    if (isGrounded && inputV < 0)
                    {
                        isGrabWall = false;
                    }

                }
                else
                {
                    if (!isCollidePlatform)
                    {
                        if (inputV > 0)
                        {
                            PlayerRigid2d.velocity = new Vector3(0, 0, 0);
                        }
                        else if (inputV < 0)
                        {
                            PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.up * inputV * PlayerClimbSpeed * Time.deltaTime + Vector3.right * xSpeed * transform.localScale.z * Time.deltaTime);

                        }
                    }
                    else
                    {
                        canLedgeClimb = true;
                        if (FaceRight)
                        {
                            ledgePosX = transform.position.x + offsetX;
                            ledgePosY = transform.position.y + offsetY;

                        }
                        else
                        {
                            ledgePosX = transform.position.x - offsetX;
                            ledgePosY = transform.position.y + offsetY;
                        }
                        destination = new Vector3(ledgePosX, ledgePosY, 0);

                        if (Input.GetKey(KeyCode.W) || inputV > 0)
                        {
                            transform.position = destination;

                        }
                        else if (inputV < 0)
                        {
                            PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.up * inputV * PlayerClimbSpeed * Time.deltaTime + Vector3.right * xSpeed * transform.localScale.z * Time.deltaTime);

                        }

                    }

                }
                print(PlayerRigid2d.velocity);
            }

            else
            {
                if (inputV < 0)
                {
                    PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.up * inputV * PlayerClimbSpeed * Time.deltaTime + Vector3.right * xSpeed * transform.localScale.z * Time.deltaTime);
                }
                else if (inputV > 0)
                {
                    PlayerRigid2d.velocity = Vector3.zero;
                }


            }
        }
    }


 


    private void PlayerMove()
    {
        if (!isCollideWall)
        {
            if (isGrounded) // if player is move on the ground with normal speed
            {
                PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.right * inputH * PlayerSpeed * Time.deltaTime);
            }
            else if (!isGrounded)// if player is jumping we can have them some control
            {
                PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.right * inputH * MidAirSpeed * Time.deltaTime);
            }
        }
        if (isCollideWall)
        {
            if (isGrounded)
            {
                if (inputH * transform.localScale.z == -1)
                {
                    PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.right * inputH * PlayerSpeed * Time.deltaTime);
                }
            }
            else if ((!isGrounded))
            {
                if (inputH * transform.localScale.z == -1)
                {
                    PlayerRigid2d.MovePosition((Vector3)transform.position + Vector3.right * inputH * MidAirSpeed * Time.deltaTime);
                }
            }
        }
    }

    

    private void PlayerJump() 
                              // side note could handle jump power by * with the character height. at the moment the vector in middle of the character so 7pixel long
    {       
        if (IsBouncingBack()) { return; }

        if (isGrounded)
        {
            animator.SetTrigger("JumpStart");
            PlayerRigid2d.velocity = Vector3.up * PlayerJumpPow;
            isJumping = true;
            isPlayerBlock = false;

        }
  
    }
    

    private void FlipPlayer()
    {
        if (inputH <0  &&  FaceRight == true) { Flip(); }
        else if (inputH > 0 && FaceRight == false) { Flip(); }    
    }

    private void Flip()
    {
        FaceRight = !FaceRight;
        Vector3 Scale= transform.localScale;
        Scale.z *= -1;
        //Scale.x *= -1;
        transform.localScale = Scale;
    }
   
    void CheckKnockDown()
    {
        if (isKnockDown)
        {
            isGrabWall = false;
           
            animator.SetBool(knockedDown_animBool, true);

            playerHealth.spaceTextGrid.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                getUpCount++;
                playerHealth.SetCurrentSpace(getUpCount);

                if (getUpCount >= getUpNeed)
                {
                    PlayerBounceUp();
                }
            }
        }
    }

    public void PlayerBounceUp()
    {       
        isKnockDown = false;

        animator.SetTrigger("BounceUp");        

        getUpCount = 0;
        getUpNeed = 0;
        playerHealth.SetCurrentSpace(getUpCount);

        playerHealth.spaceTextGrid.gameObject.SetActive(false);

        OnBounceUp();
    }

    public bool IsBouncingBack()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("KnockedDown") || animator.GetCurrentAnimatorStateInfo(0).IsTag("BounceBack");
    }

    void SetAnimatorPara()
    {       
        animator.SetFloat(inputH_animFloat, Mathf.Abs(inputH));
        animator.SetFloat(inputV_animFloat, Mathf.Abs(inputV));
        animator.SetFloat(vSpeed_animafloat, PlayerRigid2d.velocity.y);

        animator.SetBool(isJumping_animBool, isJumping);
        animator.SetBool(isGrounded_animBool, isGrounded);
        animator.SetBool(blocking_animBool, isPlayerBlock);
        animator.SetBool(isGrabWall_animBool, isGrabWall);
        animator.SetBool(knockedDown_animBool, isKnockDown);
        animator.SetBool(isOnTop_animBool, CheckOnTop());
    }

    void PlayRunningSound()
    {
        soundManager.PlaySound(soundManager.Sound.playerMove, transform.position);
    }
}

