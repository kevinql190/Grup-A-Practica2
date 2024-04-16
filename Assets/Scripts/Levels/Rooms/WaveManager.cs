using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    #region variable classes
    [System.Serializable]
    public class Wave
    {
        // Lista de enemigos en la oleada
        public List<Group> enemyGroups;
    }
    [System.Serializable]
    public class Group
    {
        public List<EnemyWaveData> enemy;
        public float respawnCooldown; // Retardo entre cada aparición 
        [HideInInspector] public bool isRespawning;
    }
    [System.Serializable]
    public class EnemyWaveData
    {
        public GameObject enemyPrefab; // Prefab del enemigo
        public Transform spawnPoint; // Punto de aparición
    }
    #endregion

    private int currentWaveIndex = -1; // Índice de la oleada actual
    private bool isSpawning = false;
    public bool waveEnded = false;
    private GameObject enemiesParent;
    private void Awake()
    {
        enabled = false;
    }
    void OnEnable()
    {
        enemiesParent = new("RoomEnemies");
        enemiesParent.transform.parent = transform;
        enemiesParent.transform.SetAsFirstSibling();
        StartNextWave();
    }
    private void OnDisable()
    {
        Destroy(enemiesParent);
    }
    void Update()
    {
        if (waves.Count == 0 || currentWaveIndex >= waves.Count) return;
        CheckRespawn();
        if (!isSpawning && CheckNextWave())
        {
            StartNextWave();
            return;
        }
    }
    private bool CheckNextWave()
    {
        foreach (Transform child in enemiesParent.transform)
        {
            if (child.childCount != 0) return false;
        }
        return true;
    }
    private void CheckRespawn()
    {
        for (int i = 1; i < enemiesParent.transform.childCount; i++)
        {
            if (waves[currentWaveIndex].enemyGroups[i].isRespawning) return;
            if (enemiesParent.transform.GetChild(i).childCount == 0 && enemiesParent.transform.GetChild(i-1).childCount != 0)
            {
                string parentName = i.ToString();
                StartCoroutine(RespawnCooldown(waves[currentWaveIndex].enemyGroups[i], parentName, i));
            }
            else if(enemiesParent.transform.GetChild(i).childCount == 0)
            {
                Destroy(enemiesParent.transform.Find(i.ToString()).gameObject);
            }
        }
    }
    void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            isSpawning = true;
            StartCoroutine(SpawnAllGroups(waves[currentWaveIndex].enemyGroups));
        }
        else
        {
            waveEnded = true;
        }
    }
    IEnumerator SpawnAllGroups(List<Group> enemyGroup)
    {
        yield return new WaitForSeconds(1f);
        foreach (Group group in enemyGroup)
        {
            CreateGroupEmptyObject(enemyGroup.IndexOf(group));
            SpawnGroup(group, enemyGroup.IndexOf(group).ToString());
            yield return null;
        }
        isSpawning = false;
    }

    private void SpawnGroup(Group group, string parentName)
    {
        Transform parent = enemiesParent.transform.Find(parentName);
        foreach (var enemy in group.enemy)
        {
            Instantiate(enemy.enemyPrefab, enemy.spawnPoint.position, enemy.spawnPoint.rotation, parent);
        }
    }
    IEnumerator RespawnCooldown(Group group, string parentName, int child)
    {
        group.isRespawning = true;
        yield return new WaitForSeconds(group.respawnCooldown);
        if (waveEnded) yield break;
        if (enemiesParent.transform.GetChild(child).childCount == 0 && enemiesParent.transform.GetChild(child - 1).childCount != 0)
        {
            SpawnGroup(group, parentName);
        }
        group.isRespawning = false;
    }

    private void CreateGroupEmptyObject(int groupIndex)
    {
        if (enemiesParent.transform.Find(groupIndex.ToString()) == null)
        {
            GameObject newObject = new(groupIndex.ToString());
            newObject.transform.parent = enemiesParent.transform;
        }
    }
}
