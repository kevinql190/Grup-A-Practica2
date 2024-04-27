using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int targetCount = 1;
    [SerializeField] private float lifeTimeBullet = 3f;
    [SerializeField] private bool destroyWhenHitWall = true;
    public int damage = 1;
    private void Start()
    {
        if (lifeTimeBullet != -1) Destroy(gameObject, lifeTimeBullet);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageReceiver = other.GetComponent<IDamageable>();
        bool shouldDamage = (targetLayers.value & 1 << other.gameObject.layer) != 0;
        if ((targetLayers.value & 1 << LayerMask.NameToLayer("Player")) != 0 && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.GetComponent<PlayerMovement>().isDashing) return; // El dash no afecta els projectils
        }
        if (damageReceiver != null && shouldDamage)
        {
            damageReceiver.TakeDamage(-damage);
            targetCount--;
        }
        if ((other.CompareTag("Wall") && destroyWhenHitWall) || targetCount < 1)
        {
            Destroy(gameObject);
        }
    }
}
