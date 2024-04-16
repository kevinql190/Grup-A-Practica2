using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SkillAbilities : MonoBehaviour
{
    [SerializeField] private GameObject skillCanvas;
    [SerializeField] private Transform spawnPoint;
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] private LayerMask raycastMask;
    private Vector3 lookPosition;
    private float targetAngle;
    private CookingSystem _cookingSystem;
    private PanController _panController;
    #region Ability Variables
    [Header("Tomato")]
    [SerializeField] private GameObject tomatoBullet;
    public float speedBulletTomato = 10f;
    [Header("Carrot")]
    [SerializeField] private GameObject carrotBullet;
    public float speedBulletCarrot = 10f;
    #endregion
    #region Ability System
    private void Start()
    {
        _cookingSystem = GetComponent<CookingSystem>();
        _panController = GetComponent<PanController>();
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
            Ability();
            _cookingSystem.skillCasted = true;
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
        spawnPoint.eulerAngles = new Vector3(0, targetAngle, 0);
        skillCanvas.transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }
    private void GetRayPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
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