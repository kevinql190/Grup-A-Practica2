using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAbilities : MonoBehaviour
{
    [SerializeField] private GameObject skillCanvas;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float skillPushSpeed;
    public Cooldown skillTime;
    private Ray ray;
    private RaycastHit hit;
    [Header("Raycast layers")]
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private LayerMask enemyLayer;
    private Vector3 lookPosition;
    private float targetAngle;
    private CookingSystem _cookingSystem;
    private PanController _panController;
    private PlayerMovement _playerMovement;
    #region Ability Variables
    [Header("Tomato")]
    [SerializeField] private GameObject tomatoBullet;
    [SerializeField] private float speedBulletTomato = 10f;
    [Header("Carrot")]
    [SerializeField] private GameObject carrotBullet;
    [SerializeField] private float speedBulletCarrot = 10f;
    [Header("Leek")]
    [SerializeField] private GameObject leekWeapon;
    [SerializeField] private float leekDistance;
    [SerializeField] private float leekRadiusDegrees = 90;
    #endregion
    #region Ability System
    private void Start()
    {
        _cookingSystem = GetComponent<CookingSystem>();
        _panController = GetComponent<PanController>();
        _playerMovement = GetComponent<PlayerMovement>();
        skillCanvas.SetActive(false);
    }
    private void Update()
    {
        if (!_cookingSystem.cooked || _cookingSystem.skillCasted) return;
        if (IsUsingGamepad())
        {
            Cursor.lockState = CursorLockMode.Locked;
            HandleSkillAim();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        RotateCanvas();
        HandleSkill();
    }
    private void HandleSkill()
    {
        if (PlayerInputHandler.AttackJustPressed)
        {
            transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
            StartCoroutine(_playerMovement.ForwardPush(skillPushSpeed, skillTime.CooldownTime));
            GetComponent<PlayerAnimatorManager>().TriggerAnimation("attack");
            spawnPoint.eulerAngles = new Vector3(0, targetAngle, 0);
            Ability();
            _cookingSystem.skillCasted = true;
            skillTime.StartCooldown();
            SetAbility(false);
        }
    }
    private void HandleSkillAim()
    {
        if (PlayerInputHandler.SkillAimInput.x != 0) lookPosition.x = PlayerInputHandler.SkillAimInput.x;
        if (PlayerInputHandler.SkillAimInput.y != 0) lookPosition.y = PlayerInputHandler.SkillAimInput.y;
    }
    public void SetAbility(bool value)
    {
        _cookingSystem.cooked = value;
        skillCanvas.GetComponentInChildren<Image>().sprite = _panController.CurrentSkillSprite;
        skillCanvas.SetActive(value);
        if (value == true)
        {
            _cookingSystem.skillCasted = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void RotateCanvas()
    {
        if (IsUsingGamepad())
        {
            if (Mathf.Abs(lookPosition.x) != 0 && Mathf.Abs(lookPosition.y) != 0)
            {
                targetAngle = - Mathf.Rad2Deg * Mathf.Atan2(-lookPosition.y, -lookPosition.x);
            }
        }
        else
        {
            GetRayPosition();
        }
        /*Vector3 direction = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        Ray ray = new(transform.position, direction);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            targetAngle = Quaternion.LookRotation(hit.transform.position - transform.position).eulerAngles.y;
        }*/
        spawnPoint.eulerAngles = new Vector3(0, targetAngle, 0);
        skillCanvas.transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }
    private void GetRayPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
        {
            lookPosition = new Vector3(hit.point.x, 0, hit.point.z);
        }
        targetAngle = Quaternion.LookRotation(lookPosition - transform.position).eulerAngles.y;
    }
    #endregion
    #region Abilities
    private void Ability()
    {
        switch (_panController.currentFoodType)
        {
            case FoodType.Default:
                break;
            case FoodType.Tomato:
                SkillTomato();
                break;
            case FoodType.Carrot:
                SkillCarrot();
                break;
            case FoodType.Leek:
                StartCoroutine(SkillLeek());
                break;
        }
    }
    private void SkillTomato()
    {
        InstantiateBullet(tomatoBullet, speedBulletTomato);
    }
    private void SkillCarrot()
    {
        InstantiateBullet(carrotBullet, speedBulletCarrot);
    }
    private IEnumerator SkillLeek()
    {
        GameObject leek = Instantiate(leekWeapon, spawnPoint.transform.position + spawnPoint.forward * leekDistance, spawnPoint.transform.rotation, spawnPoint.transform);
        leek.transform.RotateAround(transform.position, Vector3.up, -leekRadiusDegrees/2f);
        float t = 0;
        while (t < skillTime.CooldownTime)
        {
            float rotationAmount = leekRadiusDegrees / skillTime.CooldownTime * Time.deltaTime;
            if (leek != null) leek.transform.RotateAround(spawnPoint.transform.position, Vector3.up, rotationAmount);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(leek);
    }
    private void InstantiateBullet(GameObject prefab, float speedBullet)
    {
        GameObject bulletObj = Instantiate(prefab, spawnPoint.transform.position + spawnPoint.forward * 1.155f, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }
    #endregion
    private bool IsUsingGamepad()
    {
        return PlayerInputHandler.CurrentControlScheme != "Keyboard Mouse";
    }
}