using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] private GameObject breakableObjectPrefab;
    [SerializeField] public float destroyDelay = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Quaternion rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject breakPrefab = Instantiate(breakableObjectPrefab, transform.position, rotation);
            Destroy(breakPrefab, destroyDelay);
            GetComponent<Collider>().enabled = false;
        }
    }
}
