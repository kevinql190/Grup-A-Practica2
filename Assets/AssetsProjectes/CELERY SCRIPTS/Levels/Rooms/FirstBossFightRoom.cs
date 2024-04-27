using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossFightRoom : RoomManager
{
    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [Header("Waves")]
    [SerializeField] private WaveManager firstWave;
    [SerializeField] private WaveManager secondtWave;
    [SerializeField] private GameObject wingPrefab;
    [SerializeField] private Transform wingSpawnPoint;
    [SerializeField] private WaveManager thirdWave;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Transform headSpawnPoint;
    [Header("Pilar")]
    [SerializeField] private GameObject pilarPrefab;
    [SerializeField] private Transform pilarSpawnPoint;
    protected override IEnumerator RoomSequence()
    {
        var boss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        firstWave.enabled = true;
        while (!firstWave.waveEnded) yield return null;
        Debug.Log("1");
        var wing = Instantiate(wingPrefab, wingSpawnPoint.position, wingSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        secondtWave.enabled = true;
        while (!secondtWave.waveEnded) yield return null;
        Debug.Log("2");
        var head = Instantiate(headPrefab, headSpawnPoint.position, headSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        thirdWave.enabled = true;
        while (!thirdWave.waveEnded) yield return null;
        Instantiate(pilarPrefab, pilarSpawnPoint.position, pilarSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        Destroy(boss);
        Destroy(wing);
        Destroy(head);
        EndRoom();
    }
}
