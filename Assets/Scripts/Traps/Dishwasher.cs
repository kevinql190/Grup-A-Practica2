using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishwasher : MonoBehaviour
{
    [Header("Dishwasher")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public GameObject target;
    [SerializeField] public float timeBetweenShoot = 2f;
    public float attackRange = 5f;
    private float timeSinceLastShot;

    [Header("Dish")]
    public int damage = 1;
    public float speedBullet = 10f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        if (InRange())
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= timeBetweenShoot)
            {
                Attack();
                timeSinceLastShot = 0f;
            }
        }
    }
    public bool InRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange;
    }
    public void Attack()
    {
        //Instancia Bullet
        Quaternion bulletRotation = Quaternion.Euler(-90, 0, 0);

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation * bulletRotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletObj.GetComponent<Bullet>().damage = damage;
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }
}
