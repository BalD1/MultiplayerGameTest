using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    [SerializeField] private string waitText;

    [SerializeField] private Color baseColor = Color.white;

    public string WaitText { get => waitText; }

    public void SetText(string _text, Color? _color = null)
    {
        this.gameObject.SetActive(true);
        displayText.text = _text;
        displayText.color = _color.HasValue ? _color.Value : displayText.color;
    }

    public void ResetObject()
    {
        displayText.text = "";
        displayText.color = baseColor;
        this.gameObject.SetActive(false);
    }
}
