using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Debug")]
    [SerializeField] private bool teleportAtStart = true;
    [SerializeField] private int roomsToActivate = 1;
    [SerializeField] GameObject startRoomGameObject;
    [Header("StartFade")]
    [SerializeField] private BlackFade blackFade;
    [SerializeField] private float fadeTime = 1.5f;
    [Header("Rooms")]
    [SerializeField] private List<GameObject> rooms;
    public List<GameObject> checkpoints;
    private GameObject roomToActivate;
    //Level End
    public Action OnLevelEnd;
    [Header("Timer")]
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public bool isTimerON;
    private void Awake()
    {
        elapsedTime = CrossSceneInformation.CurrentTimerValue;
        isTimerON = CrossSceneInformation.CurrentCheckpoint == 0;
        if (checkpoints.Count != 0)
        {
            roomToActivate = checkpoints[CrossSceneInformation.CurrentCheckpoint];
            ActivateRoom(startRoomGameObject != null ? startRoomGameObject : roomToActivate, roomsToActivate);
            if(teleportAtStart) TeleportPlayerToCheckpoint();
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
        StartBlackFade();
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Gameplay");
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (isTimerON) elapsedTime += Time.deltaTime;
    }
    #region Rooms
    private void TeleportPlayerToCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = roomToActivate.transform.Find("Checkpoint").Find("Checkpoint").position;
        player.GetComponent<NavMeshAgent>().enabled = true;
    }
    public void ActivateRoom(GameObject room, int activeRooms = 1)
    {
        int roomToActivate = rooms.IndexOf(room);
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetActive(i >= roomToActivate && i < roomToActivate + activeRooms);
        }
    }
    #endregion
    #region Black Fade
    private void StartBlackFade()
    {
        if (blackFade != null) StartCoroutine(blackFade.FadeFromBlack(fadeTime));
    }
    #endregion
    #region End Level
    public void EndLevel()
    {
        PlayerInputHandler.Instance.LockInputs();
        OnLevelEnd?.Invoke();
    }
    #endregion
}