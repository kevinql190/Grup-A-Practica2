using System.Collections;
using UnityEngine;

public class SimpleRoom : RoomManager
{
    [SerializeField] private WaveManager waveManager;
    protected override IEnumerator RoomSequence()
    {
        waveManager.enabled = true;
        while (!waveManager.waveEnded) yield return null;
        EndRoom();
    }
}
