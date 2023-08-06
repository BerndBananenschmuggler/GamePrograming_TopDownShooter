using Assets.Scripts.Player;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalAmmoText;
    [SerializeField] private TextMeshProUGUI _magAmmoText;
    [SerializeField] private PlayerAttack _playerAttack;

    private RangeWeapon _activeWeapon;

    private void Start()
    {
        // Null Check
        if (_playerAttack == null)
            Debug.LogWarning("PlayerAttack is missing");
        if (_totalAmmoText == null)
            Debug.LogWarning("TotalAmmoText is missing");
        if (_magAmmoText == null)
            Debug.LogWarning("MagAmmoText is missing");

        // Get Active Weapon
        _activeWeapon = _playerAttack.GetEquippedWeapon();

        // Sub Attack event and with UIUpdate
        _playerAttack.OnAttacked += HandlePlayerAmmoChanged;
        _playerAttack.OnReloaded += HandlePlayerAmmoChanged;

        // Initial Update
        UpdateAmmoDisplay();
    }

    private void HandlePlayerAmmoChanged()
    {
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        _totalAmmoText.text = $"Total Ammo: {_activeWeapon.CurrentAmmo} / {_activeWeapon.MaxAmmo}";
        _magAmmoText.text = $"Current Mag: {_activeWeapon.CurrentMagFill} / {_activeWeapon.MaxMagSize}";
    }
}
