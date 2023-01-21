using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DamageNumbersPro;


public class HUDScript : MonoBehaviour
{
    [Header("Weapon Select HUD")]
    [SerializeField] private GameObject _panelWeaponsHUD;
    [SerializeField] private List<GameObject> _buttonGameObjects = new List<GameObject>();
    [SerializeField] private List<Sprite> _buttonSprites = new List<Sprite>();
    [SerializeField] private TMP_Text _txtWeaponName;
    [SerializeField] private TMP_Text _txtWeaponDamage;

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
    [SerializeField] private GameObject _panelScorePopupLocation;
    [SerializeField] private DamageNumber _txtScorePopup;

    [Header("Wave Counter HUD")]
    [SerializeField] private GameObject _panelWaveCounterHUD;
    [SerializeField] private TMP_Text _txtWaveCounter;

    [Header("Enemy Counter HUD")]
    [SerializeField] private TMP_Text _txtEnemyCounter;
    [SerializeField] private TMP_Text _txtEnemyKillsCounter;

    [Header("Player Health HUD")]
    [SerializeField] private Health _scriptPlayerHealth;
    [SerializeField] private GameObject _panelPlayerHealthHUD;
    [SerializeField] private TMP_Text _txtPlayerHealth;


    private void Start()
    {
        DisplayAmmoBars(WeaponStatsManager.Instance.W_WeaponID);

        StartCoroutine(UpdateScoreHUD(0));

        UpdatePlayerHealthHUD();
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

        _txtWeaponName.SetText($"{WeaponStatsManager.Instance.W_WeaponName}");

        int damageValue = BulletStatsManager.Instance.B_BulletDamage;
        int damageMin = damageValue - (Mathf.FloorToInt(damageValue / 10));
        int damageMax = damageValue + (Mathf.FloorToInt(damageValue / 10));

        _txtWeaponDamage.SetText($"{damageMin} - {damageMax}");
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
                //var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentRifleClip);
                var currentClip = (WeaponStatsManager.Instance.W_CurrentRifleClip);
                _txtAmmoCounter.SetText($"HEAT: {currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");

                // Update the bar
                var barFillAmount = _barAmmoForeground.fillAmount;
                _barAmmoForeground.fillAmount = Mathf.Lerp(barFillAmount, (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize, Time.deltaTime * 5f);
            }
            // shotgun
            else if (WeaponStatsManager.Instance.W_WeaponID == 1)
            {
                // Update the text
                //var currentClip = (WeaponStatsManager.Instance.W_WeaponClipSize) + (WeaponStatsManager.Instance.W_CurrentShotgunClip);
                var currentClip = (WeaponStatsManager.Instance.W_CurrentShotgunClip);
                _txtAmmoCounter.SetText($"HEAT: {currentClip}/{WeaponStatsManager.Instance.W_WeaponClipSize}");

                // Update the bar
                var barFillAmount2 = _barAmmoForeground2.fillAmount;
                _barAmmoForeground2.fillAmount = Mathf.Lerp(barFillAmount2, (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize, Time.deltaTime * 5f);
            }
        }
        else if (overheating == 1)
        {
            // Update the text
            _txtAmmoCounter.SetText($"OVERHEATING!");

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
    public IEnumerator UpdateScoreHUD(int score)
    {
        var duration = 0.5f;
        //_txtScoreCounter.SetText($"SCORE: {ScoreStatsManager.Instance.t_runScore}");   
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            var currentScore = Mathf.RoundToInt(Mathf.Lerp(score, ScoreStatsManager.Instance.t_runScore, t / duration));
            currentScore = Mathf.RoundToInt(currentScore);
            _txtScoreCounter.SetText($"{currentScore}");
            yield return null;
        }
        _txtScoreCounter.SetText($"{ScoreStatsManager.Instance.t_runScore}");
    }


    //public void ScorePopupText(int amount)
    //{
    //    _txtScorePopup.Spawn(_panelScorePopupLocation.transform.position + Vector3.up, amount);
    //}
    #endregion


    #region WAVE COUNTER HUD
    public void UpdateWaveHUD(int wave)
    {
        if (!_panelWaveCounterHUD.activeInHierarchy)
        {
            _panelWaveCounterHUD.SetActive(true);
        }
        //_txtWaveCounter.gameObject.SetActive(true);
        _txtWaveCounter.SetText($"{wave}");
    }
    #endregion


    #region ENEMY COUNTER HUD
    public void UpdateKillsRemaining(int amount)
    {
        _txtEnemyCounter.SetText($"{amount}");
    }


    public void UpdateKillsThisRun()
    {
        _txtEnemyKillsCounter.SetText($"{CareerStatsManager.Instance.t_EnemiesKilled}");
    }
    #endregion


    #region PLAYER HEALTH HUD
    public void UpdatePlayerHealthHUD()
    {
        _txtPlayerHealth.SetText($"HP: {_scriptPlayerHealth.Hp}/{_scriptPlayerHealth.MaxHp}");
    }
    #endregion


    #region ABILITY BARS HUD
    public void AbilityBarCoolDown(Image coolDownBar, Image abilityIcon, float coolDownTimer, Image activeMarker, TMP_Text hotkeyText)
    {
        hotkeyText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        coolDownBar.fillAmount = Mathf.Lerp(0, 1, coolDownTimer);
        abilityIcon.color = Color.grey;
        activeMarker.color = new Color(0.2f, 0.2f, 0.2f, 1f);
    }


    public void AbilityBarReady(Image coolDownBar, Image abilityIcon, Sprite abilitySprite, Image activeMarker, TMP_Text hotkeyText, KeyCode hotkey)
    {
        hotkeyText.SetText($"{hotkey}");
        hotkeyText.color = new Color(1f, 1f, 1f, 0.75f);
        coolDownBar.fillAmount = 0f;
        abilityIcon.sprite = abilitySprite;
        abilityIcon.color = Color.white;
        activeMarker.color = new Color(0f, 0.48f, 0.84f, 1f);
    }
    #endregion
}
