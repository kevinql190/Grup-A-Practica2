using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraController : MonoBehaviour
{    
    private float _Zoffset;
    [SerializeField] private float smoothInZ;
    [SerializeField] private float smoothOutZ;
    [SerializeField] private float dashOffsetZ;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cameraNoise;
    private PlayerMovement target;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _Zoffset = _virtualCamera.m_Lens.OrthographicSize;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _virtualCamera.Follow = target.transform;
        _cameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void FixedUpdate()
    {
        //Si fa dash, canvia la posició final fins que acabi de dashear.
        if (!target.isDashing)
        {
            //Setejar valor Zoffset
            _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(_virtualCamera.m_Lens.OrthographicSize, _Zoffset, smoothOutZ * Time.deltaTime);
        }
        else
        {
            _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(_virtualCamera.m_Lens.OrthographicSize, _Zoffset + dashOffsetZ, smoothInZ * Time.deltaTime);
        }
    }
    public void CameraShake(float intensity, float duration)
    {
        StartCoroutine(DoCameraShake(intensity, duration));
    }
    IEnumerator DoCameraShake(float intensity, float duration)
    {
        _cameraNoise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        _cameraNoise.m_AmplitudeGain = 0;
    }
}
