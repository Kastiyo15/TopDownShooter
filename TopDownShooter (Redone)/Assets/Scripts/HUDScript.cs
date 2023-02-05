using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DamageNumbersPro;


public class HUDScript : MonoBehaviour
{
    [Header("Weapon Select HUD")]
    [SerializeField] private TMP_Text _txtWeaponName;
    [SerializeField] private TMP_Text _txtWeaponDamage;
    [SerializeField] private TMP_Text _txtSwitchButton;

    [Header("Ammo Counter HUD")]
    [SerializeField] private Image _barAmmoBackground;
    [SerializeField] private Image _barAmmoForeground; // rifle
    [SerializeField] private Image _barAmmoForeground2; // shotgun
    [SerializeField] private Image _barAmmoOverheat; // rifle
    [SerializeField] private Image _barAmmoOverheat2; // shotgun

    [Header("Score Counter HUD")]
    [SerializeField] private TMP_Text _txtScoreCounter;
    [SerializeField] private DamageNumber _txtScorePopup;
    [SerializeField] private float _displayScore;
    [SerializeField] private float _roundedScore;
    [SerializeField] private float _transitionSpeed = 100f;

    [Header("Enemy Counter HUD")]
    [SerializeField] private TMP_Text _txtEnemyCounter;
    [SerializeField] private TMP_Text _txtEnemyKillsCounter;

    [Header("Player Health HUD")]
    [SerializeField] private Health _scriptPlayerHealth;
    [SerializeField] private GameObject _panelPlayerHealthHUD;
    [SerializeField] private TMP_Text _txtPlayerHealth;

    [Header("HUD Groups")]
    [SerializeField] private GameObject _uiHUD;


    private void Start()
    {
        DisplayAmmoBars(WeaponStatsManager.Instance.W_WeaponID);

        UpdatePlayerHealthHUD();

        StartCoroutine("UpdateScoreHUD");

        HideHUD();
    }


    private void HideHUD()
    {
        _uiHUD.SetActive(false);
    }


    public void ShowHUD()
    {
        _uiHUD.SetActive(true);
    }


    #region WEAPON SELECT HUD
    // Set the selected sprite on screen to display which gun is equipped
    // Sprite list will have [Q, E, Q selected, E selected]
    public void WeaponSelectedHUD(int i)
    {
        _txtWeaponName.SetText($"{WeaponStatsManager.Instance.W_WeaponName}");

        int damageValue = BulletStatsManager.Instance.B_BulletDamage;
        int damageMin = damageValue - (Mathf.FloorToInt(damageValue / 10));
        int damageMax = damageValue + (Mathf.FloorToInt(damageValue / 10));

        _txtWeaponDamage.SetText($" {damageMin} - {damageMax}");

        switch (i)
        {
            case (0):
                _txtSwitchButton.SetText($"E");
                break;
            case (1):
                _txtSwitchButton.SetText($"Q");
                break;
        }
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
                var currentClip = (WeaponStatsManager.Instance.W_CurrentRifleClip);

                // Update the bar
                var barFillAmount = _barAmmoForeground.fillAmount;
                _barAmmoForeground.fillAmount = Mathf.Lerp(barFillAmount, (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize, Time.deltaTime * 5f);
            }
            // shotgun
            else if (WeaponStatsManager.Instance.W_WeaponID == 1)
            {
                // Update the text
                var currentClip = (WeaponStatsManager.Instance.W_CurrentShotgunClip);

                // Update the bar
                var barFillAmount2 = _barAmmoForeground2.fillAmount;
                _barAmmoForeground2.fillAmount = Mathf.Lerp(barFillAmount2, (float)currentClip / (float)WeaponStatsManager.Instance.W_WeaponClipSize, Time.deltaTime * 5f);
            }
        }
        else if (overheating == 1)
        {
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
    public IEnumerator UpdateScoreHUD()
    {
        while (true)
        {
            _transitionSpeed = (ScoreStatsManager.Instance.t_runScore - _displayScore) * 5f;
            _displayScore = Mathf.MoveTowards(_displayScore, ScoreStatsManager.Instance.t_runScore, (2 * _transitionSpeed) * Time.deltaTime);
            _roundedScore = Mathf.RoundToInt(_displayScore);
            _txtScoreCounter.SetText($"{_roundedScore}");
            //_txtScoreCounter.text = string.Format("{0:000000000000}", _roundedScore);
            yield return null;
        }
    }

    // Set the fill amount to current xp value
    public void UpdateScoreBar(ScoreStatsManager.ScoreBar data, float duration)
    {
        data.MultiplierText.SetText($"x{ScoreStatsManager.Instance._currentMultiplier}");
        data.Foreground.fillAmount = (float)ScoreStatsManager.Instance._currentScore / (float)ScoreStatsManager.Instance._requiredScore;

        if (data.SlowBar.fillAmount != data.Foreground.fillAmount)
        {
            data.SlowBar.fillAmount = Mathf.Lerp(data.SlowBar.fillAmount, data.Foreground.fillAmount, Mathf.Pow(duration, 2f));
        }
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
