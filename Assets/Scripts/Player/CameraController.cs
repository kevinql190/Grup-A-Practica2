using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraController : MonoBehaviour
{    
    public float _Zoffset;
    [SerializeField] private float smoothTimeZ;
    [SerializeField] private float dashOffsetZ;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cameraNoise;
    private PlayerMovement target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void FixedUpdate()
    {
        //Si fa dash, canvia la posició final fins que acabi de dashear.
        if (!target.isDashing)
        {
            //Setejar valor Zoffset
            _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(_virtualCamera.m_Lens.OrthographicSize, _Zoffset, smoothTimeZ * Time.deltaTime);
        }
        else
        {
            _virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep(_virtualCamera.m_Lens.OrthographicSize, _Zoffset + dashOffsetZ, smoothTimeZ * Time.deltaTime);
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
