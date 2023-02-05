using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TalentInformation : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [TextArea(1, 5)]
    public string Title = "";

    [TextArea(10, 15)]
    public string Description = "";

    [SerializeField] private InformationPanel _infoPanel;


    public void OnSelect(BaseEventData eventData)
    {
        _infoPanel.UpdateInfoText(Title, Description);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _infoPanel.UpdateInfoText("", "");
    }
}
