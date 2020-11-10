using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TempCamScript : MonoBehaviour
{
    public CinemachineVirtualCamera cineVirtual;
    private CinemachineFramingTransposer framingTransposer;

    private void Awake()
    {
        framingTransposer = cineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        cineVirtual.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y;
        cineVirtual.m_Lens.OrthographicSize = Mathf.Clamp(cineVirtual.m_Lens.OrthographicSize, 5, 29);

        framingTransposer.m_TrackedObjectOffset.y += Input.mouseScrollDelta.y * 2;
        framingTransposer.m_TrackedObjectOffset.y = Mathf.Clamp(framingTransposer.m_TrackedObjectOffset.y, -29, 3);       
    }
}
