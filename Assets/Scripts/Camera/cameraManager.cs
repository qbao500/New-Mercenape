using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    //Written by Ossi Uusitalo
    public GameObject mainCam, player;
    CinemachineVirtualCamera cineVirtual;
    CinemachineFramingTransposer cineFrame;
    public float bottom, zoomIn; // You need to manually type the Ortographic size (ZoomIn) and ScreenY(bottom) from the editor to set the minimum level of zoom.
    public float top, zoomOut;
    public bool stopY = false;
    public float maxHeight, minHeight; //This will the parameters on which the camera can move on the Y axis.

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        if (player != null)
        {
            cineVirtual = mainCam.GetComponent<CinemachineVirtualCamera>();
            cineVirtual.m_Follow = player.transform;
            if (cineVirtual != null)
            {
                Debug.Log("CineVirtual found");

                cineFrame = cineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
                if (cineFrame != null)
                {
                    Debug.Log("cineFrame found");
                    //Set the maximum parameters for the zoom out
                    top = cineFrame.m_ScreenY;
                    zoomOut = cineVirtual.m_Lens.OrthographicSize;

                    //Then we set the camera on the max zoom in
                    cineVirtual.m_Lens.OrthographicSize = zoomIn;
                    cineFrame.m_ScreenY = bottom;
                    minHeight = mainCam.transform.position.y;
                }
            }
        }
    }

    private void Update()
    {
        cineFrame.m_BiasY = -0.5f;
        //This prevents the camera from going further up to peek over the actual level
        if(stopY)
        {
            cineFrame.m_DeadZoneHeight = 2f;
            cineFrame.m_SoftZoneHeight = cineFrame.m_DeadZoneHeight;
            cineFrame.m_DeadZoneWidth = cineFrame.m_SoftZoneWidth + 0.05f;

        } else
        {
            cineFrame.m_DeadZoneWidth = 0f;
            cineFrame.m_DeadZoneHeight = 0;
            cineFrame.m_SoftZoneHeight = 0.14f;
            cineFrame.m_SoftZoneWidth = 0.05f;

        }
        //The triggers for stopY:
        if(player.transform.position.y > maxHeight || cineVirtual.m_Lens.OrthographicSize >= zoomOut * 0.8f)
        {
            stopY = true;
        } else
        {
            stopY = false;
        }


        //The Screen Y axis and Ortographic size are controlled by the mouse wheel
        cineFrame.m_ScreenY -= Input.mouseScrollDelta.y;
        cineVirtual.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y;

        //Parameters
        if (cineFrame.m_ScreenY > top) 
        {
            cineFrame.m_ScreenY = top;
        }
        if(cineFrame.m_ScreenY < bottom)
        {
            cineFrame.m_ScreenY = bottom;
        }

        
        if(cineVirtual.m_Lens.OrthographicSize > zoomOut)
        {
            cineVirtual.m_Lens.OrthographicSize = zoomOut;
        }
        if (cineVirtual.m_Lens.OrthographicSize < zoomIn)
        {
            cineVirtual.m_Lens.OrthographicSize = zoomIn;
        }

    }

}

