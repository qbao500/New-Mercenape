using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    //Written by Ossi Uusitalo
    public GameObject mainCam;
    CinemachineVirtualCamera cineVirtual;
    CinemachineFramingTransposer cineFrame;
    public float bottom, zoomIn; // You need to manually type the Ortographic size (ZoomIn) and ScreenY(bottom) from the editor to set the minimum level of zoom.
    public float top, zoomOut;

    private void Start()
    {
        cineVirtual = mainCam.GetComponent<CinemachineVirtualCamera>();
        if(cineVirtual != null)
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
            }
        }
    }

    private void Update()
    {
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

