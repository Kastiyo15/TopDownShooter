using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDScript : MonoBehaviour
{
    [Header("Weapon Select HUD")]
    [SerializeField] private GameObject _panelWeaponsHUD;
    [SerializeField] private List<GameObject> _buttonGameObjects = new List<GameObject>();
    [SerializeField] private List<Sprite> _buttonSprites = new List<Sprite>();

    [Header("Ammo Counter HUD")]
    [SerializeField] private GameObject _panelAmmoCounterHUD;
    [SerializeField] private TMP_Text _txtAmmoCounter;
    [SerializeField] private Image _barAmmoBackground;
    [SerializeField] private Image _barAmmoForeground;


    // Set the selected sprite on screen to display which gun is equipped
    // Sprite list will have [Q, E, Q selected, E selected]
    public void WeaponSelectedHUD(int i)
    {
        // Set selected sprite
        _buttonGameObjects[i].GetComponent<Image>().sprite = _buttonSprites[i + 2];
        // Set other button to normal sprite
        _buttonGameObjects[1 - i].GetComponent<Image>().sprite = _buttonSprites[1 - i];
    }


    // Update the ammo counter HUD, including the bar
    public void AmmoCounterHUD()
    {
        // check which weapon is equipped
        // rifle
        if (WeaponStatsManager.Instance.W_WeaponID == 0)
        {
            // Update the text
            var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentRifleClip);
            _txtAmmoCounter.SetText($"{currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");

            // Update the bar
            _barAmmoForeground.fillAmount = (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize;
        }
        // shotgun
        else if (WeaponStatsManager.Instance.W_WeaponID == 1)
        {
            // Update the text
            var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentShotgunClip);
            _txtAmmoCounter.SetText($"{currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");

            // Update the bar
            _barAmmoForeground.fillAmount = (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize;

        }
    }
}
