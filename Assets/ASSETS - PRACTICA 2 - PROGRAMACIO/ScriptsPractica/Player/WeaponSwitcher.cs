using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int SelectedWeapon
    {
        get { return selectedWeapon; }
        set { selectedWeapon = value; SelectWeapon(); }
    }
    private int selectedWeapon = 0;
    [SerializeField] private List<GameObject> weapons;
    
    void Start()
    {
        SelectWeapon();
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            if (i == SelectedWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
            i++;
        }
    }

    void Update()
    {
        if (PlayerInputHandler.ChangeWeaponInput.y > 0)
        {
            if (SelectedWeapon >= weapons.Count - 1)
                SelectedWeapon = 0;
            else
                SelectedWeapon++;
        }
        if(PlayerInputHandler.ChangeWeaponInput.y < 0)
        {
            if (SelectedWeapon <= 0)
                SelectedWeapon = weapons.Count - 1;
            else
                SelectedWeapon--;
        }
    }
}
