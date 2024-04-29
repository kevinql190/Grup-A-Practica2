using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDispenser : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject prefabToDispense;
    [SerializeField] private float speedBullet;
    public Cooldown dispenserCooldown;
    private void Update()
    {
        if (PlayerInputHandler.AttackJustPressed && !dispenserCooldown.IsCoolingDown) InstantiateBullet(prefabToDispense, speedBullet);
    }
    private void InstantiateBullet(GameObject prefab, float speedBullet)
    {
        GameObject bulletObj = Instantiate(prefab, spawnPoint.transform.position + spawnPoint.forward * 1.155f, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }
}
