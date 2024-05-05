using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRoom : RoomManager
{
    public override void StartRoom()
    {
        base.StartRoom();
        LevelManager.Instance.isTimerON = false;
        StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().ResetHearts());
    }
    public override void ExitRoom()
    {
        base.ExitRoom();
        LevelManager.Instance.isTimerON = true;
    }
    public void RespawnSetDoors()
    {
        foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
        {
            door.CloseDoor();
        }
    }
}
