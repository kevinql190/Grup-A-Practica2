using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    [Header("Dissolve Material")]
    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private Material dissolveShader;
    [SerializeField] private List<GameObject> enterRooms;
    [SerializeField] private List<GameObject> exitRooms;
    private void Awake()
    {
        if (dissolveShader != null) dissolveShader.SetFloat("_Dissolve", 0f);
    }
    virtual public void StartRoom()
    {
        StartCoroutine(RoomEnterAndLeave(true));
        StartCoroutine(RoomSequence());
    }
    virtual protected IEnumerator RoomSequence()
    {
        yield break;
    }
    virtual public void EndRoom()
    {
        StopCoroutine(RoomSequence());
        StartCoroutine(RoomEnterAndLeave(false));
    }
    virtual public void ExitRoom()
    {

    }
    IEnumerator RoomEnterAndLeave(bool isEnter)
    {
        if (isEnter)
        {
            foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
            {
                door.CloseDoor();
            }
            StartCoroutine(Dissolve(true));
            yield return new WaitForSeconds(dissolveTime);
            foreach(GameObject room in enterRooms)
            {
                room.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject room in exitRooms)
            {
                room.SetActive(true);
            }
            StartCoroutine(Dissolve(false));
            yield return new WaitForSeconds(dissolveTime);
            foreach (StartDoor door in transform.GetComponentsInChildren<StartDoor>())
            {
                door.OpenDoor();
            }
        }
    }
    #region Room Exterior Dissolve
    public IEnumerator Dissolve(bool willDisapear)
    {
        float t = 0f;
        while (t < dissolveTime)
        {
            t += Time.deltaTime;
            float value = Mathf.InverseLerp(0f, dissolveTime, t);
            dissolveShader.SetFloat("_Dissolve", willDisapear ? value : (1 - value));
            yield return null;
        }
    }
    #endregion
}
