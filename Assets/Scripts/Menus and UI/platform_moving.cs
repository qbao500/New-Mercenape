using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_moving : MonoBehaviour
{
    public Vector3 Goal, startPos;
    public GameObject goalObject;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        //Set the 2 vectors between which the platofrm moves.
        startPos = transform.position;
        Goal = goalObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
          transform.position = Vector2.MoveTowards(transform.position, Goal, Time.deltaTime * speed);
          if(transform.position == Goal)
          {
               Debug.Log("Destination reached");
              if(Goal == goalObject.transform.position)
              {
                  Goal = startPos;
              } else if(Goal == startPos)
              {
                  Goal = goalObject.transform.position;
              }
          }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
