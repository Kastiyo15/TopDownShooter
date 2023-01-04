using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDScript : MonoBehaviour
{
    [SerializeField] private GameObject _panelWeaponsHUD;
    [SerializeField] private List<GameObject> _buttonGameObjects = new List<GameObject>();
    [SerializeField] private List<Sprite> _buttonSprites = new List<Sprite>();

    public void WeaponSelectedHUD(int i)
    {
        // Sprite list will have [Q, E, Q selected, E selected]
        // Set selected sprite
        _buttonGameObjects[i].GetComponent<Image>().sprite = _buttonSprites[i + 2];
        // Set other button to normal sprite
        _buttonGameObjects[1 - i].GetComponent<Image>().sprite =_buttonSprites[1 - i];
    }
}
