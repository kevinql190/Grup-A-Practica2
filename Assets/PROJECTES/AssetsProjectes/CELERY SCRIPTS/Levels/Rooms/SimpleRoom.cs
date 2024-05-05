using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

public class SimpleRoom : RoomManager
{
    [Header("Simple Room")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] CinemachineVirtualCamera startRoomCamera;
    [SerializeField] private float startRoomTime;
    protected override IEnumerator RoomSequence()
    {
        if(startRoomCamera != null) yield return StartCoroutine(ChangeCameraStart());
        waveManager.enabled = true;
        while (!waveManager.waveEnded) yield return null;
        EndRoom();
    }

    private IEnumerator ChangeCameraStart()
    {
        startRoomCamera.enabled = true;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(startRoomTime);
        Time.timeScale = 1;
        startRoomCamera.enabled = false;
    }

    public override void EndRoom()
    {
        base.EndRoom();
        foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
        {
            door.ChangeCamera();
        }
    }
}
