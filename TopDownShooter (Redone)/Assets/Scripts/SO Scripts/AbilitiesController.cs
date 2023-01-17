using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: add an array of [unlocked abilities] and an array of [locked abilities]
// When unlock new ability, 'add' the ability to the unlocked abilities array
// Be able to save and load the array!
public class AbilitiesController : MonoBehaviour
{
    public Ability Ability;
    float cooldownTime;
    float activeTime;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;

    public KeyCode key;


    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    Ability.Activate(gameObject);
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
                    Ability.BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = Ability.cooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
        }
    }
}
