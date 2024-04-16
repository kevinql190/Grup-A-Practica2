using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Rooms")]
    [SerializeField] private List<GameObject> rooms;
    public List<GameObject> checkpoints;
    GameObject roomToActivate;
    //Timer
    public float elapsedTime;
    public bool isTimerON;
    private void Awake()
    {
        elapsedTime = CrossSceneInformation.CurrentTimerValue;
        isTimerON = CrossSceneInformation.CurrentCheckpoint == 0;
        if (checkpoints.Count != 0)
        {
            roomToActivate = checkpoints[CrossSceneInformation.CurrentCheckpoint];
            ActivateRoom(roomToActivate);
            TeleportPlayerToCheckpoint();
            CheckpointRoom room= checkpoints[CrossSceneInformation.CurrentCheckpoint].GetComponent<CheckpointRoom>();
            if(room != null) room.RespawnSetDoors();
        }
        else
        {
            Debug.LogWarning("No checkpoints in LevelManager");
            ActivateRoom(rooms[0]);
        }
    }

    private void Start()
    {
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Gameplay");
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (isTimerON) elapsedTime += Time.deltaTime;
    }
    private void TeleportPlayerToCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = roomToActivate.transform.Find("Checkpoint").Find("Checkpoint").position;
        player.GetComponent<NavMeshAgent>().enabled = true;
    }
    public void ActivateRoom(GameObject room)
    {
        int roomToActivate = rooms.IndexOf(room);
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetActive(i == roomToActivate); //SetActive true to room and false rooms else
        }
    }
}