using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public RectTransform handleT;
    public Color backActColor, handleActColor;
    Color backDColor, handleDColor;
    public Image backI, handleI;
    Toggle toggle;
    Vector2 handlePos;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePos = handleT.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitch);
        if (toggle.isOn)
            OnSwitch(true);
        backDColor = backI.color;
        handleDColor = handleI.color;
    }
    void OnSwitch(bool on)
    {
        if (on)
        {
            handleT.anchoredPosition = handlePos * -1;
            handleI.color = handleActColor;
            backI.color = backActColor;
        }
        else
        {
            handleT.anchoredPosition = handlePos;
            handleI.color = handleDColor;
            backI.color = backDColor;
        }
    }
    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
