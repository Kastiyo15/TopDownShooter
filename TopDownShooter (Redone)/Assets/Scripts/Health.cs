using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHp = 100; // maximum HP
    private int _hp; // current HP


    // Exposing the _maxHp field to allow scripts to read it
    public int MaxHp => _maxHp;


    // Make sure _hp always has the correct value, by making a property on top of it to...
    public int Hp
    {
        // ...return _hp
        get => _hp;
        private set
        {
            // Whenever this value is smaller than current hp (_hp), then character must be damaged
            var isDamage = value < _hp;
            // before we assign new value, clamp it between min and max
            _hp = Mathf.Clamp(value, 0, _maxHp);

            // Check if damaged, if so then run Damaged Event
            if (isDamage)
            {
                Damaged?.Invoke(_hp); // Pass new hp value
            }
            else
            {
                Healed?.Invoke(_hp); // if not damaged then we were healed
            }

            // If current health is zero then invoke the Died event
            if (_hp <= 0)
            {
                Died?.Invoke();
            }
        }
    }


    // Add public unity event field, so we can use them in inspector
    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;


    /////////////////////////
    // USING EXPRESSION BODYS
    // void Example(x)
    // {
    //      return x*2;   
    // }
    // Is equivalent to
    // void Example(x) => x*2;
    /////////////////////////


    // Make sure character starts at full HP
    private void Awake() => _hp = _maxHp;

    // Allow other scripts to interact with our health system
    // Set the Hp property, not the underlying field, make sure value is always clamped correctly
    public void Damage(int amount) => Hp -= amount;

    // Add hp to character
    public void Heal(int amount) => Hp += amount;

    // Kill character by removing all health
    public void Kill(int amount) => Hp = 0;

    // Add function to allow adjusting value, setting it to the 'value'
    public void Adjust(int value) => Hp = value;

}
