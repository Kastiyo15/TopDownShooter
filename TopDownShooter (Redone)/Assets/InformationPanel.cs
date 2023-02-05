using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationPanel : MonoBehaviour
{
    public TMP_Text TitleText;
    public TMP_Text InfoText;


    private void Start()
    {
        UpdateInfoText("", "");
    }


    public void UpdateInfoText(string title, string txt)
    {
        TitleText.text = title;
        InfoText.text = txt;
    }
}
