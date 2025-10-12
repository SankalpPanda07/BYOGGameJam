using UnityEngine;
using Cinemachine;

public class CameraShake2D : MonoBehaviour
{
    public static CameraShake2D Instance;  // Singleton for easy access

    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;
    private float startingIntensity;
    private float totalShakeDuration;

    private void Awake()
    {
        Instance = this;
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();

        if (cinemachineCam != null)
            noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
            Debug.LogWarning("No CinemachineBasicMultiChannelPerlin component found! Add it in the Virtual Camera's Noise settings.");
    }

    /// <summary>
    /// Triggers a camera shake.
    /// </summary>
    /// <param name="intensity">How strong the shake is.</param>
    /// <param name="time">How long the shake lasts.</param>
    public void Shake(float intensity, float time)
    {
        if (noise == null) return;

        noise.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        totalShakeDuration = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (noise == null || shakeTimer <= 0) return;

        shakeTimer -= Time.deltaTime;

        // Gradually reduce shake intensity
        noise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / totalShakeDuration));

        if (shakeTimer <= 0f)
        {
            noise.m_AmplitudeGain = 0f;
        }
    }
}
