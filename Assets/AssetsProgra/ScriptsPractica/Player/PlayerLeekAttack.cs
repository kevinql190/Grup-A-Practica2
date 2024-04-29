using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeekAttack : MonoBehaviour
{
    [SerializeField] private float skillPushSpeed;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject leekWeapon;
    [SerializeField] private float leekDistance;
    [SerializeField] private float leekRadiusDegrees = 90;
    public Cooldown skillTime;
    private PlayerMovement _playerMovement;
    private PlayerAnimatorManager _playerAnimatorManager;
    private MeshRenderer _meshRenderer;
    private void Start()
    {
        _playerMovement = transform.root.GetComponent<PlayerMovement>();
        _playerAnimatorManager = transform.root.GetComponent<PlayerAnimatorManager>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (PlayerInputHandler.AttackJustPressed && !skillTime.IsCoolingDown) StartCoroutine(SkillLeek());
    }
    private IEnumerator SkillLeek()
    {
        _meshRenderer.enabled = false;
        _playerAnimatorManager.TriggerAnimation("attack");
        StartCoroutine(_playerMovement.ForwardPush(skillPushSpeed, skillTime.CooldownTime));
        GameObject leek = Instantiate(leekWeapon, spawnPoint.transform.position + spawnPoint.forward * leekDistance, spawnPoint.transform.rotation, spawnPoint.transform);
        leek.transform.RotateAround(transform.position, Vector3.up, -leekRadiusDegrees / 2f);
        float t = 0;
        while (t < skillTime.CooldownTime)
        {
            float rotationAmount = leekRadiusDegrees / skillTime.CooldownTime * Time.deltaTime;
            if (leek != null) leek.transform.RotateAround(spawnPoint.transform.position, Vector3.up, rotationAmount);
            t += Time.deltaTime;
            yield return null;
        }
        _meshRenderer.enabled = true;
        Destroy(leek);
    }
}
