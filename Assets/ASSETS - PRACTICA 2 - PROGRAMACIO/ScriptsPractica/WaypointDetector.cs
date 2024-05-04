using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDetector : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_enemy.target == other.gameObject)
        {
            _enemy.target = other.gameObject.GetComponent<WayPoint>().nexPoint.gameObject;
            if (_animator != null)
            {
                _animator.transform.LookAt(new Vector3(_enemy.target.transform.position.x, _animator.transform.position.y, _enemy.target.transform.position.z));
            }
        }
    }
}
