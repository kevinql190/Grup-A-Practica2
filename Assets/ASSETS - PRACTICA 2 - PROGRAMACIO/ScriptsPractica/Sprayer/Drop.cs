using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    Rigidbody _rigidbody;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 1);
        Debug.Log("Caca");
    }

    public void Init(float force, Vector3 launchDirection)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = launchDirection * force;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}