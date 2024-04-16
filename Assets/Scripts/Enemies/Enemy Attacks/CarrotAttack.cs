using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotAttack : Enemy
{
    [Header("Carrot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Bullet")]
    public float speedBullet = 10f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public override void Attack()
    {
        FacePlayer();
        //Instancia Bullet
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletObj.GetComponent<Bullet>().damage = damage;
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }
    public override void FacePlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}
