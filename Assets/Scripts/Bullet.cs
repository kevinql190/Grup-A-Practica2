using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int targetCount = 1;
    [SerializeField] private float lifeTimeBullet = 3f;
    public int damage = 1;
    private void Start()
    {
        Destroy(gameObject, lifeTimeBullet);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageReceiver = other.GetComponent<IDamageable>();
        bool shouldDamage = (targetLayers.value & 1 << other.gameObject.layer) != 0;
        if (damageReceiver != null && shouldDamage)
        {
            damageReceiver.TakeDamage(-damage);
            targetCount--;
        }
        if (other.CompareTag("Wall") || targetCount < 1)
        {
            Destroy(gameObject);
        }
    }
}
