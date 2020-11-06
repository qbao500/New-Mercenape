using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VineHingeAnchor : MonoBehaviour
{

    Rigidbody2D rb;
    GameObject player;
    public bool isSwing = false;
    public float swingSpeed;
    public UnityEvent VineEvent;
    float h, v;

    bool isFlying=false;
    BoxCollider2D box2D;
    [SerializeField] LayerMask playerlayermask;

    //limit how many times player can climb up the vine


    Transform upper, lower;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        upper = transform.Find("UpperLimit");
        lower = transform.Find("LowerLimit");


        if (VineEvent == null)
        {
            VineEvent = new UnityEvent();
        }
       VineEvent.AddListener(UngrabVine);

    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxisRaw("Vertical");



        if (isSwing)
        {
            player.transform.parent = this.transform;
            player.transform.rotation =this.transform.rotation;

           player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;// no more gravity
           //playerHinge.connectedBody

          
            rb.AddForce(new Vector2(h * swingSpeed * Time.deltaTime, 0), ForceMode2D.Force);

            if (v != 0)
            {
                if(v > 0 && (player.transform.position.y < upper.position.y)) //if player dont cross the upperlimit then he can climb more
                {
                    player.transform.Translate(Vector2.up * v * 10 * Time.deltaTime);
                } 
                
                if ( v < 0 && (player.transform.position.y > lower.position.y))//if player above lowerlimit and climb down
                {
                    player.transform.Translate(Vector2.up * v * 10 * Time.deltaTime);
                }

                if (v<0 && (player.transform.position.y <= lower.position.y)) //if player bellow lowerlimit he drop down
                {
                    isSwing = false;
                    player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    player.transform.parent = null;
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

              /* if (v > 0 &&  climbTimes <= climbLimit )
                {
                    player.transform.Translate(Vector2.up * v * 10 * Time.deltaTime);
                    climbTimes++;
                }
                if(v<0&& climbTimes > 0)
                {
                    player.transform.Translate(Vector2.up * v * 10 * Time.deltaTime);
                    climbTimes--;

                }
                if(v < 0 && climbTimes == 0)
                {
                    isSwing = false;
                    player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    player.transform.parent = null;
                    player.transform.rotation = Quaternion.Euler(0, 110, 0);
                    
                }*/ 


            }

        }
     

        if (Input.GetKeyDown(KeyCode.Space) && isSwing)
        {

            VineEvent.Invoke();
        }

        

    }


    



    private void FixedUpdate()
    {
        
        // note need 2 add time 
        if(isFlying == true) {
            if(player.GetComponent<PlayerMovement>().FaceRight) 
            { 
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(10000f, 2000f)*Time.deltaTime, ForceMode2D.Impulse);
            }else
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10000f, 2000f) * Time.deltaTime, ForceMode2D.Impulse);
            }
            Invoke("func", 0.5f);
        }
    }

    void func()
    {
        isFlying = false;
   

    }

    void UngrabVine()
    {
        isSwing = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.transform.parent = null;

        isFlying = true;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        


    }


    
    bool checkIsGrabVine()
    {
        return Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f,Vector2.down, box2D.bounds.extents.y + 0.1f, playerlayermask); 
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                this.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 0));

                isSwing = true;
            }
        }
    }
    


}

