using UnityEngine;
using UnityEngine.UI;
using TMPro;


// TODO: add an array of [unlocked abilities] and an array of [locked abilities]
// When unlock new ability, 'add' the ability to the unlocked abilities array
// Be able to save and load the array!
public class AbilitiesController : MonoBehaviour
{
    public Ability Ability;
    float cooldownTime;
    float activeTime;
    public KeyCode key;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;


    [SerializeField] private GameObject PlayerGameObject;
    [SerializeField] private HUDScript HUD;
    [SerializeField] private Image cooldownBar;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TMP_Text hotkeyText;
    [SerializeField] private Image activeMarker;



    private void Start()
    {
        if (abilityIcon.sprite == null)
        {
            HUD.AbilityBarReady(cooldownBar, abilityIcon, Ability.icon, activeMarker, hotkeyText, key);
        }
    }


    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    Ability.Activate(PlayerGameObject);
                    state = AbilityState.active;
                    activeTime = Ability.activeTime;
                }
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    Ability.BeginCooldown(PlayerGameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = Ability.cooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                    HUD.AbilityBarCoolDown(cooldownBar, abilityIcon, cooldownTime, activeMarker, hotkeyText);
                }
                else
                {
                    state = AbilityState.ready;
                    HUD.AbilityBarReady(cooldownBar, abilityIcon, Ability.icon, activeMarker, hotkeyText, key);
                }
                break;
        }
    }
}
