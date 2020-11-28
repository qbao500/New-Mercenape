using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TempCamScript : MonoBehaviour
{
    public static TempCamScript Instance { get; private set; }

    public CinemachineVirtualCamera cineVirtual;
    private CinemachineFramingTransposer framingTransposer;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        Instance = this;
        framingTransposer = cineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
        channelPerlin = cineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CameraZoom();
    }

    private void CameraZoom()
    {
        cineVirtual.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y;
        cineVirtual.m_Lens.OrthographicSize = Mathf.Clamp(cineVirtual.m_Lens.OrthographicSize, 5, 29);

        framingTransposer.m_TrackedObjectOffset.y += Input.mouseScrollDelta.y * 2;
        framingTransposer.m_TrackedObjectOffset.y = Mathf.Clamp(framingTransposer.m_TrackedObjectOffset.y, -29, 3);
    }

    public void ShakeCamera(float intensity, float frequency)
    {       
        StartCoroutine(ShakeHandle(intensity, frequency));
    }

    private IEnumerator ShakeHandle(float intensity, float frequency)
    {
        float elapse = 0f;

        while (elapse < .1f)
        {
            elapse += Time.deltaTime;
            channelPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, elapse / .1f);
            channelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 0f, elapse / .1f);

            yield return null;
        }
    }
}
