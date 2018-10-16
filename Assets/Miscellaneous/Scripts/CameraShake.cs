using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera m_camera;
    private CinemachineBasicMultiChannelPerlin m_noise;

    public static CameraShake Instance;

    private IEnumerator m_coroutine;

    private void Awake()
    {
        Instance = this;
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_noise = m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {

        m_coroutine = ProcessShake();


        StartCoroutine(m_coroutine);
    }

    private IEnumerator ProcessShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        Noise(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        m_noise.m_AmplitudeGain = amplitudeGain;
        m_noise.m_FrequencyGain = frequencyGain;
    }
}