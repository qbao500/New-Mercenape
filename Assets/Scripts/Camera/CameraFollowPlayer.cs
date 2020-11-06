using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Written by Ossi Uusitalo
    public bool isZoomedIn;
    public float zoomIn, zoomOut, zoomNormal, zoomNum; // ZoomNum is for toggling between ZoomIn and ZoomOut.
    Camera cam;
    Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        zoomNum = 1;
        isZoomedIn = true;
        cam = transform.GetComponent<Camera>();
        zoomNormal = cam.orthographicSize;

        //These are the values that the camera's Orthopode-whatever- size takes to implement the level of zoom.
        zoomIn = zoomNormal;
        zoomOut = zoomNormal + 50;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position + new Vector3(0,5,-4);
    }

    private void Update()
    {
        

        if(isZoomedIn)
        {
            cam.orthographicSize = zoomIn;
            zoomNum = 1;
        } else
        {
            cam.orthographicSize = zoomOut;
            zoomNum = 1;
        }

        if (Time.timeScale == 1)
        {
            //THe mouse wheel increases or decreases the ZoomNum variable to decide whether the camera is zoomed in or out.
            zoomNum += Input.GetAxis("Mouse ScrollWheel");
            if (zoomNum < 1 && isZoomedIn)
            {
                isZoomedIn = false;

            }
            else if (zoomNum > 1 && !isZoomedIn)
            {
                isZoomedIn = true;

            }
        }
        
    }
}
