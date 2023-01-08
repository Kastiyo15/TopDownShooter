using System.Collections.Generic;
using System.Collections;
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
    [SerializeField] private Image _barAmmoForeground; // rifle
    [SerializeField] private Image _barAmmoForeground2; // shotgun
    [SerializeField] private Image _barAmmoOverheat; // rifle
    [SerializeField] private Image _barAmmoOverheat2; // shotgun

    [Header("Score Counter HUD")]
    [SerializeField] private GameObject _panelScoreCounterHUD;
    [SerializeField] private TMP_Text _txtScoreCounter;


    private void Start()
    {
        DisplayAmmoBars(WeaponStatsManager.Instance.W_WeaponID);

        UpdateScoreHUD();
    }


    #region WEAPON SELECT HUD
    // Set the selected sprite on screen to display which gun is equipped
    // Sprite list will have [Q, E, Q selected, E selected]
    public void WeaponSelectedHUD(int i)
    {
        // Set selected sprite
        _buttonGameObjects[i].GetComponent<Image>().sprite = _buttonSprites[i + 2];
        // Set other button to normal sprite
        _buttonGameObjects[1 - i].GetComponent<Image>().sprite = _buttonSprites[1 - i];
    }
    #endregion


    #region AMMO COUNTER HUD
    // Update the ammo counter HUD, including the bar
    public void AmmoCounterHUD(int overheating)
    {
        // if not overheating run this section
        if (overheating == 0)
        {
            // check which weapon is equipped
            // rifle
            if (WeaponStatsManager.Instance.W_WeaponID == 0)
            {
                // Update the text
                var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentRifleClip);
                _txtAmmoCounter.SetText($"{currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");
                _txtAmmoCounter.color = Color.white;

                // Update the bar
                _barAmmoForeground.fillAmount = (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize;
            }
            // shotgun
            else if (WeaponStatsManager.Instance.W_WeaponID == 1)
            {
                // Update the text
                var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentShotgunClip);
                _txtAmmoCounter.SetText($"{currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");
                _txtAmmoCounter.color = Color.white;

                // Update the bar
                _barAmmoForeground2.fillAmount = (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize;
            }
        }
        else if (overheating == 1)
        {
            // Update the text
            _txtAmmoCounter.SetText($"{0f}/{WeaponStatsManager.Instance.W_WeaponClipSize}");
            _txtAmmoCounter.color = new Color(0.6078f, 0.1843f, 0.2235f, 1f);

            // Check which weapon is equipped
            if (WeaponStatsManager.Instance.W_WeaponID == 0)
            {
                // Update the bar
                _barAmmoForeground.fillAmount = 0f;
            }
            // shotgun
            else if (WeaponStatsManager.Instance.W_WeaponID == 1)
            {
                // Update the bar
                _barAmmoForeground2.fillAmount = 0f;
            }
        }
    }


    // Toggle the Activity of ammo bars depending on weapon selection
    public void DisplayAmmoBars(int id)
    {
        if (id == 0)
        {
            _barAmmoForeground.gameObject.SetActive(true);
            _barAmmoForeground2.gameObject.SetActive(false);
            _barAmmoOverheat.gameObject.SetActive(true);
            _barAmmoOverheat2.gameObject.SetActive(false);
        }
        else if (id == 1)
        {
            _barAmmoForeground.gameObject.SetActive(false);
            _barAmmoForeground2.gameObject.SetActive(true);
            _barAmmoOverheat.gameObject.SetActive(false);
            _barAmmoOverheat2.gameObject.SetActive(true);
        }
    }


    // For loop to lerp individual overheat bars
    public IEnumerator ActivateOverheatBar(int overheatID, float duration)
    {
        if (overheatID == 0)
        {
            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                _barAmmoOverheat.fillAmount = Mathf.Lerp(1, 0, t / duration);
                yield return null;
            }
        }
        else if (overheatID == 1)
        {
            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                _barAmmoOverheat2.fillAmount = Mathf.Lerp(1, 0, t / duration);
                yield return null;
            }
        }
    }
    #endregion


    #region SCORE COUNTER HUD
    public void UpdateScoreHUD()
    {
        _txtScoreCounter.SetText($"SCORE: {ScoreStatsManager.Instance.t_runScore}");
    }
    #endregion


}
