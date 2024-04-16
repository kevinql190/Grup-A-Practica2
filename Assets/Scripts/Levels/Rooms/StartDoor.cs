using UnityEngine;
using UnityEngine.AI;

public class StartDoor : MonoBehaviour
{
    [SerializeField] private Animator toastDoorObject;
    [SerializeField] private bool opensAtEnd;
    public void CloseDoor()
    {
        GetComponent<NavMeshObstacle>().enabled = true;
        toastDoorObject.SetTrigger("startclose");
    }
    public void OpenDoor()
    {
        if (opensAtEnd)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
            toastDoorObject.SetTrigger("open");
        }
        else
        {
            toastDoorObject.SetTrigger("exit");
        }
    }
    public void StopAnim()
    {
        toastDoorObject.SetTrigger("exit");
    }
    public void SetDoorCheckpoint()
    {
        if (opensAtEnd) toastDoorObject.SetTrigger("open");
        else toastDoorObject.SetTrigger("close");
    }
    private void OnDrawGizmos()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        Gizmos.color = new Color(.2f, .5f, .5f, .2f);
        if (obstacle != null) Gizmos.DrawCube(transform.position + obstacle.center, Vector3.Scale(obstacle.size, transform.localScale));
    }
}
