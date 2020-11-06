using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TempCamScript : MonoBehaviour
{
    public CinemachineVirtualCamera cineVirtual;

    private void Update()
    {
        cineVirtual.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y;

        if (cineVirtual.m_Lens.OrthographicSize > 29)
        {
            cineVirtual.m_Lens.OrthographicSize = 29;
        }
        else if (cineVirtual.m_Lens.OrthographicSize < 5)
        {
            cineVirtual.m_Lens.OrthographicSize = 5;
        }
    }
}
