// Have added the camera shake function to the playershoot function!
using UnityEngine;
using Cinemachine;


public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private float _shakeTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }


    public void ShakeCamera(float intensity, float time)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }


    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;

            if (_shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
